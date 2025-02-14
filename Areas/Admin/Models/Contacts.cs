using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dash.Areas.Admin.Models;

public class Contacts
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContactId { get; set; }
    [ForeignKey("CustomerId")]
    public int CustomerId { get; set; }
    public Customers Customer { get; set; }
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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}