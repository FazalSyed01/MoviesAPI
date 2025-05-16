using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Entities;

namespace MoviesAPI.Endpoints
{
    public static class MoviesEndpoints
    {
        //Extension Method
        public static RouteGroupBuilder MapMoviesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("movies").WithParameterValidation().RequireAuthorization();

            //GET /movies
            group.MapGet("/", async (MovieContext movieContext) => await movieContext.Movies.Include("Genre").ToListAsync());

            //GET /movies/{id}
            group.MapGet("/{id}", async (MovieContext movieContext, int id) =>
            {

                Movie? movie = await movieContext.Movies.Include("Genre").FirstOrDefaultAsync(x => x.Id == id);
                return movie != null ? Results.Ok(movie) : Results.NotFound();
            });

            //POST /movies
            group.MapPost("/", async (Movie newMovie, MovieContext movieContext) =>
            {
                newMovie.Genre = await movieContext.Genres.FirstOrDefaultAsync(x => x.Id == newMovie.GenreId);
                movieContext.Movies.Add(newMovie);
                await movieContext.SaveChangesAsync();
                return Results.Created($"/movies/{newMovie.Id}", newMovie);
            });

            //PUT /movies/{id}
            group.MapPut("/{id}", async (int id, Movie updatedMovie, MovieContext movieContext) =>
            {
                Movie? movie = await movieContext.Movies.FindAsync(id);

                if (movie is null)
                {
                    return Results.NotFound();
                }

                //movieContext.Entry(movie).CuurentValues.SetValues(updatedMovie);
                if (updatedMovie.Name is not null) { movie.Name = updatedMovie.Name; }
                if (updatedMovie.Price != 0) { movie.Price = updatedMovie.Price; }
                if (updatedMovie.ReleaseDate != default) { movie.ReleaseDate = updatedMovie.ReleaseDate; }
                if (updatedMovie.GenreId != 0) { movie.GenreId = updatedMovie.GenreId; }

                movieContext.Movies.Update(movie);
                await movieContext.SaveChangesAsync();
                return Results.NoContent();
            });

            //DELETE /movies/{id}
            group.MapDelete("/{id}", async (int id, MovieContext movieContext) =>
            {
                Movie? movie = await movieContext.Movies.FindAsync(id);
                if (movie is null)
                {
                    return Results.NotFound();
                }
                movieContext.Movies.Remove(movie);
                await movieContext.SaveChangesAsync();
                return Results.NoContent();
            });

            return group;
        }
    }
}
