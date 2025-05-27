using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Interfaces
{
    public interface IAuthService
    {
        int RegisterUser(User User);
        bool Login(string username, string password);
        bool Logout(int userId);
        bool IsUserAuthenticated(int userId);
        bool IsUsernameAvailable(string username);
    }
}
