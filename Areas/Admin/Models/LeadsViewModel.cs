using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dash.Areas.Identity.Models;

namespace Dash.Areas.Admin.Models;

public class AddLeadViewModel : Leads
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string AlternatePhone { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }
    public string Status { get; set; }
    public string Source { get; set; }
    public string Priority { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string Industry { get; set; }
    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public bool IsConverted { get; set; }
    public string Notes { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime LastContactDate { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime FollowUpDate { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class UpdateLeadsViewModel : Leads
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LeadId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string AlternatePhone { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    [DataType(DataType.PostalCode)]
    public string Zip { get; set; }
    public string Country { get; set; }
    public string Status { get; set; }
    public string Source { get; set; }
    public string Priority { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public string Industry { get; set; }
    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public bool IsConverted { get; set; }
    public string Notes { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime LastContactDate { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime FollowUpDate { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}