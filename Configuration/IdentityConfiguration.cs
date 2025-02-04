using Dash.Areas.Identity.Models;
using Dash.Data;
using Microsoft.AspNetCore.Identity;

namespace Dash.Configuration;

public static class IdentityConfiguration
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>()
            .AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddUserManager<UserManager<AppUser>>()
            .AddDefaultTokenProviders();
        services.AddDefaultIdentity<AppUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });
        
        return services;
    }
}