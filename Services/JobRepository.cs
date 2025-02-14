using System.Text.RegularExpressions;
using AspNetCoreHero.ToastNotification.Abstractions;
using Dash.Areas.Admin.Models;
using Dash.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Services;

public class JobRepository : Repository<Jobs>, IJobRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Jobs> _dbSet;
    private readonly INotyfService _notyfService;
    public JobRepository(ApplicationDbContext context, INotyfService notyfService) : base(context)
    {
        _context = context;
        _notyfService = notyfService;
        _dbSet = _context.Set<Jobs>();
    }

    public async Task<IEnumerable<Jobs>> GetAllJobsAsync()
    {
        return await _dbSet
            .Include(c => c.UserJobs)
            .ThenInclude(x => x.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserJobs>> GetAllUserJobsAsync()
    {
        var userJobs = await _context.UserJobs.Include(x => x.User).Include(x => x.Job).ToListAsync();
        return userJobs;
    }

    public async  Task<Jobs> GetJobByIdAsync(int id)
    {
        var job = await _dbSet
            .Include(x => x.UserJobs)
            .ThenInclude(x => x.User)
            .Include(x => x.UserJobs)
            .ThenInclude(x => x.Job)
            .FirstOrDefaultAsync(x => x.JobId == id);
        if (job == null)
        {
            _notyfService.Error("Job not found");
            throw new NullReferenceException("Job not found");
        }
        return job;
    }

    public async Task<UserJobs> GetUserJobByIdAsync(int id)
    {
        var userJob = await _context.UserJobs.Include(x => x.User).Include(x => x.Job).FirstOrDefaultAsync(x => x.UserJobId == id);
        if (userJob == null)
        {
            _notyfService.Error("User's Job not found");
            throw new NullReferenceException("User's Job not found");
        }
        return userJob;
    }

    public async Task AddJobAsync([FromForm] UserJobs jobs)
    {
        var userjobsmodel = new UserJobs
        {
            Job = new Jobs
            {
                Title = jobs.Job.Title,
                Description = jobs.Job.Description,
                StartDate = jobs.Job.StartDate,
                EndDate = jobs.Job.EndDate,
                Status = jobs.Job.Status,
                Priority = jobs.Job.Priority,
                Type = jobs.Job.Type,
                Category = jobs.Job.Category,
                EstimatedCost = jobs.Job.EstimatedCost
            },
            UserId = jobs.UserId,
            JobId = jobs.JobId,
        };
        _context.UserJobs.Add(userjobsmodel);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateJobAsync(int id, [FromForm] UserJobs jobs)
    {
        var userJob = await GetUserJobByIdAsync(id);
        userJob.Job.Title = jobs.Job.Title;
        userJob.Job.Description = jobs.Job.Description;
        userJob.Job.StartDate = jobs.Job.StartDate;
        userJob.Job.EndDate = jobs.Job.EndDate;
        userJob.Job.Status = jobs.Job.Status;
        userJob.Job.Priority = jobs.Job.Priority;
        userJob.Job.Type = jobs.Job.Type;
        userJob.Job.Category = jobs.Job.Category;
        userJob.Job.EstimatedCost = jobs.Job.EstimatedCost;
        _context.UserJobs.Update(userJob);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteJobAsync(int id)
    {
        var job = await _dbSet.FirstOrDefaultAsync(x => x.JobId == id);
        if (job == null)
        {
            throw new NullReferenceException("Job not found");
        }
        _dbSet.Remove(job);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountJobsAsync()
    {
        return await _dbSet.CountAsync();
    }
}