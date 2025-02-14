using Dash.Areas.Admin.Models;
using Dash.Areas.Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Services;

public interface IJobRepository : IRepository<Jobs>
{
    Task<IEnumerable<Jobs>> GetAllJobsAsync();
    Task<IEnumerable<UserJobs>> GetAllUserJobsAsync();
    Task<Jobs> GetJobByIdAsync(int id);
    Task<UserJobs> GetUserJobByIdAsync(int id);
    Task AddJobAsync([FromForm] UserJobs jobs);
    Task UpdateJobAsync(int id, [FromForm] UserJobs jobs);
    Task DeleteJobAsync(int id);
    Task<int> CountJobsAsync();
}