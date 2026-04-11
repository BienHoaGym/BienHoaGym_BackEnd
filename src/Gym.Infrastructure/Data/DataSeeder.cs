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
            var now = DateTime.UtcNow;
            var today = now.Date;

            // 1. Providers
            if (!await context.Providers.AnyAsync())
            {
                context.Providers.AddRange(new List<Provider> {
                    new Provider { Name = "C\u00F4ng ty Thi\u1EBFt b\u1ECB Gym To\u00E0n C\u1EA7u", ContactPerson = "Nguy\u1EC5n V\u0103n A", Email = "contact@globalgym.com", PhoneNumber = "0901234567", Address = "TP.HCM", IsActive = true, CreatedAt = now },
                    new Provider { Name = "T\u1ED5ng kho Dinh d\u01B0\u1EE1ng Vi\u1EC7t", ContactPerson = "Tr\u1EA7n Th\u1ECB B", Email = "sales@dinhduongviet.vn", PhoneNumber = "0908889999", Address = "H\u00E0 N\u1ED9i", IsActive = true, CreatedAt = now }
                });
                await context.SaveChangesAsync();
            }

            // 2. Packages
            if (!await context.MembershipPackages.AnyAsync())
            {
                context.MembershipPackages.AddRange(new List<MembershipPackage>{
                    new MembershipPackage { Name = "G\u00F3i Standard 1 Th\u00E1ng", DurationInMonths = 1, DurationInDays = 30, Price = 500000, IsActive = true, CreatedAt = now },
                    new MembershipPackage { Name = "G\u00F3i Premium 6 Th\u00E1ng", DurationInMonths = 6, DurationInDays = 180, Price = 2500000, IsActive = true, CreatedAt = now },
                    new MembershipPackage { Name = "G\u00F3i VIP 1 N\u0103m", DurationInMonths = 12, DurationInDays = 365, Price = 4500000, IsActive = true, CreatedAt = now }
                });
                await context.SaveChangesAsync();
            }

            // 3. Members & Subscriptions & Payments
            var lastMonthInvoices = await context.Invoices.CountAsync(i => i.CreatedAt >= today.AddDays(-30));
            // Demo data seeding has been disabled due to MemberCode unique constraint conflicts
            if (false)
            {
                var packages = await context.MembershipPackages.ToListAsync();
                var random = new Random();

                for (int i = 1; i <= 100; i++)
                {
                    var joinedDate = today.AddDays(-random.Next(1, 180));
                    var member = new Member
                    {
                        FullName = $"Th\u00E0nh vi\u00EAn Demo {i}",
                        Email = $"member{i}@demo.com",
                        PhoneNumber = $"09123456{i:D2}",
                        JoinedDate = joinedDate,
                        Status = MemberStatus.Active, // Dng Status thay vA IsActive
                        CreatedAt = joinedDate
                    };
                    context.Members.Add(member);
                    await context.SaveChangesAsync();

                    var pkg = packages[random.Next(packages.Count)];
                    var sub = new MemberSubscription
                    {
                        MemberId = member.Id,
                        PackageId = pkg.Id,
                        StartDate = joinedDate,
                        EndDate = joinedDate.AddDays(pkg.DurationInDays),
                        OriginalPrice = pkg.Price, // Dng OriginalPrice/FinalPrice
                        FinalPrice = pkg.Price,
                        Status = SubscriptionStatus.Active,
                        CreatedAt = joinedDate,
                        OriginalPackageName = pkg.Name
                    };
                    context.MemberSubscriptions.Add(sub); // Dng MemberSubscriptions
                    await context.SaveChangesAsync();

                    var payment = new Payment
                    {
                        MemberSubscriptionId = sub.Id, // Dng MemberSubscriptionId
                        Amount = pkg.Price,
                        PaymentDate = joinedDate,
                        Method = random.Next(2) == 0 ? PaymentMethod.Cash : PaymentMethod.BankTransfer, // Dng Method vA Enum
                        Status = PaymentStatus.Completed,
                        CreatedAt = joinedDate
                    };
                    context.Payments.Add(payment);

                    var invoice = new Invoice
                    {
                        MemberId = member.Id,
                        InvoiceNumber = $"INV-{joinedDate:yyyyMMdd}-{i:D3}",
                        TotalAmount = pkg.Price,
                        DiscountAmount = 0, // FinalAmount t tAnh
                        Status = PaymentStatus.Completed,
                        CreatedAt = joinedDate,
                        PaymentMethod = payment.Method
                    };
                    context.Invoices.Add(invoice);
                    await context.SaveChangesAsync();
                    
                    context.InvoiceDetails.Add(new InvoiceDetail {
                        InvoiceId = invoice.Id,
                        ItemName = pkg.Name,
                        ItemType = "Package",
                        Quantity = 1,
                        UnitPrice = pkg.Price,
                        SubscriptionId = sub.Id
                    });
                }

                // 4. POS Orders (Bán lẻ)
                for (int i = 1; i <= 30; i++)
                {
                    var orderDate = today.AddDays(-random.Next(1, 90));
                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        OrderNumber = $"POS-{orderDate:yyyyMMdd}-{i:D3}",
                        TotalAmount = random.Next(50000, 500000),
                        CreatedDate = orderDate,
                        Status = "Completed",
                        IsDeleted = false
                    };
                    context.Orders.Add(order);
                }
                
                await context.SaveChangesAsync();
            }

            logger.LogInformation("✅ High-fidelity demo data seeding completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Error during demo data seeding");
        }
    }

    public static async Task SeedDefaultAdminAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<GymDbContext>();
        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<User>>();
        
        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
        if (adminRole == null)
        {
            adminRole = new Role { RoleName = "Admin", Description = "Administrator", Permissions = "[\"All\"]" };
            context.Roles.Add(adminRole);
            await context.SaveChangesAsync();
        }

        var adminUser = await context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Username == "admin");
        if (adminUser == null)
        {
            adminUser = new User { Id = Guid.NewGuid(), Username = "admin", Email = "admin@bienhoagym.com", FullName = "System Administrator", IsActive = true };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "123456");
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }

        if (!adminUser.UserRoles.Any(ur => ur.RoleId == adminRole.Id))
        {
            adminUser.UserRoles.Add(new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id });
            await context.SaveChangesAsync();
        }
    }
}