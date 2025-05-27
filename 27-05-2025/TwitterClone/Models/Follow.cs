using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Models{
    public class Follow{
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public int FollowerId { get; set; }

        [ForeignKey("FollowerId")]
        public User? Follower { get; set; }
    }
}