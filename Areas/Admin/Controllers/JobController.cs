using AspNetCoreHero.ToastNotification.Abstractions;
using Dash.Areas.Admin.Models;
using Dash.Areas.Identity.Models;
using Dash.Data;
using Dash.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]/[action]")]
[Authorize(Roles = "Admin,User")]
public class JobController : Controller
{
    private readonly  ApplicationDbContext _context;
    private readonly IJobRepository _jobRepository;
    private readonly INotyfService _notyfService;
    private const int PageSize = 10;
    public JobController(ApplicationDbContext context, IJobRepository jobRepository, INotyfService notyfService)
    {
        _context = context;
        _jobRepository = jobRepository;
        _notyfService = notyfService;
    }
    private AppUser? ActiveUser
    {
        get
        {
            return _context.AppUsers
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefault(u => u.Id == HttpContext.Session.GetString("Id"));
        }
    }
    [HttpGet]
    public async Task<IActionResult> Index(string searchString,  int page = 1)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        var userJobs = await _context.UserJobs.Include(u => u.User).Include(u => u.Job).OrderBy(u => u.User.UserName)
            .Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();
        var totalJobs = await _context.Jobs.CountAsync();
        var jobs = _context.UserJobs.Include(u => u.User).Include(j => j.Job).AsQueryable();
        var totalPages = (int)System.Math.Ceiling((double)totalJobs / PageSize);
        if (!string.IsNullOrEmpty(searchString))
        {
            jobs = jobs.Where(j => j.User.UserName!.Contains(searchString) || j.Job.StartDate.ToShortDateString().Contains(searchString) || j.Job.Title.Contains(searchString) || j.Job.Status.Contains(searchString));
        }
        
        var index = new JobsPaginateViewModel
        {
            Jobs = jobs,
            UserJobs = userJobs,
            CurrentPage = page,
            TotalPages = totalPages
        };
        ViewBag.user = ActiveUser;
        return View(index);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> JobDetails(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        var job = await _jobRepository.GetUserJobByIdAsync(id);
        ViewBag.user = ActiveUser;
        return View(job);
    }

    [HttpGet]
    public async Task<IActionResult> AddJobPage()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        ViewBag.users = await _context.Users.ToListAsync();
        ViewBag.user = ActiveUser;
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddJob([FromForm] UserJobs jobs)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        try
        {
            await _jobRepository.AddJobAsync(jobs);
            _notyfService.Success("Job added.");
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Job", new {area = "Admin"});
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction("Index", "Job", new { area = "Admin" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateJobPage(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        var userJob = await _jobRepository.GetUserJobByIdAsync(id);
        if (userJob == null)
        {
            _notyfService.Error("Job not found.");
            return RedirectToAction("Index", "Job", new {area = "Admin"});
        }
        ViewBag.user = ActiveUser;
        ViewBag.users = await _context.Users.ToListAsync();
        return View(userJob);
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateJob(int id, [FromForm] UserJobs jobs)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        
        try
        {
            await _jobRepository.UpdateJobAsync(id, jobs);
            _notyfService.Success("Job updated.");
            ViewBag.users = await _context.Users.ToListAsync();
            ViewBag.user = ActiveUser;
            return RedirectToAction("Index", "Job", new {area = "Admin"});
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error(ex.Message);
            throw new DbUpdateConcurrencyException(ex.Message);
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error(ex.Message);
            throw new DbUpdateException(ex.Message);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            throw new Exception(ex.Message);
        }
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, you need to be to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }
        await _jobRepository.DeleteJobAsync(id);
        _notyfService.Success("Job deleted.");
        ViewBag.user = ActiveUser;
        ViewBag.users = await _context.Users.ToListAsync();
        return RedirectToAction("Index", "Job", new { area = "Admin" });
    }
}