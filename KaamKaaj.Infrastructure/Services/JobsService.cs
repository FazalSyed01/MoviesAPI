using MoviesAPI.Data;
using MoviesAPI.Entities;
using KaamKaaj.Application.Interfaces;

namespace MoviesAPI.Services
{
    public class JobsService : IJobsService
    {
        private readonly AppDbContext _context;
        public JobsService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddJobsAsync(Jobs job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
        }
    }
    
}
