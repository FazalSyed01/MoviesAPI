using KaamKaaj.Application.Interfaces;
using KaamKaaj.Domain.DTOs;
using KaamKaaj.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesAPI.Data;
using MoviesAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesAPI.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthInterface _authService;

        public AuthController(IAuthInterface authService)
        {
            _authService = authService;
        }


        [HttpGet("get-users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _authService.GetUsersAsync();
            return Ok(users);
        }


        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] UserDto userDto)
        {
            try
            {
                var result = await _authService.SignupAsync(userDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            try
            {
                var token = await _authService.LoginAsync(userDto);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
    
}

