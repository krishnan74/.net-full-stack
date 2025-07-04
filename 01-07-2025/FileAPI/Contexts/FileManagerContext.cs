using FileAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace FileAPI.Contexts
{
    public class FileManagerContext : DbContext
    {

        public FileManagerContext(DbContextOptions<FileManagerContext> options) : base(options)
        {

        }

        public DbSet<FileDetails> fileDetails { get; set; }

    }
}