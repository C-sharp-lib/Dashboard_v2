using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dash.Areas.Admin.Models;

public class Contacts
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContactId { get; set; }
    public string ImageUrl { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [DataType(DataType.Text)]
    public string FirstName { get; set; }
    [DataType(DataType.Text)]
    public string LastName { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }
    [DataType(DataType.Text)]
    public string Address { get; set; }
    [DataType(DataType.Text)]
    public string City { get; set; }
    [DataType(DataType.Text)]
    public string State { get; set; }
    [DataType(DataType.Text)]
    public string ZipCode { get; set; }
    [DataType(DataType.Text)]
    public string Country { get; set; }
    [DataType(DataType.Url)]
    public string Website { get; set; }
    [DataType(DataType.Text)]
    public string JobTitle { get; set; }
    [DataType(DataType.Text)]
    public string Department { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string WorkPhone { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string MobilePhone { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string Fax { get; set; }
    [DataType(DataType.Text)]
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}