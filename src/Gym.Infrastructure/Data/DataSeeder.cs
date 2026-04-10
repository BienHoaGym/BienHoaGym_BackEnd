using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
                    new Provider { Name = "Công ty Thiết bị Gym Toàn Cầu", ContactPerson = "Nguyễn Văn A", Email = "contact@globalgym.com", PhoneNumber = "0901234567", Address = "TP.HCM", IsActive = true },
                    new Provider { Name = "Tổng kho Dinh dưỡng Việt", ContactPerson = "Trần Thị B", Email = "sales@dinhduongviet.vn", PhoneNumber = "0908889999", Address = "Hà Nội", IsActive = true }
                });
                await context.SaveChangesAsync();
            }
            var providers = await context.Providers.ToListAsync();

            // 2. Equipment Categories
            if (!await context.EquipmentCategories.AnyAsync())
            {
                context.EquipmentCategories.AddRange(new List<EquipmentCategory> {
                    new EquipmentCategory { Name = "Máy chạy bộ", Description = "Các loại máy chạy bộ điện" },
                    new EquipmentCategory { Name = "Máy tập cơ ngực", Description = "Thiết bị tập luyện cơ ngực" },
                    new EquipmentCategory { Name = "Tạ tay & Tạ đòn", Description = "Dụng cụ tập tạ" }
                });
                await context.SaveChangesAsync();
            }
            var categories = await context.EquipmentCategories.ToListAsync();

            // 3. Equipments
            if (!await context.Equipments.AnyAsync())
            {
                for (int i = 1; i <= 10; i++)
                {
                    context.Equipments.Add(new Equipment
                    {
                        Name = $"Máy chạy bộ Matrix v{i}",
                        EquipmentCode = $"MTX-{100 + i}",
                        CategoryId = categories[0].Id,
                        ProviderId = providers[0].Id,
                        Status = i == 5 ? EquipmentStatus.Maintenance : EquipmentStatus.Active,
                        PurchaseDate = today.AddYears(-1),
                        PurchasePrice = 20000000
                    });
                }
                await context.SaveChangesAsync();
            }

            // 4. Products
            if (!await context.Products.AnyAsync())
            {
                context.Products.AddRange(new List<Product> {
                    new Product { Name = "Whey Protein Gold", SKU = "WHEY-01", Price = 1500000, CostPrice = 1100000, Category = "Thực phẩm bổ sung", Unit = "Hộp", StockQuantity = 25, IsActive = true, ProviderId = providers[1].Id },
                    new Product { Name = "Nước suối Aquafina", SKU = "WATER-01", Price = 10000, CostPrice = 5000, Category = "Đồ uống", Unit = "Chai", StockQuantity = 150, IsActive = true, ProviderId = providers[1].Id },
                    new Product { Name = "Găng tay tập Gym", SKU = "GLV-01", Price = 250000, CostPrice = 150000, Category = "Dụng cụ", Unit = "Đôi", StockQuantity = 15, IsActive = true, ProviderId = providers[0].Id },
                    new Product { Name = "Khăn lau mồ hôi", SKU = "TWL-01", Price = 50000, CostPrice = 25000, Category = "Dụng cụ", Unit = "Cái", StockQuantity = 40, IsActive = true, ProviderId = providers[0].Id }
                });
                await context.SaveChangesAsync();
            }

            // 5. Warehouses
            if (!await context.Warehouses.AnyAsync())
            {
                var w1 = new Warehouse { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Kho Tổng", IsActive = true };
                var w2 = new Warehouse { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Name = "Kho Quầy", IsActive = true };
                context.Warehouses.AddRange(w1, w2);
                await context.SaveChangesAsync();
            }

            // 6. Trainers
            if (!await context.Trainers.AnyAsync(t => t.FullName == "Nguyễn PT Demo"))
            {
                var trainer1 = new Trainer { FullName = "Nguyễn PT Demo", Specialization = "Bodybuilding", IsActive = true, HireDate = today.AddMonths(-12) };
                var trainer2 = new Trainer { FullName = "Trần Yoga", Specialization = "Yoga", IsActive = true, HireDate = today.AddMonths(-6) };
                context.Trainers.AddRange(trainer1, trainer2);
                await context.SaveChangesAsync();
            }

            // 7. Packages
            if (!await context.MembershipPackages.AnyAsync(p => p.Name == "Gói Demo 1 Tháng"))
            {
                context.MembershipPackages.AddRange(new List<MembershipPackage>{
                    new MembershipPackage { Name = "Gói Demo 1 Tháng", DurationInMonths = 1, DurationDays = 30, Price = 500000, IsActive = true },
                    new MembershipPackage { Name = "Gói Demo 3 Tháng", DurationInMonths = 3, DurationDays = 90, Price = 1350000, IsActive = true },
                    new MembershipPackage { Name = "Gói Demo 1 Năm", DurationInMonths = 12, DurationDays = 365, Price = 4500000, IsActive = true }
                });
                await context.SaveChangesAsync();
            }
            var packages = await context.MembershipPackages.ToListAsync();

            // 8. Members & Subscriptions
            string[] names = { "Nguyễn Văn Hùng", "Trần Thị Lan", "Lê Minh Tâm", "Phạm Hoàng Nam", "Hoàng Thu Thủy", "Đặng Văn Bình", "Vũ Huy Hoàng", "Đỗ Mỹ Linh", "Bùi Anh Tuấn", "Ngô Phương Anh" };
            
            if (!await context.Members.AnyAsync())
            {
                for (int i = 0; i < names.Length; i++)
                {
                    var code = $"MEM{1000 + i}";
                    var member = new Member
                    {
                        FullName = names[i],
                        MemberCode = code,
                        PhoneNumber = $"0912000{100 + i}",
                        Email = $"member{i}@example.com",
                        JoinedDate = today.AddMonths(-3),
                        Status = i == 8 ? MemberStatus.Suspended : (i == 9 ? MemberStatus.Inactive : MemberStatus.Active)
                    };
                    context.Members.Add(member);
                    await context.SaveChangesAsync();

                    var subStatus = SubscriptionStatus.Active;
                    var startDate = today.AddDays(-20);
                    var endDate = today.AddDays(10);

                    if (i == 0) { startDate = today.AddDays(-29); endDate = today.AddDays(1); } 
                    else if (i == 1) { subStatus = SubscriptionStatus.Pending; startDate = today; endDate = today.AddDays(30); }

                    var pkg = packages[i % packages.Count];
                    var sub = new MemberSubscription
                    {
                        MemberId = member.Id,
                        PackageId = pkg.Id,
                        StartDate = startDate,
                        EndDate = subStatus == SubscriptionStatus.Pending ? startDate.AddDays(pkg.DurationDays) : endDate,
                        Status = subStatus,
                        OriginalPrice = pkg.Price,
                        FinalPrice = pkg.Price,
                        OriginalPackageName = pkg.Name
                    };
                    context.MemberSubscriptions.Add(sub);
                    await context.SaveChangesAsync();

                    if (subStatus == SubscriptionStatus.Active)
                    {
                        context.Payments.Add(new Payment {
                            MemberSubscriptionId = sub.Id,
                            Amount = pkg.Price,
                            PaymentDate = startDate,
                            Status = PaymentStatus.Completed,
                            Method = PaymentMethod.Cash
                        });
                    }
                }
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