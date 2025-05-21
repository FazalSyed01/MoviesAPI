using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Entities;

namespace MoviesAPI.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly MovieContext _context;
        public GenreController(MovieContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetMovies() =>
        await _context.Genres.ToListAsync();
    }
}
