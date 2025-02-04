using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Dash.Areas.Identity.Models;

public class AddRoleViewModel : IdentityRole
{
    [Required]
    public string Name { get; set; }
}
public class UserRolesViewModel : IdentityUserRole<string>
{
    [Required]
    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; }
    public AppUser User {get;set;}
    [Required]
    [ForeignKey(nameof(RoleId))]
    public string RoleId { get; set; }
    public AppRole Role { get; set; }
}
public class RoleAssignViewModel
{
    
    public string RoleID { get; set; }
    public string? RoleName { get; set; }
    public bool RoleExist { get; set; }
}