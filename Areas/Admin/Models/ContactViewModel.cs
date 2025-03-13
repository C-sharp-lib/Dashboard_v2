using System.ComponentModel.DataAnnotations;

namespace Dash.Areas.Admin.Models;

public class AddContactViewModel : Contacts
{
    public IFormFile ImageUrl { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; } 
    public string Website { get; set; }
    public string JobTitle { get; set; }
    public string Department { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string WorkPhone { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string MobilePhone { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string Fax { get; set; }
    public string Bio { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class UpdateContactViewModel : Contacts
{
    public IFormFile ImageUrl { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; } 
    public string Website { get; set; }
    public string JobTitle { get; set; }
    public string Department { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string WorkPhone { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string MobilePhone { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string Fax { get; set; }

    public string Bio { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}