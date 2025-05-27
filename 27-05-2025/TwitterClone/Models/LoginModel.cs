namespace TwitterClone.Models
{
    public class LoginModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public LoginModel() { }

        public LoginModel(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}