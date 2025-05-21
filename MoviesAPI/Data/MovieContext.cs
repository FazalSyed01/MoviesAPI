using Microsoft.EntityFrameworkCore;
using MoviesAPI.Entities;


namespace MoviesAPI.Data
{
    public class MovieContext(DbContextOptions<MovieContext> options) : DbContext(options)
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<User> User { get; set; }


    }
}
