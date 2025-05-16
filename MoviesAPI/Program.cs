using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesAPI.Data;
using MoviesAPI.Endpoints;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

//Use connection string from appsettings.json
builder.Services.AddDbContext<MovieContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("MoviesDbCS")));

var jwtSettings = builder.Configuration.GetSection("Jwt");


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };

});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowLocal3000");
app.UseAuthentication();
app.UseAuthorization();


//Async method to seed data in the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MovieContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}
app.MapAuthEndpoints();
app.MapGet("/", () => "Hello World!");
app.MapMoviesEndpoints();
app.Run();
