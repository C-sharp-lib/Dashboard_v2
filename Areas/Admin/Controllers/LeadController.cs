using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
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
[Authorize(Roles = "Admin,Manager")]
public class LeadController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILeadsRepository _leadsRepository;
    private readonly INotyfService _notyfService;
    public LeadController(ApplicationDbContext context, ILeadsRepository leadsRepository, INotyfService notyfService)
    {
        _context = context;
        _leadsRepository = leadsRepository;
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
    public async Task<IActionResult> Index()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        var leads = await _leadsRepository.GetAllLeadsAsync();
        ViewBag.user = ActiveUser;
        return View(leads);
    }

    [HttpGet]
    public async Task<IActionResult> AddLeadPage()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        ViewBag.users = await _context.Users.ToListAsync();
        ViewBag.user = ActiveUser;
        return View();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateLeadPage(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var lead = await _leadsRepository.GetLeadByIdAsync(id);
        ViewBag.users = await _context.Users.ToListAsync();
        ViewBag.user = ActiveUser;
        ViewBag.lead = lead;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddLead([FromForm] AddLeadViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        try
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Model state is invalid.");
                return RedirectToAction("Index", "Lead", new { area = "Admin" });
            }
            ViewBag.users = await _context.Users.ToListAsync();
            ViewBag.user = ActiveUser;
            await _leadsRepository.AddLeadAsync(model);
            _notyfService.Success("Lead successfully added.");
            return RedirectToAction("Index", "Lead", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            _notyfService.Error($"Unable to add lead | Exception: {ex.Message}");
            return RedirectToAction("Index", "Lead", new { area = "Admin" });
        }
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateLead(int id, [FromForm] UpdateLeadsViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        
        try
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Model state is invalid.");
                return RedirectToAction("Index", "Lead", new { area = "Admin" });
            }
            ViewBag.users = await _context.Users.ToListAsync();
            ViewBag.user = ActiveUser;
            await _leadsRepository.UpdateLeadAsync(id, model);
            _notyfService.Success("Lead successfully updated.");
            return RedirectToAction("Index", "Lead", new { area = "Admin" });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error($"Unable to update lead | DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("Index", "Lead", new { area = "Admin" });
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error($"Unable to update lead | DbUpdateException: {ex.Message}");
            return RedirectToAction("Index", "Lead", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            _notyfService.Error($"Unable to update lead | Exception: {ex.Message}");
            return RedirectToAction("Index", "Lead", new { area = "Admin" });
        }
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteLead(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        await _leadsRepository.DeleteLeadAsync(id);
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Lead", new { area = "Admin" });
    }
}