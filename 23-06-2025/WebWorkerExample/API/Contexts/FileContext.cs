using Microsoft.EntityFrameworkCore;

namespace API.Contexts
{
    public class FileContext : DbContext
    {

        public FileContext(DbContextOptions<FileContext> options) : base(options)
        {

        }

    }
}