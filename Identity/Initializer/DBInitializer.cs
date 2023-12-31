﻿using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Initializer
{
    public class DBInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DBInitializer(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            await SeedRolesAsync();

            await SeedUser("admin@inveon.io", "$Passwr0d%", "Inveon", "Admin", Roles.ADMIN);

            await SeedUser("consumer@inveon.io", "$Passwr0d%", "Inveon", "Consumer", Roles.CONSUMER);

        }

        private async Task SeedRolesAsync()
        {
            var allRolesProperties = typeof(Roles).GetFields();

            foreach (var roleProp in allRolesProperties)
            {
                var role = roleProp.GetValue(allRolesProperties).ToString();

                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task<ApplicationUser> SeedUser(string username, string password, string firstName, string lastName, string roleName)
        {
            if (!_userManager.Users.Any(t => t.UserName == username))
            {
                var user = new ApplicationUser()
                {
                    UserName = username
                };

                user.SetFirstName(firstName);
                user.SetLastName(lastName);
                user.Email = username;
                user.EmailConfirmed = true;

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, roleName);

                return user;
            }
            else
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user != null && !await _userManager.IsInRoleAsync(user, roleName))
                    await _userManager.AddToRoleAsync(user, roleName);

                return user;
            }
        }
    }
}
