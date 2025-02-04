using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Dash.Areas.Identity.Models;

public class AppUserRoles : IdentityUserRole<string>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string UserId { get; set; }
    public string RoleId { get; set; }
    public virtual AppUser User { get; set; }
    public virtual AppRole Role { get; set; }
}