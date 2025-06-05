using Notify.Contexts;
using Notify.Interfaces;
using Notify.Models;
using Microsoft.EntityFrameworkCore;

namespace Notify.Repositories
{
    public  class FileRepository : Repository<int, FileDetails>
    {
        public FileRepository(NotifyContext notifyContext) : base(notifyContext)
        {
        }

        public override async Task<FileDetails> Get(int key)
        {
            var file = await _notifyContext.FileDetails.SingleOrDefaultAsync(p => p.Id == key);

            return file??throw new Exception("No File with the given ID");
        }

        public override async Task<IEnumerable<FileDetails>> GetAll()
        {
            var files = _notifyContext.FileDetails;
            if (files.Count() == 0)
                throw new Exception("No File in the database");

            return (await files.ToListAsync());
        }
    }
}