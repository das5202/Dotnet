using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuth.Models;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly byte[] _key;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Auth([FromBody] Users user)
        {
            IActionResult response = Unauthorized();

            if (user != null && user.Id.Equals(123) && user.Password.Equals("a"))
            {
                var jwtToken = GenerateToken(user.Id); // Generate a new token
                return Ok(jwtToken);
            }

            return response;
        }

        [Authorize]
        [HttpGet("renew")]
        public IActionResult RenewToken()
        {
            var userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Iat));

            var jwtToken = GenerateToken(userId); // Re-generate a new token
            return Ok(jwtToken);
        }


        private string GenerateToken(int userId)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var signingCredentials = new SigningCredentials(
               new SymmetricSecurityKey(_key),
               SecurityAlgorithms.HmacSha512
            );

            var subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Iat, userId.ToString())
            });

            var expires = DateTime.UtcNow.AddDays(100);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
