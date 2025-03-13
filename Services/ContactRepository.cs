using Dash.Areas.Admin.Models;
using Dash.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dash.Services;

public class ContactRepository : Repository<Contacts>, IContactRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Contacts> _dbSet;
    private readonly IWebHostEnvironment _webenv;
    public ContactRepository(ApplicationDbContext context, IWebHostEnvironment env) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Contacts>();
        _webenv = env;
    }

    public async Task<IEnumerable<Contacts>> GetAllContactsAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Contacts> GetContactByIdAsync(int id)
    {
        var contact = await _dbSet.FirstOrDefaultAsync(c => c.ContactId == id);
        if (contact == null)
        {
            throw new NullReferenceException("Contact not found");
        }
        return contact;
    }

    public async Task AddContactAsync([FromForm] AddContactViewModel model)
    {
        /*string uniqueFileName3 = "";
        if (model.ImageUrl != null && model.ImageUrl.Length > 0)
        {
            var permittedExtensions3 = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension3 = Path.GetExtension(model.ImageUrl.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension3) || !permittedExtensions3.Contains(extension3))
            {
                throw new FormatException("Invalid image extension");
            }

            string fileName3 = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
            uniqueFileName3 = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageUrl.FileName);
            string uploadsFolder3 = Path.Combine(_webenv.WebRootPath, "Uploads/Contacts/");
            if (!Directory.Exists(uploadsFolder3))
            {
                Directory.CreateDirectory(uploadsFolder3);
            }

            string filePath = Path.Combine(uploadsFolder3, uniqueFileName3);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageUrl.CopyToAsync(fileStream);
            }
        }*/

        string uploadsFolder = Path.Combine(_webenv.WebRootPath, "Uploads/Contacts/");
        if (model.ImageUrl != null && model.ImageUrl.Length > 0)
        {
            string uniqueFileName1 =
                Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageUrl.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName1);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageUrl.CopyToAsync(fileStream);
            }
            var contact = new Contacts
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Address = model.Address,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                Country = model.Country,
                Website = model.Website,
                JobTitle = model.JobTitle,
                Department = model.Department,
                WorkPhone = model.WorkPhone,
                MobilePhone = model.MobilePhone,
                Bio = model.Bio,
                Fax = model.Fax,
                ImageUrl = uniqueFileName1 != null! ? Path.Combine("Uploads/Contacts/", uniqueFileName1) : null!
            };
            await _dbSet.AddAsync(contact);
            await _context.SaveChangesAsync();  
        }
    }

    public async Task UpdateContactAsync(int id, UpdateContactViewModel model)
    {
        var contact = await _dbSet.FirstOrDefaultAsync(c => c.ContactId == id);
        if (contact == null)
        {
            throw new NullReferenceException("Contact not found");
        }
        string uploadsFolder = Path.Combine(_webenv.WebRootPath, "Uploads/Contacts/");
        if (model.ImageUrl != null && model.ImageUrl.Length > 0)
        {
            string uniqueFileName1 =
                Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageUrl.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName1);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageUrl.CopyToAsync(fileStream);
            }

            contact.ImageUrl = uniqueFileName1;
        }
        contact.FirstName = model.FirstName;
        contact.LastName = model.LastName;
        contact.PhoneNumber = model.PhoneNumber;
        contact.Email = model.Email;
        contact.Address = model.Address;
        contact.City = model.City;
        contact.State = model.State;
        contact.ZipCode = model.ZipCode;
        contact.Country = model.Country;
        contact.Website = model.Website;
        contact.JobTitle = model.JobTitle;
        contact.Department = model.Department;
        contact.WorkPhone = model.WorkPhone;
        contact.MobilePhone = model.MobilePhone;
        contact.Fax = model.Fax;
        contact.Bio = model.Bio;
        _dbSet.Update(contact);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteContactAsync(int id)
    {
        var contact = await _dbSet.FirstOrDefaultAsync(c => c.ContactId == id);
        if (contact == null)
        {
            throw new NullReferenceException("Contact not found");
        }

        _dbSet.Remove(contact);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountContactsAsync()
    {
        return await _dbSet.CountAsync();
    }
}