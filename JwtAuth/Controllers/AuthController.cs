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
        [HttpPost("auth")] 
        public IActionResult Auth([FromBody] Users user)
        {
            IActionResult response = Unauthorized();

            if (user != null && user.Id.Equals(123) && user.Password.Equals("a"))
            {
                var (accessToken, refreshToken) = GenerateTokens(user.Id); // Generate both tokens
                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }

            return response;
        }


        [Authorize]
        [HttpPost("renew")] 
        public IActionResult RenewToken([FromBody] RefreshRequestModel model)
        {
            var refreshToken = model.RefreshToken; 
            var userId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Iat));
            var (newAccessToken, newRefreshToken) = GenerateTokens(userId); 

            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }

        private (string AccessToken, string RefreshToken) GenerateTokens(int userId)
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

            var accessTokenExpiration = TimeSpan.FromMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpirationMinutes"]));
            var refreshTokenExpiration = TimeSpan.FromDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpirationDays"]));

            var expires = DateTime.UtcNow.Add(accessTokenExpiration);

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

            var refreshExpires = DateTime.UtcNow.Add(refreshTokenExpiration);

            var refreshTokenDescription = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = refreshExpires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };

            var refreshToken = tokenHandler.CreateToken(refreshTokenDescription);
            var jwtRefreshToken = tokenHandler.WriteToken(refreshToken);

            return (jwtToken, jwtRefreshToken);
        }
    }
}

