using Verdure.Core;
using Verdure.Data.Mongo;
using Verdure.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.UseMongoDbPersistence(configureOptions =>
{
    var config = builder.Configuration.GetSection("MongoConnectString");

    config.Bind(configureOptions);
});

builder.Services.AddScoped<IArticleService, ArticleService>();

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
