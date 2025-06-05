using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notify.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }

        [ForeignKey("Username")]
        public User User { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;        
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string Name => $"{FirstName} {LastName}";
    }
}