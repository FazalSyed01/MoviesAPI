using Microsoft.EntityFrameworkCore;
using MoviesAPI.Entities;


namespace MoviesAPI.Data
{
    public class MovieContext(DbContextOptions<MovieContext> options) : DbContext(options)
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<User> User { get; set; }


        //Method to seed data into db
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Drama" },
                new Genre { Id = 3, Name = "Sci-Fi" },
                new Genre { Id = 4, Name = "Horror" },
                new Genre { Id = 5, Name = "Fantasy" }
            );

            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 1,
                    Name = "Inception",
                    Price = 14.99m,
                    GenreId = 3,
                    ReleaseDate = new DateOnly(2010, 7, 16)
                },
                new Movie
                {
                    Id = 2,
                    Name = "The Dark Knight",
                    Price = 12.99m,
                    GenreId = 1,
                    ReleaseDate = new DateOnly(2008, 7, 18)
                },
                new Movie
                {
                    Id = 3,
                    Name = "The Shawshank Redemption",
                    Price = 9.99m,
                    GenreId = 2,
                    ReleaseDate = new DateOnly(1994, 9, 23)
                },
                new Movie
                {
                    Id = 4,
                    Name = "The Godfather",
                    Price = 11.99m,
                    GenreId = 2,
                    ReleaseDate = new DateOnly(1972, 3, 24)
                },
                new Movie
                {
                    Id = 5,
                    Name = "The Matrix",
                    Price = 13.99m,
                    GenreId = 3,
                    ReleaseDate = new DateOnly(1999, 3, 31)
                }
            );
        }
    }
}
