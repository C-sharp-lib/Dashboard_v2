using Dash.Areas.Admin.Models;
using Dash.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Services;

public class LeadsRepository : Repository<Leads>, ILeadsRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Leads> _dbSet;
    public LeadsRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Leads>();
    }

    public async Task<IEnumerable<Leads>> GetAllLeadsAsync()
    {
        return await _dbSet
            .Include(x => x.User)
            .ToListAsync();
    }

    public async Task<Leads> GetLeadByIdAsync(int id)
    {
        var lead = await _dbSet.Include(x => x.User).FirstOrDefaultAsync(x => x.LeadId == id);
        if (lead == null)
        {
            throw new NullReferenceException($"Lead with id {id} not found");
        }
        return lead;
    }

    public async Task AddLeadAsync([FromForm] AddLeadViewModel model)
    {
        var leads = new Leads
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            UserId = model.UserId,
            AlternatePhone = model.AlternatePhone,
            City = model.City,
            State = model.State,
            Zip = model.Zip,
            Country = model.Country,
            Status = model.Status,
            Source = model.Source,
            JobTitle = model.JobTitle,
            CompanyName = model.CompanyName,
            Industry = model.Industry,
            IsConverted = model.IsConverted,
            Notes = model.Notes,
            LastContactDate = model.LastContactDate,
            FollowUpDate = model.FollowUpDate
        };
        await _dbSet.AddAsync(leads);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLeadAsync(int id, [FromForm] UpdateLeadsViewModel model)
    {
        var lead = await _dbSet.Include(x => x.User).FirstOrDefaultAsync(x => x.LeadId == id);
        if (lead == null)
        {
            throw new NullReferenceException($"Lead with id {id} not found");
        }
        lead.FirstName = model.FirstName;
        lead.LastName = model.LastName;
        lead.Email = model.Email;
        lead.Phone = model.Phone;
        lead.Address = model.Address;
        lead.City = model.City;
        lead.State = model.State;
        lead.Zip = model.Zip;
        lead.Country = model.Country;
        lead.Status = model.Status;
        lead.Source = model.Source;
        lead.JobTitle = model.JobTitle;
        lead.CompanyName = model.CompanyName;
        lead.Industry = model.Industry;
        lead.IsConverted = model.IsConverted;
        lead.LastContactDate = model.LastContactDate;
        lead.FollowUpDate = model.FollowUpDate;
        lead.Notes = model.Notes;
        lead.UserId = model.UserId;
        _dbSet.Update(lead);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLeadAsync(int id)
    {
        var lead = await _dbSet.Include(x => x.User).FirstOrDefaultAsync(x => x.LeadId == id);
        if (lead == null)
        {
            throw new NullReferenceException($"Lead with id {id} not found");
        }
        _dbSet.Remove(lead);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountLeadsAsync()
    {
        return await _dbSet.CountAsync();
    }
}