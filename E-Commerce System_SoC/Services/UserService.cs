using Backend_session_10_SoC.Models;
using Backend_session_10_SoC.Repositories;

namespace Backend_session_10_SoC.Services
{
    public class UserService
    {
        private UserRepository userRepo;

        public UserService(UserRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        public List<User> GetAll()
        {
            return userRepo.GetAll();
        }

        public User GetById(int userId)
        {
            return userRepo.GetById(userId);
        }

        public User GetWithOrdersAndItems(int userId)
        {
            return userRepo.GetWithOrdersAndItems(userId);
        }

        public void Register(string name, string email, string password, string fullName,
                             string phoneNumber, string address)
        {
            User user = new User();
            user.Name             = name;
            user.email            = email;
            user.passwordHash     = password;   // production: hash this
            user.fullName         = fullName;
            user.phoneNumber      = phoneNumber;
            user.address          = address;
            user.registrationDate = DateTime.Now;
            user.isActive         = true;

            userRepo.Add(user);
        }

        public int GetLastUserId()
        {
            List<User> all = userRepo.GetAll();
            return all[all.Count - 1].userId;
        }
    }
}
