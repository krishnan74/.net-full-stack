using DotnetAPI.Contexts;
using DotnetAPI.Interfaces;
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Repositories
{
    public class ContactURepository : Repository<int, ContactU>
    {
        public ContactURepository(DotnetAPIContext context) : base(context) { }

        public override async Task<ContactU> Get(int key)
        {
            var contact = await _context.ContactUs.SingleOrDefaultAsync(c => c.id == key);
            return contact ?? throw new KeyNotFoundException($"No contact with the given ID: {key}");
        }

        public override async Task<IEnumerable<ContactU>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var contacts = _context.ContactUs;
            if (!contacts.Any())
                return Enumerable.Empty<ContactU>();
            return await contacts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
