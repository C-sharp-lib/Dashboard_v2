using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dash.Areas.Identity.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;

namespace Dash.Areas.Admin.Models;

public class Jobs
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int JobId { get; set; }
    public string Title { get; set; }
    [DataType(DataType.Text)]
    public string Description { get; set; }
    public string Category { get; set; }
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
    public string Type { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime UpdatedAt { get; set; }
    public IEnumerable<UserJobs> UserJobs { get; set; }
    
    [Precision(10,2)]
    public decimal EstimatedCost { get; set; }
    [Precision(10,2)]
    public decimal Revenue { get; set; }
    [Precision(10,2)]
    public decimal Profit => Revenue - EstimatedCost;

    public decimal GetProfit()
    {
        return Revenue - EstimatedCost;
    }
    public decimal GetProfitPercentage()
    {
        if(Revenue == 0) return 0;
        return ((Revenue - EstimatedCost) / Revenue) * 100;
    }
    [Precision(10,2)]
    public decimal ProfitPercent => Revenue > 0 ? (Profit / Revenue) * 100 : 0;
    
    
    
    public string TruncateWords(string text, int wordLimit)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }
        var words = text.Split(' ');
        if (words.Length <= wordLimit)
        {
            return text;
        }
        return string.Join(' ', words.Take(wordLimit)) + "...";
    }

    public Jobs()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
}