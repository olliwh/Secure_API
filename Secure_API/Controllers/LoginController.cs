﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Secure_API.Models;
using Secure_API.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Secure_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]

    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginRepository _loginRepository;
        public LoginController(IConfiguration config, ILoginRepository loginRepository)
        {
            _configuration = config;
            _loginRepository = loginRepository;
        }
        [EnableRateLimiting("fixed")]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Login([FromBody] UserCredentials loginData)
        {
            User? user = _loginRepository.Login(loginData);
            if (user == null) return Unauthorized("Invalid credentials");
            return Ok(CreateToken(user));

        }
        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Post([FromBody] User userInfo)
        {
            try
            {
                User created = _loginRepository.CreateUser(userInfo);
                return Created($"api/Login/{created.UserId}", created);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string CreateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SekretKey"]));
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//jwt ID
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),//isssued at
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }; 
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(4),
                signingCredentials: signature);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
