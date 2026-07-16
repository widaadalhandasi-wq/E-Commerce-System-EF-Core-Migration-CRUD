
using First_Wep_App;
using FirstWebApp.Repositories;
using FirstWebApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FirstWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //1) services container ( place to register program services ) / dependency injection container
            var builder = WebApplication.CreateBuilder(args); //start line of service container

            // Add services to the container.

            //1-register context ( make an object )
            builder.Services.AddDbContext<ProjectContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //service lifetime
            //Repo class
            builder.Services.AddScoped<ProductRepo>();
            //builder.Services.AddTransient<ProductRepo>();
            //builder.Services.AddSingleton<ProductRepo>();
            //builder.Services.AddScoped<CategoryRepo>();
            //builder.Services.AddScoped<UserRepo>();
            //builder.Services.AddScoped<ReviewRepo>();

            //Services class
            builder.Services.AddScoped<ProductService>();
            //builder.Services.AddScoped<CategoryService>();
            //builder.Services.AddScoped<UserService>();
            //builder.Services.AddScoped<ReveiwService>();


            //Controller class
            builder.Services.AddControllers();




            // Swagger ---Accept the request
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build(); //end line of service container
            ////////////////////////////////////////////////////////////////////////////



            // 2)Configure the HTTP request pipeline / middleware pipline

           //swagger ---check the service for each request
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection(); //middleware

            app.UseAuthorization(); //middleware


            app.MapControllers(); //middleware
            //////////////////////////////////





            //run application
            app.Run();
        }
    }
}
