using FileAPI.Contexts;
using FileAPI.Interfaces;
using FileAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FileAPI.Repositories
{
    public  class FileRepository : Repository<int, FileDetails>
    {
        public FileRepository(FileManagerContext fileManagerContext) : base(fileManagerContext)
        {
        }

        public override async Task<FileDetails> Get(int key)
        {
            var file = await _fileManagerContext.fileDetails.SingleOrDefaultAsync(p => p.Id == key);

            return file??throw new Exception("No File with the given ID");
        }

        public override async Task<IEnumerable<FileDetails>> GetAll()
        {
            var files = _fileManagerContext.fileDetails;
            if (files.Count() == 0)
                throw new Exception("No File in the database");

            return (await files.ToListAsync());
        }
    }
}