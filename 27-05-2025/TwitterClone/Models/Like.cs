using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Models{
    public class Like{
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public int TweetId { get; set; }

        [ForeignKey("TweetId")]
        public Tweet? Tweet { get; set; }
    }
}