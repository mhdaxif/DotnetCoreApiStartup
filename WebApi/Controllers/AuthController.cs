using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private IUserService _userService;

        public AuthController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        [HttpGet("token/{id}")]
        public async Task<ActionResult> GetTokenForUerAsync(int id)
        {
            var user = _userService.GetSingle(id);
            if (user == null) return NotFound("User does not exists");

            var tokenString = await BuildToken(user);
            var loginResult = new
            {
                token = tokenString,
                expiresIn = DateTime.Now.AddDays(180),
                requestAt = DateTime.Now,
                token_type = "bearer",
                profile = new
                {
                    user.Name
                }
            };

            return Ok(loginResult);
        }

        [NonAction]
        private async Task<string> BuildToken(User user)
        {
            var fullName = $"{user.Name}";
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    //new Claim(JwtRegisteredClaimNames.Email, user?.Email),
                    //new Claim(JwtRegisteredClaimNames.Jti, user?.UserName),
                    new Claim("Name", fullName),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              //claims.DistinctBy(c => c.Value),
              expires: DateTime.Now.AddDays(180),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}