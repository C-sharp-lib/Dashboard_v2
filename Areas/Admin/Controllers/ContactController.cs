using System.Text.Json;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Helpers;
using Dash.Areas.Admin.Models;
using Dash.Areas.Identity.Models;
using Dash.Data;
using Dash.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dash.Areas.Admin.Controllers;
[Area("Admin")]
[Route("[area]/[controller]/[action]")]
[Authorize(Roles = "Admin,User")]
public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IContactRepository _contactRepository;
    private readonly INotyfService _notyfService;
    private readonly ApiSettings _apiSettings;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly GeoService _geoService;

    public ContactController(ApplicationDbContext context, IContactRepository contactRepository,
        INotyfService notyfService, IOptions<ApiSettings> apiSettings, IHttpClientFactory httpClientFactory, GeoService geoService)
    {
        _context = context;
        _contactRepository = contactRepository;
        _notyfService = notyfService;
        _apiSettings = apiSettings.Value;
        _httpClientFactory = httpClientFactory;
        _geoService = geoService;
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
            _notyfService.Error("You are not logged in.  Please login again.");
            return RedirectToAction("Login", "Identity", new {area="Identity"});
        }
        ViewBag.user = ActiveUser;
        var contacts = await _contactRepository.GetAllContactsAsync();
        return View(contacts);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> ContactDetails(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  Please login again.");
            return RedirectToAction("Login", "Identity", new {area="Identity"});
        }

        var contact = await _contactRepository.GetContactByIdAsync(id);
        var contactAddress = await _context.Contacts
            .Where(c => c.ContactId == id)
            .Select(c => new 
            {
                c.Address,
                c.City,
                c.State,
                c.ZipCode
            })
            .FirstOrDefaultAsync();

        if (contactAddress == null)
        {
            _notyfService.Error("There was no contact with that ID.");
            return RedirectToAction("Index", "Contact", new {area = "Admin"});
        }

        var cAddress =
            $"{contactAddress.Address}, {contactAddress.City}, {contactAddress.State} {contactAddress.ZipCode}";
        var location = 
        ViewBag.address = cAddress;
        ViewBag.user = ActiveUser;
        ViewBag.location = location;
        ViewBag.apiKey = _apiSettings.ApiKey;
        
        return View(contact);
    }
    
    private async Task<(double Lat, double Lon)?> GetGeoLocation(string address)
    {
        var apiKey = _apiSettings.ApiKey;
        var url = $"https://api.geoapify.com/v1/geocode/search?text={Uri.EscapeDataString(address)}&apiKey={apiKey}";

        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        var feature = doc.RootElement.GetProperty("features")[0].GetProperty("geometry").GetProperty("coordinates");

        return (feature[1].GetDouble(), feature[0].GetDouble());
    }

    private string GenerateMapUrl(double lat, double lon)
    {
        var apiKey = _apiSettings.ApiKey;
        var markerUrl = "~/img/"; // Custom marker image URL

        return $"https://maps.geoapify.com/v1/staticmap?style=osm-bright&width=600&height=400&center=lonlat:{lon},{lat}&zoom=14&marker=lonlat:{lon},{lat};type:material;color:red;icon:{Uri.EscapeDataString(markerUrl)}&apiKey={apiKey}";
    }

    [HttpGet]
    public async Task<IActionResult> AddContactPage()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  Please login again.");
            return RedirectToAction("Login", "Identity", new {area="Identity"});
        }
        ViewBag.user = ActiveUser;
        return View();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> UpdateContactPage(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  Please login again.");
            return RedirectToAction("Login", "Identity", new {area="Identity"});
        }
        var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.ContactId == id);
        if (contact == null)
        {
            _notyfService.Error("There was no contact with that ID.");
            return RedirectToAction("Index", "Contact", new {area = "Admin"});
        }
        ViewBag.user = ActiveUser;
        return View(contact);
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateContact(int id, [FromForm] UpdateContactViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  Please login again.");
            return RedirectToAction("Login", "Identity", new {area="Identity"});
        }
        ViewBag.user = ActiveUser;
        try
        {
            await _contactRepository.UpdateContactAsync(id, model);
            await _context.SaveChangesAsync();
            _notyfService.Success("Contact updated");
            return RedirectToAction("Index", "Contact", new { area = "Admin"});
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _notyfService.Error($"There has been a exception while updating the contact | DbUpdateConcurrencyException: {ex.Message}");
            return RedirectToAction("Index", "Contact", new { area = "Admin"});
        }
        catch (DbUpdateException ex)
        {
            _notyfService.Error($"There has been a exception while updating the contact | DbUpdateException: {ex.Message}");
            return RedirectToAction("Index", "Contact", new { area = "Admin"});
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the contact | Exception: {ex.Message}");
            return RedirectToAction("Index", "Contact", new { area = "Admin"});
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddContact([FromForm] AddContactViewModel model)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in.  Please login again.");
            return RedirectToAction("Login", "Identity", new {area="Identity"});
        }
        ViewBag.user = ActiveUser;
        try
        {
            await _contactRepository.AddContactAsync(model);
            await _context.SaveChangesAsync();
            _notyfService.Success("Contact added");
            return RedirectToAction("Index", "Contact", new { area = "Admin"});
        }
        catch (Exception ex)
        {
            _notyfService.Error($"There has been a exception while updating the contact | Exception: {ex.Message}");
            return RedirectToAction("Index", "Contact", new { area = "Admin"});
        }
    }
    
    
    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        await _contactRepository.DeleteContactAsync(id);
        _notyfService.Success("Contact deleted.");
        ViewBag.user = ActiveUser;
        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }
}