using Dash.Areas.Identity.Models;
using Dash.Data;
using Microsoft.EntityFrameworkCore;

namespace Dash.Services;

public class OnlineUserService
{
    private readonly ApplicationDbContext _context;

    public OnlineUserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void UpdateUserActivity(string id)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            user.LastActive = DateTime.Now;
            user.IsOnline = true;
            _context.SaveChanges();
        }
    }

    public async Task<IEnumerable<AppUser>> GetOnlineUsers()
    {
        DateTime activeThreshold = DateTime.Now.AddMinutes(-10);
        return await _context.Users.Where(u => u.LastActive >= activeThreshold).ToListAsync();
    }

    public async Task<IEnumerable<AppUser>> GetOfflineUsers()
    {
        DateTime threshold = DateTime.Now.AddMinutes(-10);
        return await _context.Users.Where(u => u.LastActive < threshold || u.LastActive == null).ToListAsync();
    }
    
    public void MarkUserOffline(string userId)
    {
        var user =  _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            user.IsOnline = false; 
            user.LastActive = DateTime.Now;
            _context.SaveChanges();
        }
    }
    public void CheckOfflineUsers()
    {
        var threshold = DateTime.UtcNow.AddMinutes(-10);
        var offlineUsers = _context.Users.Where(u => u.LastActive < threshold || u.LastActive == null).ToList();

        foreach (var user in offlineUsers)
        {
            user.IsOnline = false;
            _context.SaveChanges();
        }
    }
    public void RemoveInactiveUsers(TimeSpan timeout)
    {
        var inactiveUsers = _context.Users
            .Where(u => u.LastActive < DateTime.UtcNow - timeout)
            .ToList();

        foreach (var user in inactiveUsers)
        {
            user.IsOnline = false;
            user.LastActive = null;
            _context.SaveChanges();
        }
    }
}