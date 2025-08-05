using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Repositories
{
    public class ContactUsRepository : Repository<int, ContactUs>
    {
        public ContactUsRepository(DotnetAPIContext context) : base(context) { }

        public override async Task<ContactUs> Get(int key)
        {
            var contact = await _context.ContactUs.SingleOrDefaultAsync(c => c.Id == key);
            return contact ?? throw new KeyNotFoundException($"No contact with the given ID: {key}");
        }

        public override async Task<IEnumerable<ContactUs>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var contacts = _context.ContactUs;
            if (!contacts.Any())
                return Enumerable.Empty<ContactUs>();
            return await contacts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
