using Dash.Services;

namespace Dash.Configuration;

public static class AppConfiguration
{
    public static IServiceCollection AddAppConfiguration(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddHttpContextAccessor();
        services.AddControllersWithViews();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.IOTimeout = TimeSpan.FromMinutes(10);

        });
        services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = "/Identity/Identity/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        });
        return services;
    }
}