using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MoviesAPI.Entities;
using MoviesAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {

            app.MapGet("/get-users", async (MovieContext movieContext) => await movieContext.User.ToListAsync());


            app.MapPost("/signup", async (UserDto userDto, MovieContext dbContetx) =>
            {
                if(await dbContetx.User.AnyAsync(x => x.Username == userDto.Username))
                {
                    return Results.BadRequest("User already exists");
                }

                var hasher = new PasswordHasher<User>();
                var user = new User
                {
                    Username = userDto.Username,
                    PasswordHash = hasher.HashPassword(null!, userDto.Password)
                };
                dbContetx.User.Add(user);
                await dbContetx.SaveChangesAsync();
                return Results.Ok($"User Created");

            });


            app.MapPost("/login", async (UserDto userDto, MovieContext dbContext, IConfiguration config) =>
            {

                var User = await dbContext.User.FirstOrDefaultAsync(x => x.Username == userDto.Username);
                if (User is null)
                {
                    return Results.Unauthorized();
                }

                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(User, User.PasswordHash, userDto.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    return Results.Unauthorized();
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, User.Username),
                    new Claim(ClaimTypes.NameIdentifier, User.Id.ToString())
                };

                var jwtSettings = config.GetSection("Jwt");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(new
                {
                    token = tokenString,
                });


            });


        }
    }
}

public record UserDto(string Username, string Password);