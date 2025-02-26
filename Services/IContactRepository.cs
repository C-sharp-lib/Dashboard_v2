using Dash.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dash.Services;

public interface IContactRepository : IRepository<Contacts>
{
    Task<IEnumerable<Contacts>> GetAllContactsAsync();
    Task<Contacts> GetContactByIdAsync(int id);
    Task AddContactAsync([FromForm] AddContactViewModel model);
    Task UpdateContactAsync(int id, [FromForm] UpdateContactViewModel model);
    Task DeleteContactAsync(int id);
    Task<int> CountContactsAsync();
}