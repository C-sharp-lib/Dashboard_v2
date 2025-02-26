namespace Dash.Areas.Admin.Models;

public class JobsPaginateViewModel
{
    public IEnumerable<UserJobs> Jobs { get; set; }
    public IEnumerable<UserJobs> UserJobs { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}