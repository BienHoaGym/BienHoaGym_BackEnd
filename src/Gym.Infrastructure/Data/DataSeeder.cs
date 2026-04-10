using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace Gym.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedDemoDataAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<GymDbContext>();
        var logger = serviceProvider.GetRequiredService<ILogger<GymDbContext>>();

        try
        {
            var today = DateTime.Today;

            // 1. Providers
            if (!await context.Providers.AnyAsync())
            {
                context.Providers.AddRange(new List<Provider> {
                    new Provider { Name = "C\u00F4ng ty Thi\u1EBFt b\u1ECB Gym To\u00E0n C\u1EA7u", ContactPerson = "Nguy\u1EC5n V\u0103n A", Email = "contact@globalgym.com", PhoneNumber = "0901234567", Address = "TP.HCM", IsActive = true },
                    new Provider { Name = "T\u1ED5ng kho Dinh d\u01B0\u1EE1ng Vi\u1EC7t", ContactPerson = "Tr\u1EA7n Th\u1ECB B", Email = "sales@dinhduongviet.vn", PhoneNumber = "0908889999", Address = "H\u00E0 N\u1ED9i", IsActive = true }
                });
                await context.SaveChangesAsync();
            }

            // 2. Equipment Categories
            if (!await context.EquipmentCategories.AnyAsync())
            {
                context.EquipmentCategories.AddRange(new List<EquipmentCategory> {
                    new EquipmentCategory { Name = "M\u00E1y chá\u1EA1y b\u1ED9", Description = "C\u00E1c lo\u1EA1i m\u00E1y chá\u1EA1y b\u1ED9 \u0111i\u1EC7n" },
                    new EquipmentCategory { Name = "M\u00E1y t\u1EADp c\u01A1 ng\u1EF1c", Description = "Thi\u1EBFt b\u1ECB t\u1EADp luy\u1EC7n c\u01A1 ng\u1EF1c" }
                });
                await context.SaveChangesAsync();
            }

            // 3. Packages
            if (!await context.MembershipPackages.AnyAsync())
            {
                context.MembershipPackages.AddRange(new List<MembershipPackage>{
                    new MembershipPackage { Name = "G\u00F3i Demo 1 Th\u00E1ng", DurationInMonths = 1, DurationDays = 30, Price = 500000, IsActive = true },
                    new MembershipPackage { Name = "G\u00F3i Demo 1 N\u0103m", DurationInMonths = 12, DurationDays = 365, Price = 4500000, IsActive = true }
                });
                await context.SaveChangesAsync();
            }

            logger.LogInformation("Demo data seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during automatic seeding");
        }
    }

    public static async Task SeedDefaultAdminAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<GymDbContext>();
        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<User>>();
        
        // 1. Ensure Admin Role exists
        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
        if (adminRole == null)
        {
            adminRole = new Role 
            { 
                RoleName = "Admin", 
                Description = "Administrator", 
                Permissions = "[\"All\"]" 
            };
            context.Roles.Add(adminRole);
            await context.SaveChangesAsync();
        }

        // 2. Ensure Admin User exists
        var adminUser = await context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Username == "admin");
        if (adminUser == null)
        {
            adminUser = new User 
            { 
                Id = Guid.NewGuid(), 
                Username = "admin", 
                Email = "admin@bienhoagym.com", 
                FullName = "Administrator", 
                IsActive = true 
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "123456");
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }

        // 3. Ensure User holds the Admin Role
        if (!adminUser.UserRoles.Any(ur => ur.RoleId == adminRole.Id))
        {
            // Adding to the navigation collection directly is safer when DbSet<UserRole> is not exposed
            adminUser.UserRoles.Add(new UserRole 
            { 
                UserId = adminUser.Id, 
                RoleId = adminRole.Id 
            });
            await context.SaveChangesAsync();
        }
    }
}