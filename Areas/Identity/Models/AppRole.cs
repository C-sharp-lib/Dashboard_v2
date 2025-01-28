using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Dash.Areas.Identity.Models;
public class AppRole : IdentityRole
{
    public IEnumerable<AppUserRoles> UserRoles { get; set; }
}