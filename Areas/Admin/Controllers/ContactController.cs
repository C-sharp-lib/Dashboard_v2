using AspNetCoreHero.ToastNotification.Abstractions;
using Dash.Areas.Identity.Models;
using Dash.Data;
using Dash.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Admin.Controllers;

public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IContactRepository _contactRepository;
    private readonly INotyfService _notyfService;

    public ContactController(ApplicationDbContext context, IContactRepository contactRepository,
        INotyfService notyfService)
    {
        _context = context;
        _contactRepository = contactRepository;
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
    public async Task<IActionResult> Index()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  Please login again.");
            return RedirectToAction("Login", "Identity", new {area="Identity"});
        }

        var contacts = await _contactRepository.GetAllContactsAsync();
        return View(contacts);
    }
}