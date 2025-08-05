namespace DotnetAPI.DTOs.User
{
    public class AddUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UpdateUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
