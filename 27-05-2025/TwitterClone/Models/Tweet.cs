using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Models{
    public class Tweet{
        public int Id { get; set; }
        public string Caption { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<HashTagTweet>? HashTagTweets { get; set; }
    }
}