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
            var utcNow = DateTime.UtcNow;
            var today = DateTime.Today;
            var random = new Random();

            // 1. Providers
            if (!await context.Providers.AnyAsync())
            {
                context.Providers.AddRange(new List<Provider> {
                    new Provider { Name = "C\u00F4ng ty Thi\u1EBFt b\u1ECB Gym To\u00E0n C\u1EA7u", ContactPerson = "Nguy\u1EC5n V\u0103n A", Email = "contact@globalgym.com", PhoneNumber = "0901234567", Address = "TP.HCM", IsActive = true },
                    new Provider { Name = "T\u1ED5ng kho Dinh d\u01B0\u1EE1ng Vi\u1EC7t", ContactPerson = "Tr\u1EA7n Th\u1ECB B", Email = "sales@dinhduongviet.vn", PhoneNumber = "0908889999", Address = "H\u00E0 N\u1ED9i", IsActive = true }
                });
                await context.SaveChangesAsync();
            }
            var providers = await context.Providers.ToListAsync();

            // 2. Equipment Categories
            if (!await context.EquipmentCategories.AnyAsync())
            {
                context.EquipmentCategories.AddRange(new List<EquipmentCategory> {
                    new EquipmentCategory { Name = "M\u00E1y chá\u1EA1y b\u1ED9", Description = "C\u00E1c lo\u1EA1i m\u00E1y chá\u1EA1y b\u1ED9 \u0111i\u1EC7n" },
                    new EquipmentCategory { Name = "M\u00E1y t\u1EADp c\u01A1 ng\u1EF1c", Description = "Thi\u1EBFt b\u1ECB t\u1EADp luy\u1EC7n c\u01A1 ng\u1EF1c" },
                    new EquipmentCategory { Name = "T\u1EA1 tay & T\u1EA1 \u0111\u00F2n", Description = "D\u1EE5ng c\u1EE5 t\u1EADp t\u1EA1" }
                });
                await context.SaveChangesAsync();
            }
            var categories = await context.EquipmentCategories.ToListAsync();

            // 4. Products
            if (!await context.Products.AnyAsync())
            {
                context.Products.AddRange(new List<Product> {
                    new Product { Name = "Whey Protein Gold", SKU = "WHEY-01", Price = 1500000, CostPrice = 1100000, Category = "Th\u1EF1c ph\u1EA9m b\u1ED5 sung", Unit = "H\u1ED9p", StockQuantity = 25, IsActive = true, ProviderId = providers[1].Id },
                    new Product { Name = "N\u01B0\u1EDBc su\u1ED1i Aquafina", SKU = "WATER-01", Price = 10000, CostPrice = 5000, Category = "\u0110\u1ED3 u\u1ED1ng", Unit = "Chai", StockQuantity = 150, IsActive = true, ProviderId = providers[1].Id }
                });
                await context.SaveChangesAsync();
            }

            // 5. Warehouses
            if (!await context.Warehouses.AnyAsync())
            {
                context.Warehouses.AddRange(
                    new Warehouse { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Kho T\u1ED5ng", IsActive = true },
                    new Warehouse { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Name = "Kho Qu\u1EADDy", IsActive = true }
                );
                await context.SaveChangesAsync();
            }

            // 7. Packages
            if (!await context.MembershipPackages.AnyAsync())
            {
                context.MembershipPackages.AddRange(new List<MembershipPackage>{
                    new MembershipPackage { Name = "G\u00F3i Demo 1 Th\u00E1ng", DurationInMonths = 1, DurationDays = 30, Price = 500000, IsActive = true },
                    new MembershipPackage { Name = "G\u00F3i Demo 1 N\u0103m", DurationInMonths = 12, DurationDays = 365, Price = 4500000, IsActive = true }
                });
                await context.SaveChangesAsync();
            }

            await context.SaveChangesAsync();
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
        
        // 1. Ensure Admin Role
        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
        if (adminRole == null)
        {
            adminRole = new Role { 
                Id = Guid.NewGuid(), 
                RoleName = "Admin", 
                Description = "Administrator", 
                Permissions = "[\"All\"]" 
            };
            context.Roles.Add(adminRole);
            await context.SaveChangesAsync();
        }

        // 2. Ensure Admin User
        var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
        if (adminUser == null)
        {
            adminUser = new User { 
                Id = Guid.NewGuid(), 
                Username = "admin", 
                Email = "admin@bienhoagym.com", 
                FullName = "Administrator", 
                IsActive = true 
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "123456");
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            // Link Role
            context.UserRoles.Add(new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id });
            await context.SaveChangesAsync();
        }
    }
}