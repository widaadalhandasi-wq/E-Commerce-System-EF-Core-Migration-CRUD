using Backend_session_10_SoC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Backend_session_10_SoC
{
    public class EcommerceContext : DbContext
    {
        //1-Csharp Models registeration

        //register product means we are telling the database that 
        //we have a table called Products and it will be mapped to the Product model class
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> reviews { get; set; }


        //2-Database connection string
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(
                                  "Server=localhost;Database=EcommerceDB2026;Trusted_Connection=True;TrustServerCertificate=True; "
                                );
        }





    }
}
