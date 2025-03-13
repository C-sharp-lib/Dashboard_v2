using AspNetCoreHero.ToastNotification.Abstractions;
using Dash.Areas.Admin.Models;
using Dash.Areas.Identity.Models;
using Dash.Data;
using Dash.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Exception = System.Exception;


namespace Dash.Areas.Admin.Controllers;
[Area("Admin")]
[Route("[area]/[controller]/[action]")]
[Authorize(Roles = "Admin,User,Customer")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IJobRepository _jobRepository;
    private readonly ILeadsRepository _leadsRepository;
    private readonly ICampaignRepository _campaignRepository;
    private readonly ICampaignUserNoteRepository _campaignUserNoteRepository;
    private readonly ICampaignUserTaskRepository _campaignUserTaskRepository;
    private readonly OnlineUserService _onlineUserService;
    private readonly INotyfService _notyfService;
    private readonly RoleManager<AppRole> _roleManager;
    public DashboardController(ApplicationDbContext context, IUserRepository userRepository, IEventRepository eventRepository, 
        IProductRepository productRepository, ICustomerRepository customerRepository, INotyfService notyfService, 
        ICampaignRepository campaignRepository, ICampaignUserNoteRepository campaignUserNoteRepository,
        ICampaignUserTaskRepository campaignUserTaskRepository, IJobRepository jobRepository, ILeadsRepository leadsRepository,  RoleManager<AppRole> roleManager,
        OnlineUserService onlineUserService)
    {
        _context = context;
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _jobRepository = jobRepository;
        _leadsRepository = leadsRepository;
        _campaignRepository = campaignRepository;
        _campaignUserNoteRepository = campaignUserNoteRepository;
        _campaignUserTaskRepository = campaignUserTaskRepository;
        _notyfService = notyfService;
        _roleManager = roleManager;
        _onlineUserService = onlineUserService;
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

    private decimal ConversionRate
    {
        get
        {
            var clicks = _context.Campaigns.Select(c => c.Clicks).Count();
            var conversions = _context.Campaigns.Select(c => c.Conversions).Count();
            if (clicks == 0) return 0;
            return ((decimal)conversions / clicks) * 100;
        }
    }
    private decimal RevemuePercentage
    {
        get
        {
            var revenueTarget = _context.Campaigns.Select(c => c.RevenueTarget).Sum(c => c);
            var actualRevenue = _context.Campaigns.Select(c => c.ActualRevenue).Sum(c => c);
            var revenuePercentage = revenueTarget > 0 ? (actualRevenue / revenueTarget) * 100 : 0;
            return revenuePercentage;
        }
    }
    private List<AppUser> GetUsers()
    {
        return _context.AppUsers.ToList();
    }
    private List<Event> GetEvents()
    {
        return _context.Events.ToList();
    }
    private List<Products> GetProducts()
    {
        return _context.Products.ToList();
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (ActiveUser == null)
        {
            _notyfService.Error("You are not logged in, please login before accessing this page.");
            return RedirectToAction("Login", "Identity", new { area = "Identity" });
        }
        IndexViewModel index = null;
        index = new IndexViewModel
        {
            UserEvents = await _eventRepository.GetAllUserEventsAsync(),
            Events = await _eventRepository.GetAllEventsAsync(),
            Products = await _productRepository.GetAllProductsAsync(),
            Users = await _onlineUserService.GetOnlineUsers(),
            OfflineUsers = await _onlineUserService.GetOfflineUsers(),
            Jobs = await _jobRepository.GetAllJobsAsync(),
            Campaigns = await _campaignRepository.GetAllCampaignsAsync(),
            CampaignUserNotes = await _campaignUserNoteRepository.GetAllCampaignUserNotesAsync(),
            CampaignUserTasks = await _campaignUserTaskRepository.GetAllCampaignUserTasksAsync(),
            JobCount = await _jobRepository.CountJobsAsync(),
            UserCount = GetUsers().Count,
            EventCount = GetEvents().Count,
            ProductCount = GetProducts().Count,
            CustomerCount = await _customerRepository.CountCustomersAsync(),
            CampaignCount = await _campaignRepository.CampaignCountAsync(),
            CampaignUserNoteCount = await _campaignUserNoteRepository.CampaignUserNoteCountAsync(),
            CampaignUserTaskCount = await _campaignUserTaskRepository.CampaignUserTaskCountAsync(),
            RoleCount = await _roleManager.Roles.CountAsync(),
            LeadCount = await _leadsRepository.CountLeadsAsync(),
            JobRevenueTotal = await _context.Jobs.SumAsync(j => j.Revenue),
            JobEstimatateTotal = await _context.Jobs.SumAsync(j => j.EstimatedCost),
            ClicksCount = await _context.Campaigns.Select(c => c.Clicks).CountAsync(),
            CoversionCount = await _context.Campaigns.Select(c => c.Conversions).CountAsync(),
            UserJobs = await _jobRepository.GetAllUserJobsAsync(),
        };
        
        List<DataPoint> dataPoints1 = new List<DataPoint>();
			List<DataPoint> dataPoints2 = new List<DataPoint>();
			List<DataPoint> dataPoints3 = new List<DataPoint>();
 
			dataPoints1.Add(new DataPoint(1451586600000, 50000, null));
			dataPoints1.Add(new DataPoint(1454265000000, 40000, null));
			dataPoints1.Add(new DataPoint(1456770600000, 30000, null));
			dataPoints1.Add(new DataPoint(1459449000000, 35000, null));
			dataPoints1.Add(new DataPoint(1462041000000, 43000, null));
			dataPoints1.Add(new DataPoint(1464719400000, 60000, null));
			dataPoints1.Add(new DataPoint(1467311400000, 35000, null));
			dataPoints1.Add(new DataPoint(1469989800000, 50000, null));
			dataPoints1.Add(new DataPoint(1472668200000, 70000, "High Renewals"));
			dataPoints1.Add(new DataPoint(1475260200000, 35000, null));
			dataPoints1.Add(new DataPoint(1477938600000, 30000, null));
			dataPoints1.Add(new DataPoint(1480530600000, 37000, null));
 
			dataPoints2.Add(new DataPoint(1451586600000, 45000, null));
			dataPoints2.Add(new DataPoint(1454265000000, 48000, null));
			dataPoints2.Add(new DataPoint(1456770600000, 40000, null));
			dataPoints2.Add(new DataPoint(1459449000000, 41000, null));
			dataPoints2.Add(new DataPoint(1462041000000, 49000, null));
			dataPoints2.Add(new DataPoint(1464719400000, 46000, null));
			dataPoints2.Add(new DataPoint(1467311400000, 42000, null));
			dataPoints2.Add(new DataPoint(1469989800000, 43000, null));
			dataPoints2.Add(new DataPoint(1472668200000, 50000, null));
			dataPoints2.Add(new DataPoint(1475260200000, 43000, null));
			dataPoints2.Add(new DataPoint(1477938600000, 42000, null));
			dataPoints2.Add(new DataPoint(1480530600000, 50000, null));
 
			dataPoints3.Add(new DataPoint(1451586600000, 27000, null));
			dataPoints3.Add(new DataPoint(1454265000000, 21000, null));
			dataPoints3.Add(new DataPoint(1456770600000, 12000, null));
			dataPoints3.Add(new DataPoint(1459449000000, 18000, null));
			dataPoints3.Add(new DataPoint(1462041000000, 24000, null));
			dataPoints3.Add(new DataPoint(1464719400000, 33000, null));
			dataPoints3.Add(new DataPoint(1467311400000, 16000, null));
			dataPoints3.Add(new DataPoint(1469989800000, 29000, null));
			dataPoints3.Add(new DataPoint(1472668200000, 38000, null));
			dataPoints3.Add(new DataPoint(1475260200000, 24000, null));
			dataPoints3.Add(new DataPoint(1477938600000, 12000, null));
			dataPoints3.Add(new DataPoint(1480530600000, 19000, null));
 
 
			ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
			ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
			ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);
        
        ViewBag.ClicksToConversions = ConversionRate;
        ViewBag.ActualToTarget = RevemuePercentage;
        ViewBag.JobProfitPercentage = _context.Jobs.Select(j => j.GetProfitPercentage());
        ViewBag.Users = await _userRepository.GetAllUsersAsync();
        ViewBag.Events = await _eventRepository.GetAllEventsAsync();
        ViewBag.Products = await _productRepository.GetAllProductsAsync();
        ViewBag.user = ActiveUser;
        return View(index);
    }

  
   

}