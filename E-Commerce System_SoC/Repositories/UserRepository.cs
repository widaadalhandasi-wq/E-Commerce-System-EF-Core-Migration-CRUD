using Backend_session_10_SoC.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_session_10_SoC.Repositories
{
    public class UserRepository
    {
        private EcommerceContext context;

        public UserRepository(EcommerceContext context)
        {
            this.context = context;
        }

        public List<User> GetAll()
        {
            return context.Users.ToList();
        }

        public User GetById(int userId)
        {
            return context.Users.FirstOrDefault(u => u.userId == userId);
        }

        public User GetWithOrdersAndItems(int userId)
        {
            return context.Users
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(i => i.Product)
                .FirstOrDefault(u => u.userId == userId);
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
    }
}
