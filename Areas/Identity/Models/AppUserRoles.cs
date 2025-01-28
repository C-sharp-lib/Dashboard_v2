using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Dash.Areas.Identity.Models;
[Table("AspNetUserRoles", Schema ="aspnet-Dash-020194b5-f0f8-4d5b-8e75-48a4efce9697")]

public class AppUserRoles : IdentityUserRole<string>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; }
    [ForeignKey(nameof(RoleId))]
    public string RoleId { get; set; }
    public virtual AppUser User { get; set; }
    public virtual AppRole Role { get; set; }
}