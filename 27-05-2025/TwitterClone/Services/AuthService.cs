using TwitterClone.Interfaces;
using TwitterClone.Models;
using TwitterClone.Repositories;
using TwitterClone.Utils;

namespace TwitterClone.Services
{
    public class AuthService : IAuthService{

        private readonly IRepository<int, User> _userRepository;

        public AuthService(IRepository<int, User> userRepository)
        {
            _userRepository = userRepository;
        }

        public int RegisterUser(User user){
            try
            {
                if( IsUsernameAvailable(user.UserName) == false )
                {
                    Console.WriteLine("Username is already taken.");
                    return -1;
                }

                
                var hash = SecurePasswordHasher.Hash(user.Password);

                user.Password = hash;

                var result = _userRepository.Add(user);

                if (result != null)
                {
                    return result.Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }

        public bool Login(string username, string password){
            try{
                var users = _userRepository.GetAll();
                var user = users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) && SecurePasswordHasher.Verify(password, u.Password));
                if( user != null ){
                    user.IsLoggedIn = true;
                    _userRepository.Update(user);
                }
                return user != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Logout(int userId)
        {
            try
            {
                var user = _userRepository.GetById(userId);
                if (user != null)
                {
                    user.IsLoggedIn = false;
                    _userRepository.Update(user);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool IsUserAuthenticated(int userId)
        {
            try
            {
                var user = _userRepository.GetById(userId);
                return user != null && user.IsLoggedIn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool IsUsernameAvailable(string username)
        {
            try
            {
                var users = _userRepository.GetAll();
                return !users.Any(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}