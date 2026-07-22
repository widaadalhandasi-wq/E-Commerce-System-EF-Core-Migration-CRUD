using First_Wep_App;
using FirstWebApp.Models;

namespace FirstWebApp.Repositories
{
    public class UserRepo
    {
        private ProjectContext context;

        public UserRepo(ProjectContext _context)
        {
            context = _context;
        }

        public List<User> GetAll()
        {
            return context.Users.ToList();
        }

        public User GetById(int id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByEmail(string email)
        {
            return context.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool EmailExists(string email)
        {
            return context.Users.Any(u => u.Email == email);
        }

        public void Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void Update()
        {
            context.SaveChanges();
        }

        public void Delete(User user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }
    }
}
