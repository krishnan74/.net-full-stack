using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Models{
    public class HashTagTweet{
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int HashTagId { get; set; }

        [ForeignKey("HashTagId")]
        public HashTag? HashTag { get; set; }

        public int TweetId { get; set; }

        [ForeignKey("TweetId")]
        public Tweet? Tweet { get; set; }
    }
}