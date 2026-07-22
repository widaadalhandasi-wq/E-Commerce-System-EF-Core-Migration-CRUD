using FirstWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace First_Wep_App
{
    public class ProjectContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
        }
    }

}
