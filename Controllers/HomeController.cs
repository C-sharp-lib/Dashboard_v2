using System.Diagnostics;
using Dash.Data;
using Microsoft.AspNetCore.Mvc;
using Dash.Models;
using Dash.Services;
using Microsoft.EntityFrameworkCore;

namespace Dash.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepository;
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IUserRepository userRepository)
    {
        _logger = logger;
        _context = context;
        _userRepository = userRepository;
    }
    [HttpGet]
    public async Task<IActionResult>Index()
    {
        var users = await _context.Users.Take(4).ToListAsync();
        ViewBag.users = users;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}