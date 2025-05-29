using System;

namespace BankApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool SessionActive { get; set; } = false;
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PANNumber { get; set; }
        public ICollection<Account>? UserAccounts { get; set; }
    }
}