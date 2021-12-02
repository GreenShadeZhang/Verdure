﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GreenShade.Blog.Api.Filters;
using GreenShade.Blog.Api.Hubs;
using GreenShade.Blog.Api.Services;
using GreenShade.Blog.DataAccess.Data;
using GreenShade.Blog.DataAccess.Services;
using GreenShade.Blog.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace GreenShade.Blog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            });
            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("OffLineNpgSqlCon")));
            services.AddDbContext<BlogSysContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("OffLineNpgSqlCon")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();
            services.AddSignalR();
            services.Configure<JwtSeetings>(Configuration.GetSection("JwtSeetings"));
            services.Configure<QQLoginSetting>(Configuration.GetSection("qqlogin"));
            services.Configure<Dictionary<string, WnsSetting>>(Configuration.GetSection("wns"));
            services.AddHttpClient<ThirdLoginService>();
            services.AddScoped<ArticleService>();
            services.AddScoped<BlogManageService>();
            services.AddHttpClient<WallpaperService>();
            services.AddHttpClient<PushWnsService>();
            //services.AddScoped<ThirdLoginService>();
            var jwtSeetings = new JwtSeetings();
            //绑定jwtSeetings
            Configuration.Bind("JwtSeetings", jwtSeetings);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSeetings.Issuer,
                    ValidAudience = jwtSeetings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSeetings.SecretKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken) &&
                            (context.HttpContext.WebSockets.IsWebSocketRequest || context.Request.Headers["Accept"] == "text/event-stream"))
                        {
                            context.Token = context.Request.Query["access_token"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin() //允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://192.168.1.109:4200", "http://localhost:4200", "http://192.168.1.103:4200",
                    "http://192.168.1.103:4200", "http://192.168.16.67:4200", "http://192.168.16.138:4200", "https://www.douwp.club")
                    .AllowCredentials()//指定处理cookie
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(60));
                });
            });
            services.AddControllers(options =>
            {
                options.Filters.Add(new ExceptionHandleAttribute());//根据实例注入过滤器
            });
            //services.AddControllers();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors("any");
            app.UseWebSockets();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
