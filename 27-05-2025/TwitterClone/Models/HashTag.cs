namespace TwitterClone.Models
{
    public class HashTag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<HashTagTweet>? HashTagTweets { get; set; }
    }
}