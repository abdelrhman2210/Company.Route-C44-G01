using Company.Route_C44_G01.BLL.Interfaces;
using Company.Route_C44_G01.BLL.Repositories;
using Company.Route_C44_G01.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepository>(); // Register EmployeeRepository for dependency injection
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies(); // Enable lazy loading proxies
            }); // Register CompanyDbContext for dependency injection

            // LifeTime :
            //builder.Services.addscoped() // Create New Instance Per Request - unreachable object after request is completed
            //builder.Services.AddSingleton() // Create Single Instance For All Requests - reachable object as long as the application is running
            //builder.Services.addtransient() // Create New Instance Every Time You Request It - unreachable object after request is completed

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
