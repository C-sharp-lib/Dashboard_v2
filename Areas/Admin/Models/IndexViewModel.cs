﻿using Dash.Areas.Identity.Models;
using Dash.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Admin.Models;

public class IndexViewModel
{
    public IEnumerable<AppUser> Users { get; set; }
    public IEnumerable<AppUser> OfflineUsers { get; set; }
    public IEnumerable<UserEvents> UserEvents { get; set; }
    public IEnumerable<Event> Events { get; set; }
    public IEnumerable<Products> Products { get; set; }
    public IEnumerable<Campaigns> Campaigns { get; set; }
    public IEnumerable<CampaignUserNotes> CampaignUserNotes { get; set; }
    public IEnumerable<CampaignUserTasks> CampaignUserTasks { get; set; }
    public IEnumerable<Jobs> Jobs { get; set; }
    public IEnumerable<UserJobs> UserJobs { get; set; }
    public int RoleCount { get; set; }
    public int CustomerCount { get; set; }

    public Event SelectedEvent { get; set; }
    public int UserCount { get; set; }
    public int EventCount { get; set; }
    public int ProductCount { get; set; }
    public int CampaignCount { get; set; }
    public int CampaignUserNoteCount { get; set; }
    public int CampaignUserTaskCount { get; set; }
    public int JobCount { get; set; }
    public int LeadCount { get; set; }
    public long ClicksCount { get; set; }
    public long CoversionCount { get; set; }
    public decimal JobEstimatateTotal { get; set; }
    public decimal JobRevenueTotal { get; set; }
    /*public decimal JobProfitTotal { get; set; }*/
    /*public decimal ConversionRate { get; set; }
    public decimal JobProfitPercent { get; set; }*/
    
}