namespace TwitterClone.Models{
    public class User{
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsLoggedIn { get; set; } = false;
        public string? ProfilePictureUrl { get; set; }

        public ICollection<Tweet>? Tweets { get; set; }
        
    }
}