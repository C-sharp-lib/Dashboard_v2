using System.Text.Json;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Dash.Areas.Admin.Models;
using Dash.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dash.Services;

public class GeoService
{
    private readonly ApiSettings _apiSettings;
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notyfService;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public GeoService(IOptions<ApiSettings> apiSettings, ApplicationDbContext context, INotyfService notyfService,
        IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {

        _apiSettings = apiSettings.Value;
        _context = context;
        _notyfService = notyfService;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }
    
    /*public async Task<string> GetMapImageUrlAsync(string address)
    {
        string _apiKey = _apiSettings.ApiKey;
        // Step 1: Geocode the address to get coordinates (latitude and longitude)
        var geocodeUrl = $"https://geocode.search.hereapi.com/v1/geocode?q={address}&apiKey={_apiKey}";

        var response = await _httpClient.GetStringAsync(geocodeUrl);
        dynamic geocodeResult = JsonConvert.DeserializeObject(response);
        
        if (geocodeResult.items.Count == 0)
        {
            return null; // No result found for the address
        }

        // Extract coordinates (latitude, longitude)
        var lat = geocodeResult.items[0].position.lat;
        var lng = geocodeResult.items[0].position.lng;

        // Step 2: Generate a map image with a marker at the coordinates
        var mapImageUrl = $"https://image.maps.ls.hereapi.com/mia/1.6/visualization?apiKey={_apiKey}&lat={lat}&lng={lng}&zoom=14&marker={lat},{lng}";

        return mapImageUrl;
    }*/
    
}