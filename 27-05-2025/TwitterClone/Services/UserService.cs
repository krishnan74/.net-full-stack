using TwitterClone.Interfaces;
using TwitterClone.Models;
using TwitterClone.Repositories;


namespace TwitterClone.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepository;

        public UserService(IRepository<int, User> userRepository)
        {
            _userRepository = userRepository;
        }
        

        public User GetUserById(int id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                if (user != null)
                {
                    return user;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public User UpdateUser(User user)
        {
            try
            {
                var result = _userRepository.Update(user);
                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public void DeleteUser(int id)
        {
            try
            {
                _userRepository.Delete(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // public List<User>? SearchUser(UserSearchModel userSearchModel)
        // {
            
        //     return null;
        // }
    }
}