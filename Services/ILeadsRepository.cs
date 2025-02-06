using Dash.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Services;

public interface ILeadsRepository : IRepository<Leads>
{
    Task<IEnumerable<Leads>> GetAllLeadsAsync();
    Task<Leads> GetLeadByIdAsync(int id);
    Task AddLeadAsync([FromForm] AddLeadViewModel model);
    Task UpdateLeadAsync(int id, [FromForm] UpdateLeadsViewModel model);
    Task DeleteLeadAsync(int id);
    Task<int> CountLeadsAsync();
}