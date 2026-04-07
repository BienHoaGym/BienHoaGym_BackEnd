using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gym.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedDemoDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GymDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<GymDbContext>>();

        // Check if seeding is needed (e.g., if there are no members besides the minimal seeds)
        if (await context.Members.CountAsync() > 10)
        {
            return;
        }

        logger.LogInformation("Starting automatic demo data seeding for Docker/Development...");

        try
        {
            var utcNow = DateTime.UtcNow;
            var today = DateTime.SpecifyKind(utcNow.Date, DateTimeKind.Utc);

            // 1. Check for Trainers
            if (!await context.Trainers.AnyAsync(t => t.FullName == "Nguyễn PT Demo"))
            {
                var trainer1 = new Trainer { FullName = "Nguyễn PT Demo", Specialization = "Bodybuilding", IsActive = true, HireDate = today.AddMonths(-12), CreatedAt = utcNow };
                var trainer2 = new Trainer { FullName = "Lê Yoga Demo", Specialization = "Yoga & Pilates", IsActive = true, HireDate = today.AddMonths(-6), CreatedAt = utcNow };
                context.Trainers.AddRange(trainer1, trainer2);
                await context.SaveChangesAsync();
            }

            var t1 = await context.Trainers.FirstAsync(t => t.FullName == "Nguyễn PT Demo");
            var t2 = await context.Trainers.FirstAsync(t => t.FullName == "Lê Yoga Demo");

            // 2. Packages
            if (!await context.MembershipPackages.AnyAsync(p => p.Name == "Gói Demo 1 Tháng"))
            {
                var pkg1 = new MembershipPackage { Name = "Gói Demo 1 Tháng", DurationInMonths = 1, DurationDays = 30, Price = 500000, IsActive = true, CreatedAt = utcNow };
                var pkg2 = new MembershipPackage { Name = "Gói Demo 1 Năm", DurationInMonths = 12, DurationDays = 365, Price = 4500000, IsActive = true, CreatedAt = utcNow };
                context.MembershipPackages.AddRange(pkg1, pkg2);
                await context.SaveChangesAsync();
            }

            var p1 = await context.MembershipPackages.FirstAsync(p => p.Name == "Gói Demo 1 Tháng");
            var p2 = await context.MembershipPackages.FirstAsync(p => p.Name == "Gói Demo 1 Năm");

            // 3. Members, Subscriptions, Payments, Check-ins
            var random = new Random();
            for (int i = 1; i <= 15; i++)
            {
                var code = $"DEMO{100 + i}";
                if (await context.Members.AnyAsync(m => m.MemberCode == code)) continue;

                var member = new Member
                {
                    FullName = $"Hội viên Demo {i}",
                    MemberCode = code,
                    PhoneNumber = $"0988000{100 + i}",
                    Email = $"demo{i}@example.com",
                    JoinedDate = today.AddDays(-i * 3),
                    Status = MemberStatus.Active,
                    CreatedAt = utcNow
                };
                context.Members.Add(member);
                await context.SaveChangesAsync();

                // Add Subscription
                var pkg = i % 2 == 0 ? p1 : p2;
                var sub = new MemberSubscription
                {
                    MemberId = member.Id,
                    PackageId = pkg.Id,
                    StartDate = today.AddMonths(-1),
                    EndDate = today.AddMonths(pkg.DurationInMonths),
                    Status = SubscriptionStatus.Active,
                    OriginalPrice = pkg.Price,
                    FinalPrice = pkg.Price,
                    OriginalPackageName = pkg.Name,
                    CreatedAt = utcNow
                };
                context.MemberSubscriptions.Add(sub);
                await context.SaveChangesAsync();

                // Add Payment (spread over last few months)
                var payDate = today.AddMonths(-random.Next(0, 3)).AddDays(random.Next(1, 28));
                context.Payments.Add(new Payment
                {
                    MemberSubscriptionId = sub.Id,
                    Amount = pkg.Price,
                    PaymentDate = payDate,
                    Status = PaymentStatus.Completed,
                    Method = (PaymentMethod)random.Next(1, 4),
                    CreatedAt = payDate
                });

                // Add some check-ins
                for (int d = 0; d < 5; d++)
                {
                    context.CheckIns.Add(new CheckIn
                    {
                        MemberId = member.Id,
                        SubscriptionId = sub.Id,
                        CheckInTime = today.AddDays(-d).AddHours(random.Next(8, 20)),
                        CheckOutTime = today.AddDays(-d).AddHours(22),
                        CreatedAt = utcNow
                    });
                }
            }

            // 4. Equipment
            if (!await context.Equipments.AnyAsync(e => e.Name.Contains("Matrix Demo")))
            {
                context.Equipments.Add(new Equipment { Name = "Máy chạy Matrix Demo", Status = EquipmentStatus.Active, CreatedAt = utcNow, PurchaseDate = today.AddYears(-1) });
                context.Equipments.Add(new Equipment { Name = "Dàn tạ Demo", Status = EquipmentStatus.Maintenance, NextMaintenanceDate = today.AddDays(-1), CreatedAt = utcNow, PurchaseDate = today.AddMonths(-6) });
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Demo data seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during automatic seeding");
        }
    }
}
