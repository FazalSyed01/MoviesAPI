using MoviesAPI.Entities;

namespace KaamKaaj.Application.Interfaces
{
    public interface IJobsService
    {
        Task AddJobsAsync(Jobs job);
    }
}
