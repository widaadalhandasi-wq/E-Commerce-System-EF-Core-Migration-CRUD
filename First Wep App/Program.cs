using First_Wep_App;
using FirstWebApp.Repositories;
using FirstWebApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FirstWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //services container ( place to register program services ) / dependency injection container
            var builder = WebApplication.CreateBuilder(args); //start line of service container

            // Add services to the container.

            // 1 - register context
            builder.Services.AddDbContext<ProjectContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // service lifetime
            builder.Services.AddScoped<ProductRepo>();
            builder.Services.AddScoped<UserRepo>();

            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<UserService>();


            // ── JWT Authentication ─────────────────────────────────────────────
            builder.Services.AddScoped<AuthService>();

            var jwtKey = builder.Configuration["JwtSettings:SecretKey"];
            var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
            var jwtAudience = builder.Configuration["JwtSettings:Audience"];

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        RoleClaimType = "role",
                        IssuerSigningKey = new SymmetricSecurityKey(
                                                       Encoding.UTF8.GetBytes(jwtKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var error = context.Exception.Message;
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var claims = context.Principal.Claims.ToList();
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            var error = context.Error;
                            var desc = context.ErrorDescription;
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();
            // ── end JWT Authentication ─────────────────────────────────────────────







            builder.Services.AddControllers();

            // ── Swagger with JWT support ───────────────────────────────────────
            builder.Services.AddEndpointsApiExplorer();
            //  builder.Services.AddSwaggerGen();


            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token in the box below"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            new List<string>()
        }
    });
            });

            var app = builder.Build(); //end line of service container
            ////////////////////////////////////////////////////////////////////


            // Configure the HTTP request pipeline / middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //jwt
            app.UseAuthentication();  // ← must be before UseAuthorization
            app.UseAuthorization();
            //jwt


            app.MapControllers();

            // run application
            app.Run();
        }
    }
}