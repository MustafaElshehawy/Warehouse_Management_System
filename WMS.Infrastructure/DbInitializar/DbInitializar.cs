using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;
using WMS.Core.Entities;
using WMS.Infrastructure.Context;

namespace WMS.Infrastructure.DbInitializar
{
    public class DbInitializar:IDbInitializar
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializar(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            //Migration 
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {

                    _context.Database.Migrate();
                }

            }
            catch (Exception)
            {

                throw;
            }

            //Role

            if (!_roleManager.RoleExistsAsync(SD.SuperAdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.SuperAdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();



                //User

                _userManager.CreateAsync(new ApplicationUser

                { 
                    UserName="MostafaAd13",
                    Email = "Admin@gmail.com",
                    PhoneNumber="012345678",
                    EmailConfirmed = true
                },"ADMin123@@").GetAwaiter().GetResult();

                ApplicationUser user =_context.ApplicationUsers.FirstOrDefault(u=>u.Email == "Admin@gmail.com");

                _userManager.AddToRoleAsync(user, SD.SuperAdminRole).GetAwaiter().GetResult();

            }



            return;

        }

    }


}
