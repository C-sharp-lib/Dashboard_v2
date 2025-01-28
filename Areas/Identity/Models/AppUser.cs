﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dash.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;

namespace Dash.Areas.Identity.Models;
public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime? DateOfBirth { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime? DateOfHire { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public IEnumerable<UserSchedules> UserSchedules { get; set; }
    public IEnumerable<UserEvents> UserEvents { get; set; }
    public IEnumerable<CampaignUserNotes> CampaignUserNotes { get; set; }
    public IEnumerable<CampaignUserTasks> CampaignUserTasks { get; set; }
    public IEnumerable<AppUserRoles> UserRoles { get; set; }
    public IEnumerable<Jobs> Jobs { get; set; }
}