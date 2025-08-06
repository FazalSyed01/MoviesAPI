using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Entities;

namespace MoviesAPI.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public JobsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jobs>>> GetJobs() =>
        await _context.Jobs.ToListAsync();


        //POST /jobs
        [HttpPost]
        public async Task<ActionResult<Jobs>> PostJob(Jobs newMovie)
        {
            _context.Jobs.Add(newMovie);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJobs), new { id = newMovie.Id }, newMovie);
        }
    }
}
