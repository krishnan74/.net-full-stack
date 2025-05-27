namespace TwitterClone.Models
{
    public class HashTag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<HashTagTweet>? HashTagTweets { get; set; }
    }
}