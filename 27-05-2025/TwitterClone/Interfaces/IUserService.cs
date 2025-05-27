using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Interfaces
{
    public interface IUserService
    {
        User GetUserById(int id);
        User UpdateUser(User User);
        void DeleteUser(int id);
        // List<User>? SearchUser( UserSearchModel UserSearchModel );
    }
}
