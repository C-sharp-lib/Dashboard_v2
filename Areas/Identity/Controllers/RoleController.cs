using AspNetCoreHero.ToastNotification.Abstractions;
using Dash.Areas.Identity.Models;
using Dash.Data;
using Dash.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Identity.Controllers;

[Area("Identity")]
[Route("[area]/[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly INotyfService _notyfService;

    public RoleController(ApplicationDbContext context, RoleManager<AppRole> roleManager,
        UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        IUserRepository userRepository, INotyfService notyfService)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _userRepository = userRepository;
        _signInManager = signInManager;
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
        var users = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .ToListAsync();
        var roles = await _roleManager.Roles.Include(r => r.UserRoles).ThenInclude(r => r.User).ToListAsync();
        ViewBag.users = users;
        ViewBag.user = ActiveUser;
        return View(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> AddRoleToUserPage(string id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var user = await _userManager.Users
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
        ViewBag.roles = await _roleManager.Roles.ToListAsync();
        ViewBag.theUser = user;
        ViewBag.user = ActiveUser;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignRole([FromForm] UserRolesViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        try
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                _notyfService.Error("Role doesn't exist.");
                return RedirectToAction("Index", "Role", new { area = "Identity" });
            }
            if (user == null)
            {
                _notyfService.Error("User doesn't exist.");
                return RedirectToAction("Index", "Role", new { area = "Identity" });
            }

            var userRoles = new AppUserRoles
            {
                RoleId = role.Id,
                UserId = user.Id,
            };
            ViewBag.user = ActiveUser;
            _context.UserRoles.Add(userRoles);
            await _context.SaveChangesAsync();
            _notyfService.Success("Role assigned.");
            return RedirectToAction("Index", "Role", new { area = "Identity" });
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction("Index", "Role", new { area = "Identity" });
        }
    }
    
    [HttpGet]
    public IActionResult AddRole()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddARole([FromForm] AddRoleViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        AppRole role = new AppRole
        {
            Name = model.Name,
            NormalizedName = model.Name.ToUpper(),
        };
        IdentityResult result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            _notyfService.Error(result.Errors.FirstOrDefault()!.Description);
            return RedirectToAction("Index", "Role", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Role", new { area = "Identity" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateRole(string id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var updatableRole = await _roleManager.FindByIdAsync(id);
        if (updatableRole == null)
        {
            _notyfService.Error("Role could not be found.");
            return RedirectToAction("Index", "Role", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        return View(updatableRole);
    }
   
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveRoleFromUser(string userId, string roleName)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _notyfService.Error("User not found");
            return RedirectToAction("Index", "Identity", new { area = "Identity" });
        }

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        ViewBag.user = ActiveUser;
        if (result.Succeeded)
        {
            _notyfService.Success("Role removed successfully");
            return RedirectToAction("Index", "Identity", new { area = "Identity" });
        }
        else
        {
            _notyfService.Error("Error deleting role");
            return RedirectToAction("Index", "Identity", new { area = "Identity" });
        }
    }
    

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRole(string id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        var role = await _context.AppRoles.FindAsync(id);
        if (role == null)
        {
            _notyfService.Error("Role not found");
            return RedirectToAction("Index", "Role", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        _context.AppRoles.Remove(role);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Role", new { area = "Identity" });
    }
}