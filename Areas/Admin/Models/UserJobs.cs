using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dash.Areas.Identity.Models;

namespace Dash.Areas.Admin.Models;

public class UserJobs
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserJobId { get; set; }
    [Required]
    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; }
    public AppUser User { get; set; }
    [Required]
    [ForeignKey(nameof(JobId))]
    public int JobId { get; set; }
    public Jobs Job { get; set; }
    
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
}

public class UpdateUserJobs : UserJobs
{
    [Required]
    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; }
    public AppUser User { get; set; }
    [Required]
    [ForeignKey(nameof(JobId))]
    public int JobId { get; set; }
    public Jobs Job { get; set; }
}

