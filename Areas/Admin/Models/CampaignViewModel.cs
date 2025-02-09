using System.ComponentModel.DataAnnotations;
using Dash.Areas.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Admin.Models;

public class AddCampaignViewModel
{
    [Required(ErrorMessage = "Please enter the title of the campaign")]
    [DataType(DataType.Text)]
    public string Title { get; set; }
    [Required(ErrorMessage = "Please enter the description of the campaign")]
    [DataType(DataType.Text)]
    public string Description { get; set; }
    [Required(ErrorMessage = "Please enter the type of the campaign")]
    [DataType(DataType.Text)]
    public string Type { get; set; }
    [Required(ErrorMessage = "Please enter the status of the campaign")]
    [DataType(DataType.Text)]
    public string Status { get; set; }
    [Required(ErrorMessage = "Please enter the start date of the campaign")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "Please enter the end date of the campaign")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    [Precision(10,2)]
    [Required(ErrorMessage = "Please enter the budget of the campaign")]
    public decimal Budget { get; set; }
    [Precision(10,2)]
    [Required(ErrorMessage = "Please enter the spend amount of the campaign")]
    public decimal Spend { get; set; }
    [Required(ErrorMessage = "Please enter the target audience of the campaign")]
    public string TargetAudience { get; set; }
    [Required(ErrorMessage = "Please enter the channel of the campaign")]
    public string Channel {  get; set; }
    [Required(ErrorMessage = "Please enter the goals of the campaign")]
    public string Goals { get; set; }
    [Precision(10,2)]
    [Required(ErrorMessage = "Please enter the revenue target of the campaign")]
    public decimal RevenueTarget { get; set; }
    [Precision(10, 2)]
    [Required(ErrorMessage = "Please enter the actual revenue of the campaign")]
    public decimal ActualRevenue { get; set; }
    [Required(ErrorMessage = "Please enter the impressions of the campaign")]
    public long Impressions { get; set; }
    [Required(ErrorMessage = "Please enter the clicks of the campaign")]
    [Range(1, 1000000000, ErrorMessage = "Quantity must be between 1 and 1000000000.")]
    public long Clicks { get; set; }
    [Required(ErrorMessage = "Please enter the leads generated of the campaign")]
    [Range(1, 1000000000, ErrorMessage = "Quantity must be between 1 and 1000000000.")]
    public int LeadsGenerated { get; set; }
    [Required(ErrorMessage = "Please enter the conversions of the campaign")]
    [Range(1, 1000000000, ErrorMessage = "Quantity must be between 1 and 1000000000.")]
    public int Conversions {  get; set; }
    [Precision(10,2)]
    [Required(ErrorMessage = "Please enter the return on investment of the campaign")]
    public decimal ROI { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class AddCampaignUserNotesViewModel : CampaignUserNotes
{
    [Required(ErrorMessage = "Please enter the campaign ID")]
    public int CampaignId { get; set; }
    public Campaigns Campaign { get; set; }
    [Required(ErrorMessage = "Please enter the user ID")]
    public string UserId { get; set; }
    public AppUser User { get; set; }
    [Required(ErrorMessage = "Please enter the return on note title of the campaign user note")]
    public string NoteTitle { get; set; }
    [Required(ErrorMessage = "Please enter the return on note content of the campaign user note")]
    [DataType(DataType.Text)]
    public string NoteContent { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class AddCampaignUserTasksViewModel : CampaignUserTasks
{
    [Required(ErrorMessage = "Please enter the campaign ID")]
    public int CampaignId { get; set; }
    public Campaigns Campaign { get; set; }
    [Required(ErrorMessage = "Please enter the user ID")]
    public string UserId { get; set; }
    public AppUser User { get; set; }
    [Required(ErrorMessage = "Please enter the title of the campaign user task")]
    public string TaskTitle { get; set; }
    [Required(ErrorMessage = "Please enter the description of the campaign user task")]
    public string TaskDescription { get; set; }
    [Required(ErrorMessage = "Please enter the priority of the campaign user task")]
    public string Priority { get; set; }
    [Required(ErrorMessage = "Please enter the start date of the campaign user task")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "Please enter the end date of the campaign user task")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class UpdateCampaignViewModel : Campaigns
{
    [Required(ErrorMessage = "Please enter the title of the campaign")]
    [DataType(DataType.Text)]
    public string Title { get; set; }
    [Required(ErrorMessage = "Please enter the description of the campaign")]
    [DataType(DataType.Text)]
    public string Description { get; set; }
    [Required(ErrorMessage = "Please enter the type of the campaign")]
    [DataType(DataType.Text)]
    public string Type { get; set; }
    [Required(ErrorMessage = "Please enter the status of the campaign")]
    [DataType(DataType.Text)]
    public string Status { get; set; }
    [Required(ErrorMessage = "Please enter the start date of the campaign")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "Please enter the end date of the campaign")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    [Precision(10,2)]
    [Required(ErrorMessage = "Please enter the budget of the campaign")]
    public decimal Budget { get; set; }
    [Precision(10,2)]
    [Required(ErrorMessage = "Please enter the spend amount of the campaign")]
    public decimal Spend { get; set; }
    [Required(ErrorMessage = "Please enter the target audience of the campaign")]
    public string TargetAudience { get; set; }
    [Required(ErrorMessage = "Please enter the channel of the campaign")]
    public string Channel {  get; set; }
    [Required(ErrorMessage = "Please enter the goals of the campaign")]
    public string Goals { get; set; }
    [Precision(10,2)]
    [Required(ErrorMessage = "Please enter the revenue target of the campaign")]
    public decimal RevenueTarget { get; set; }
    [Precision(10, 2)]
    [Required(ErrorMessage = "Please enter the actual revenue of the campaign")]
    public decimal ActualRevenue { get; set; }
    [Required(ErrorMessage = "Please enter the impressions of the campaign")]
    public long Impressions { get; set; }
    [Required(ErrorMessage = "Please enter the clicks of the campaign")]
    [Range(1, 1000000000, ErrorMessage = "Quantity must be between 1 and 1000000000.")]
    public long Clicks { get; set; }
    [Required(ErrorMessage = "Please enter the leads generated of the campaign")]
    [Range(1, 1000000000, ErrorMessage = "Quantity must be between 1 and 1000000000.")]
    public int LeadsGenerated { get; set; }
    [Required(ErrorMessage = "Please enter the conversions of the campaign")]
    [Range(1, 1000000000, ErrorMessage = "Quantity must be between 1 and 1000000000.")]
    public int Conversions {  get; set; }
    [Precision(10,2)]
    [Required(ErrorMessage = "Please enter the return on investment of the campaign")]
    public decimal ROI { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class UpdateCampaignUserNotesViewModel
{
    [Required(ErrorMessage = "Please enter the campaign ID")]
    public int CampaignId { get; set; }
    public Campaigns Campaign { get; set; }
    [Required(ErrorMessage = "Please enter the user ID")]
    public string UserId { get; set; }
    public AppUser User { get; set; }
    [Required(ErrorMessage = "Please enter the return on note title of the campaign user note")]
    public string NoteTitle { get; set; }
    [Required(ErrorMessage = "Please enter the return on note content of the campaign user note")]
    [DataType(DataType.Text)]
    public string NoteContent { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class UpdateCampaignUserTasksViewModel
{
    [Required(ErrorMessage = "Please enter the campaign ID")]
    public int CampaignId { get; set; }
    public Campaigns Campaign { get; set; }
    [Required(ErrorMessage = "Please enter the user ID")]
    public string UserId { get; set; }
    public AppUser User { get; set; }
    [Required(ErrorMessage = "Please enter the title of the campaign user task")]
    public string TaskTitle { get; set; }
    [Required(ErrorMessage = "Please enter the description of the campaign user task")]
    public string TaskDescription { get; set; }
    [Required(ErrorMessage = "Please enter the priority of the campaign user task")]
    public string Priority { get; set; }
    [Required(ErrorMessage = "Please enter the start date of the campaign user task")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "Please enter the end date of the campaign user task")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}