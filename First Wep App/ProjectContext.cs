using FirstWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace First_Wep_App
{
    public class ProjectContext:DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(
                                  "Server=localhost;Database=EcommerceDB2026;Trusted_Connection=True;TrustServerCertificate=True; "
                                );
        }

    }
}
