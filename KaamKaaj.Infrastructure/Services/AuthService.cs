using KaamKaaj.Application.Interfaces;
using KaamKaaj.Domain.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoviesAPI.Data;
using MoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KaamKaaj.Infrastructure.Services
{
    public class AuthService : IAuthInterface
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<string> SignupAsync(UserDto userDto)
        {
            if (await _context.User.AnyAsync(u => u.Username == userDto.Username))
            {
                throw new Exception("User already exists");
            }

            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                Username = userDto.Username,
                Role = string.IsNullOrWhiteSpace(userDto.Role) ? "User" : userDto.Role,
                PasswordHash = hasher.HashPassword(null!, userDto.Password)
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return "User Created";
        }

        public async Task<string> LoginAsync(UserDto userDto)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Username == userDto.Username);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, userDto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials");

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
