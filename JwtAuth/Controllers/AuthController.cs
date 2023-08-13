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
        private readonly IConfiguration configuration;
        private readonly byte[] key; // Store the key as a private field

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]); // Load the key from configuration
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Auth([FromBody] Users user)
        {
            IActionResult response = Unauthorized();
            if (user != null)
            {
                if (user.Id.Equals(123) && user.Name.Equals("a"))
                {
                    var issuer = configuration["Jwt:Issuer"];
                    var audience = configuration["Jwt:Audience"];

                    var signingCredentials = new SigningCredentials(
                       new SymmetricSecurityKey(key),
                       SecurityAlgorithms.HmacSha512
                    );

                    var subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Iat, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Name),
                    });

                    var expires = DateTime.UtcNow.AddDays(1);
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
                    return Ok(jwtToken);
                }
            }
            return response;
        }
    }
}
