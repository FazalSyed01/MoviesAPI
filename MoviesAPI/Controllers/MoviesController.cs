using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Entities;

namespace MoviesAPI.Endpoints
{

    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MoviesController(AppDbContext context) => _context = context;

        //GET /movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies() =>
        await _context.Movies.Include(m => m.Genre).ToListAsync();

        //GET /movies/{id}
        [Authorize]
        [HttpGet("{id}")]
 
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);
            return movie is not null ? Ok(movie) : NotFound();
        }

        //POST /movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie newMovie)
        {
            newMovie.Genre = await _context.Genres.FindAsync(newMovie.GenreId);
            _context.Movies.Add(newMovie);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovie), new { id = newMovie.Id }, newMovie);
        }
        //PUT /movies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie updatedMovie)
        {
            if (id != updatedMovie.Id)
            {
                return BadRequest();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie is null)
            {
                return NotFound();
            }
            if (movie.Name is not null)
            {
                movie.Name = updatedMovie.Name;
            }
            if (movie.Price != 0)
            {
                movie.Price = updatedMovie.Price;
            }
            if(movie.GenreId != 0)
            {
                movie.GenreId = updatedMovie.GenreId;
            }
            if(movie.ReleaseDate != default)
            {
                movie.ReleaseDate = updatedMovie.ReleaseDate;
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //DELETE /movies/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
       
            var movie = await _context.Movies.FindAsync(id);
            if (movie is null) return NotFound();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
