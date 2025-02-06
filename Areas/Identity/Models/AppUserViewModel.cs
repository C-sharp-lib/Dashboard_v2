using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Dash.Areas.Identity.Models;

public class AppUserViewModel : AppUser
{
    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required]
    [DataType(DataType.Text)]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "First name is required")]
    [DataType(DataType.Text)]
    public string? FirstName { get; set; }
    [Required(ErrorMessage = "Middle name is required")]
    [DataType(DataType.Text)]
    public string? MiddleName { get; set; }
    [Required(ErrorMessage = "Last name is required")]
    [DataType(DataType.Text)]
    public string? LastName { get; set; }
    public IFormFile? ImageUrl { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime? DateOfBirth { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [Required(ErrorMessage = "Date of hire is required")]
    public DateTime? DateOfHire { get; set; }
    [Required(ErrorMessage = "Address is required")]
    [DataType(DataType.Text)]
    public string? Address { get; set; }
    [Required(ErrorMessage = "City is required")]
    [DataType(DataType.Text)]
    public string? City { get; set; }
    [Required(ErrorMessage = "State is required")]
    [DataType(DataType.Text)]
    public string? State { get; set; }
    [Required(ErrorMessage = "Zipcode is required")]
    [DataType(DataType.Text)]
    public string ZipCode { get; set; }
    [Required(ErrorMessage = "Bio is required")]
    [DataType(DataType.Text)]
    public string? Bio { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; }
}