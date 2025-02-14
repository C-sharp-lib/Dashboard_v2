using AspNetCoreHero.ToastNotification.Abstractions;
using Dash.Areas.Identity.Models;
using Dash.Controllers;
using Dash.Data;
using Dash.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Identity.Controllers;

[Area("Identity")]
[Route("{area?}/[controller]/[action]")]
public class IdentityController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webenv;
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly INotyfService _notyfService;
    public IdentityController(UserManager<AppUser> userM, SignInManager<AppUser> signInM, 
        IHttpContextAccessor contextAccessor, ApplicationDbContext context, IWebHostEnvironment webenv,
        IUserRepository userRepository, INotyfService notyfService)
    {
        _userManager = userM;
        _signInManager = signInM;
        _httpContextAccessor = contextAccessor;
        _context = context;
        _webenv = webenv;
        _userRepository = userRepository;
        _notyfService = notyfService;
    }
    private AppUser? ActiveUser
    {
        get
        {
            return _context.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefault(u => u.Id == HttpContext.Session.GetString("Id"));
        }
    }
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in. You need to be logged in to view this page.");
            return RedirectToAction("Login", "Identity", new {area = "Identity"});
        }

        var users = await _userRepository.GetAllUsersAsync();
        ViewBag.user = ActiveUser;
        return View(users);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.UserRoles
                .Include(x => x.User)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.User.Email == model.Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.User, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    HttpContext.Session.SetString("Id", user.User.Id);
                    HttpContext.Session.SetString("UserName", user.User.UserName!);
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });    
                }
                _notyfService.Error("Invalid login attempt.");
            }
            else
            {
                _notyfService.Error("User does not exist.");
            }
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var users = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                DateOfHire = model.DateOfHire,
                Address = model.Address,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode
            };
            var result = await _userManager.CreateAsync(users, model.Password);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(users, false);
                HttpContext.Session.SetString("Id", users.Id);
                HttpContext.Session.SetString("UserName", users.UserName);
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        await _signInManager.SignOutAsync();
        _notyfService.Information("User logged out.");
        return RedirectToAction("Login", "Identity", new { area = "Identity" });
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserPage(string id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        var user = await _context.Users
            .Include(x => x.UserJobs)
            .ThenInclude(x => x.Job)
            .Include(x => x.UserEvents)
            .ThenInclude(x => x.Event)
            .Include(x => x.CampaignUserNotes)
            .ThenInclude(x => x.Campaign)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            _notyfService.Error("User does not exist.");
            return RedirectToAction("Index", "Identity", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        return View(user);
    }
    
    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(string id, [FromForm] UpdateAppUserViewModel user)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        try
        {
            await _userRepository.UpdateUserAsync(id, user);
            
            ViewBag.user = ActiveUser;
            _notyfService.Success("User updated.");
            return RedirectToAction("UserDetails", "Identity", new { area = "Identity", id });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error($"There has been a exception while updating the user | DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error($"There has been a exception while updating the user | DbUpdateException: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the user | Exception: {ex.Message}");
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> UserDetails(string id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }

        AppUser user = await _userRepository.GetUserByIdAsync(id);
        ViewBag.user = ActiveUser;
        return View(user);
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        await _userRepository.DeleteUserAsync(id);
        _notyfService.Success("User deleted.");
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }
    
    [HttpGet]
    public IActionResult AccessDenied()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        ViewBag.user = ActiveUser;
        return View();
    }
}