using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dash.Areas.Identity.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Dash.Areas.Admin.Models;

public class AddJobsViewModel : Jobs
{
    public string Title { get; set; }
    [DataType(DataType.Text)]
    public string Description { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public string Company { get; set; }
    public string ContactFax { get; set; }
    public string ContactPerson { get; set; }
    public string ContactNumber { get; set; }
    public string ContactEmail { get; set; }
    public string ContactAddress { get; set; }
    public string ContactCity { get; set; }
    public string ContactState { get; set; }
    public string ContactZip { get; set; }
    public string ContactCountry { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Precision(10,2)]
    public decimal EstimatedCost { get; set; }
    public IEnumerable<UserJobs> UserJobs { get; set; }
}
