using DesignPatterns.ProxyPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Models
{
    public class User
    {
        public string Username { get; }
        public Role UserRole { get; }

        public User(string username, Role role)
        {
            Username = username;
            UserRole = role;
        }
    }
}
