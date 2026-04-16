using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WMS.Core.Entities;
using WMS.Core.Repositories;
using WMS.Infrastructure.Context;
using WMS.Infrastructure.DbInitializar;
using WMS.Infrastructure.Implementations;

namespace WMS.WebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.AllowedUserNameCharacters = "abc...xyzABC...XYZ0123456789-._@+ أبتثجحخدذرزسشصضطظعغفقكلمنهوي";
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(3);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;

            }).AddDefaultTokenProviders().AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IDbInitializar,DbInitializar >();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            SeedDb();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{Area=Admin}/{controller=User}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();

            void SeedDb()
            {

                using (var scope = app.Services.CreateScope())
                {

                    var dbInitizer = scope.ServiceProvider.GetRequiredService<IDbInitializar>();

                    dbInitizer.Initialize();
                }
            }

        }
    }
}
