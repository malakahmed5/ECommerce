using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModuleEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.IdentityData.DataSeed
{
    public class IdentityDataInitializer : IDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataInitializer> _logger;

        public IdentityDataInitializer(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager ,ILogger<IdentityDataInitializer> logger)
        {
           _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task InitializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                    await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
                    await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
                }
                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser()
                    {
                        UserName = "MalakAhmed",
                        DisplayName = "Malak Ahmed",
                        Email = "malaakahmed.xx@gmail.com",
                        PhoneNumber = "01123456789",
                    };
                    var user02 = new ApplicationUser()
                    {
                        UserName = "MohamedAhmed",
                        DisplayName = "Mohamed Ahmed",
                        Email = "mohamed@gmail.com",
                        PhoneNumber = "01123456789",
                    };
                    await _userManager.CreateAsync(user01,"P@ssw0rd");
                    await _userManager.CreateAsync(user02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(user01, "SuperAdmin");
                    await _userManager.AddToRoleAsync(user02, "Admin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured When Try To Seeding In Database.Security");
            }
        }
    }
}
