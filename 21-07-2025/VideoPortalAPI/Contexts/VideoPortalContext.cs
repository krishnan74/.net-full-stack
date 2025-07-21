using VideoPortalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace VideoPortalAPI.Contexts
{
    public class VideoPortalContext : DbContext
    {

        public VideoPortalContext(DbContextOptions<VideoPortalContext> options) : base(options)
        {

        }

        public DbSet<TrainingVideo> TrainingVideos { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

    }
}