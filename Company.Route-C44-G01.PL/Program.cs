using AutoMapper;
using Company.Route_C44_G01.BLL;
using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.BLL.Repositories;
using Company.Route_C44_G01.DAL.Data.Contexts;
using Company.Route_C44_G01.DAL.Models;
using Company.Route_C44_G01.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Route_C44_G01.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Add MVC services to the container
            builder.Services.AddScoped<IDepartmentRepo,DepartmentRepository>(); // Register DepartmentRepository for dependency injection
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Register UnitOfWork for dependency injection
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepository>(); // Register EmployeeRepository for dependency injection
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>(); // Add Identity services to the container and applying dependency injection

            //builder.Services.AddTransient<IMapper, Mapper>(); // Register AutoMapper for dependency injection
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies(); // Enable lazy loading proxies
            }); // Register CompanyDbContext for dependency injection

            // builder.Services.AddAutoMapper(typeof(EmpProfile)); // Register AutoMapper for dependency injection
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmpProfile())); // Register AutoMapper for dependency injection
            builder.Services.AddAutoMapper(M => M.AddProfile(new DeptProfile())); // Register AutoMapper for dependency injection


            // LifeTime :
            //builder.Services.addscoped() // Create New Instance Per Request - unreachable object after request is completed
            //builder.Services.AddSingleton() // Create Single Instance For All Requests - reachable object as long as the application is running
            //builder.Services.addtransient() // Create New Instance Every Time You Request It - unreachable object after request is completed

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                //config.LogoutPath = "/Account/SignOut";
                //config.AccessDeniedPath = "/Account/AccessDenied";
                config.ExpireTimeSpan = TimeSpan.FromMinutes(10); 
            }); // Configure application cookie settings

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
