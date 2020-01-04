using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GreenShade.Blog.Api.Services;
using GreenShade.Blog.DataAccess.Services;
using GreenShade.Blog.Domain.Dto;
using GreenShade.Blog.Domain.Models;
using GreenShade.Blog.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GreenShade.Blog.Api.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ThirdLoginService _thirdLogin;
        private readonly JwtSeetings _jwtSeetings;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
             ThirdLoginService thirdLogin,
            IOptions<JwtSeetings> jwtSeetingsOptions
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _thirdLogin = thirdLogin;
            _jwtSeetings = jwtSeetingsOptions.Value;
        }

        [HttpPost("account/login")]
        public async Task<IActionResult> SignIn([FromBody]LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
              
                var claims = new Claim[]
               {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.NameIdentifier,user.Id)
               };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSeetings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _jwtSeetings.Issuer,
                    _jwtSeetings.Audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddHours(30),
                    creds
                    );
                var userDto = new UserInfoDto(user);
                userDto.JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(userDto);
                //return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return BadRequest();
        }



        [HttpPost("account/register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {

            var userInfo = new ApplicationUser { NickName = model.NickName, UserName = model.Email, Email = model.Email, SecurityStamp = "FS" };
            var result = await _userManager.CreateAsync(userInfo, model.Password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var claims = new Claim[]
               {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.NameIdentifier,user.Id)
               };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSeetings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _jwtSeetings.Issuer,
                    _jwtSeetings.Audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    creds
                    );
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return BadRequest();
        }

        [HttpPost("account/third_login")]
        public async Task<IActionResult> ThirdLgin([FromBody]ThirdLoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Code))
            {
                return BadRequest();
            }

            else
            {
                if (model.ThirdType == (int)LoginType.QQ)
                {
                    var user = await _thirdLogin.QQLogin(model.Code);

                    if (user != null)
                    {
                        var userInDb = await _userManager.FindByIdAsync(user.Id);
                        if (userInDb != null && !string.IsNullOrEmpty(userInDb.Id))
                        {
                            var claims = new Claim[]
              {
                    new Claim(ClaimTypes.Name,userInDb.UserName),
                    new Claim(ClaimTypes.NameIdentifier,userInDb.Id)
              };
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSeetings.SecretKey));
                            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                            var token = new JwtSecurityToken(
                                _jwtSeetings.Issuer,
                                _jwtSeetings.Audience,
                                claims,
                                DateTime.Now,
                                DateTime.Now.AddMinutes(30),
                                creds
                                );
                            var userDto = new UserInfoDto(userInDb);
                            userDto.JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                            return Ok(userDto);
                        }
                    }
                   
                    return Ok(user);
                }
            }
            return Ok(null);
        }


        [HttpGet("account/get_user")]
        public async Task<IActionResult> GetLgin(string userId)
        {          
            var user = await _userManager.FindByIdAsync(userId);
            var userDto = new UserInfoDto(user);
            return Ok(userDto);
        }
    }
}