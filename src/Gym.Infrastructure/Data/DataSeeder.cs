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
                    new MembershipPackage { Name = "G\u00F3i Standard 1 Th\u00E1ng", DurationInMonths = 1, DurationDays = 30, Price = 500000, IsActive = true, CreatedAt = now },
                    new MembershipPackage { Name = "G\u00F3i Premium 6 Th\u00E1ng", DurationInMonths = 6, DurationDays = 180, Price = 2500000, IsActive = true, CreatedAt = now },
                    new MembershipPackage { Name = "G\u00F3i VIP 1 N\u0103m", DurationInMonths = 12, DurationDays = 365, Price = 4500000, IsActive = true, CreatedAt = now }
                });
                await context.SaveChangesAsync();
            }

            // 3. Members & Subscriptions & Payments (Trang doanh thu l\u1EA5y t\u1EEB \u0111\u00E2y)
            if (!await context.Invoices.AnyAsync())
            {
                var packages = await context.MembershipPackages.ToListAsync();
                var random = new Random();

                for (int i = 1; i <= 20; i++)
                {
                    var joinedDate = today.AddDays(-random.Next(1, 60));
                    var member = new Member
                    {
                        FullName = $"Th\u00E0nh vi\u00EAn Demo {i}",
                        Email = $"member{i}@demo.com",
                        PhoneNumber = $"09123456{i:D2}",
                        JoinedDate = joinedDate,
                        IsActive = true,
                        CreatedAt = joinedDate
                    };
                    context.Members.Add(member);
                    await context.SaveChangesAsync();

                    // M\u1ED7i member mua 1 g\u00F3i
                    var pkg = packages[random.Next(packages.Count)];
                    var sub = new MemberSubscription
                    {
                        MemberId = member.Id,
                        PackageId = pkg.Id,
                        StartDate = joinedDate,
                        EndDate = joinedDate.AddDays(pkg.DurationDays),
                        Price = pkg.Price,
                        Status = SubscriptionStatus.Active,
                        CreatedAt = joinedDate
                    };
                    context.Subscriptions.Add(sub);
                    await context.SaveChangesAsync();

                    // T\u1EA1o thanh to\u00E1n (Quan tr\u1ECDng cho b\u00E1o c\u00E1o)
                    var payment = new Payment
                    {
                        SubscriptionId = sub.Id,
                        Amount = pkg.Price,
                        PaymentDate = joinedDate,
                        PaymentMethod = random.Next(2) == 0 ? "Ti\u1EC1n m\u1EB7t" : "Chuy\u1EC3n kho\u1EA3n",
                        Status = PaymentStatus.Completed,
                        CreatedAt = joinedDate
                    };
                    context.Payments.Add(payment);

                    // T\u1EA1o h\u00F3a \u0111\u01A1n (Quan tr\u1ECDng cho b\u00E1o c\u00E1o)
                    var invoice = new Invoice
                    {
                        MemberId = member.Id,
                        InvoiceNumber = $"INV-{joinedDate:yyyyMMdd}-{i:D3}",
                        TotalAmount = pkg.Price,
                        TaxAmount = pkg.Price * 0.1m,
                        DiscountAmount = 0,
                        FinalAmount = pkg.Price * 1.1m,
                        Status = PaymentStatus.Completed,
                        CreatedAt = joinedDate,
                        PaymentMethod = payment.PaymentMethod
                    };
                    context.Invoices.Add(invoice);
                    await context.SaveChangesAsync();
                    
                    // Th\u00EAm chi ti\u1EBFt h\u00F3a \u0111\u01A1n
                    context.InvoiceDetails.Add(new InvoiceDetail {
                        InvoiceId = invoice.Id,
                        ItemName = pkg.Name,
                        ItemType = "Package",
                        Quantity = 1,
                        UnitPrice = pkg.Price,
                        TotalPrice = pkg.Price,
                        SubscriptionId = sub.Id
                    });
                }
                
                // Th\u00EAm m\u1ED9t v\u00E0i h\u00F3a \u0111\u01A1n cho ng\u00E0y h\u00F4m nay \u0111\u1EC3 th\u1EA5y doanh thu h\u00F4m nay
                for (int j = 1; j <= 5; j++) {
                    var hour = 8 + (j * 2); 
                    var transTime = today.AddHours(hour);
                    var invId = Guid.NewGuid();
                    var inv = new Invoice {
                        Id = invId,
                        MemberId = null, // Kh\u00E1ch l\u1EBB
                        InvoiceNumber = $"POS-{today:yyyyMMdd}-{j:D3}",
                        TotalAmount = 50000,
                        FinalAmount = 50000,
                        Status = PaymentStatus.Completed,
                        CreatedAt = transTime,
                        PaymentMethod = "Cash"
                    };
                    context.Invoices.Add(inv);
                    context.InvoiceDetails.Add(new InvoiceDetail {
                        InvoiceId = invId,
                        ItemName = "N\u01B0\u1EDBc su\u1ED1i Lavie",
                        ItemType = "Product",
                        Quantity = 2,
                        UnitPrice = 25000,
                        TotalPrice = 50000
                    });
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