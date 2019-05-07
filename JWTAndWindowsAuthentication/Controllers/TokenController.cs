using JWTAndWindowsAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAndWindowsAuthentication.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateTokenBearerAuth([FromBody]LoginModel login)
        {

            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(tokenString);
            }

            return response;
        }

        [Authorize(AuthenticationSchemes = "Windows")]
        [HttpGet("WinAuth")]
        public IActionResult CreateTokenWinAuth()
        {
            IActionResult response = Unauthorized();

            var user = new UserModel()
            {
                Name = User.Identity.Name,
                Email = User.Identity.Name + "@domain.com"
            };

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(tokenString);
            }

            return response;
        }

        private string BuildToken(UserModel user)
        {

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
               };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            if (login.Username == "user" && login.Password == "secret")
            {
                user = new UserModel { Name = "User Bearer", Email = "user@domain.com"};
            }

            return user;

        }

    }
}
