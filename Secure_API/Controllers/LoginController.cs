using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Secure_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Secure_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration config)
        {
            _configuration = config;   
        }
        [HttpPost]
        public IActionResult Login([FromBody] UserCredentials data)
        {
            if (data.Username == "string" && data.Password == "string1")
            {
                var token = CreateToken(data.Username);
                return Ok(token);
            }
            return BadRequest("Login failed");
        }
        private string CreateToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SekretKey"]));
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//jwt ID
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),//isssued at
                        new Claim("UserId", username) }; //info om bruger

            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),//tjek om det blover valideret?,
                signingCredentials: signature);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("token validated");
        }
    }
}
