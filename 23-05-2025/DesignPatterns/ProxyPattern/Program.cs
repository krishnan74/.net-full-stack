using System;
using DesignPatterns.Interfaces;
using DesignPatterns.Models;

namespace DesignPatterns.ProxyPattern
{
    // Role Enum
    public enum Role
    {
        Admin,
        User,
        Guest
    }
    
    // ProxyFile Class
    public class ProxyFile : IFile
    {
        private CustomFile _realFile;
        private User _user;

        public ProxyFile(string fileName, User user)
        {
            _realFile = new CustomFile(fileName);
            _user = user;
        }

        public void Read()
        {
            switch (_user.UserRole)
            {
                case Role.Admin:
                    _realFile.Read();
                    break;
                case Role.User:
                    _realFile.ReadMetadata();
                    break;
                case Role.Guest:
                default:
                    Console.WriteLine("[Access Denied] You do not have permission to read this file.");
                    break;
            }
        }
    }

    // Client Code
    class Program
    {
        static void Main(string[] args)
        {
            var users = new[]
            {
                new User("Alice", Role.Admin),
                new User("Bob", Role.Guest),
                new User("Charlie", Role.User)
            };

            foreach (var user in users)
            {
                Console.WriteLine($"\nUser: {user.Username} | Role: {user.UserRole}");
                IFile file = new ProxyFile("SensitiveData.txt", user);
                file.Read();
            }
        }
    }
}
