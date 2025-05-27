using TwitterClone.Models;
using Microsoft.EntityFrameworkCore;

namespace TwitterClone.Contexts
{
    public class TwitterContext : DbContext
    {

        public TwitterContext(DbContextOptions<TwitterContext> options) : base(options)
        {

        }

        public DbSet<User> users { get; set; }
        public DbSet<Tweet> tweets { get; set; }
        public DbSet<Like> likes { get; set; }
        public DbSet<HashTag> hash_tags { get; set; }
        public DbSet<HashTagTweet> hash_tag_tweets { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Follow> follows { get; set; }
        
    }
}