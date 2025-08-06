using Microsoft.EntityFrameworkCore;
using MoviesAPI.Entities;


namespace MoviesAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Jobs> Jobs { get; set; }

    }
}
