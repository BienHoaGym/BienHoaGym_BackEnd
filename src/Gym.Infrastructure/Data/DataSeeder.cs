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
            var random = new Random();

            // 1. Providers
            if (!await context.Providers.AnyAsync())
            {
                context.Providers.AddRange(new List<Provider> {
                    new Provider { Id = Guid.NewGuid(), Name = "GymX Supplier", ContactPerson = "Nguyễn Văn A", Email = "a@gymx.com", PhoneNumber = "0901234567", SupplyType = "Máy móc gym", IsActive = true, CreatedAt = utcNow },
                    new Provider { Id = Guid.NewGuid(), Name = "Nutrition Pro", ContactPerson = "Trần Thị B", Email = "b@nutrition.com", PhoneNumber = "0907654321", SupplyType = "Thực phẩm bổ sung", IsActive = true, CreatedAt = utcNow },
                    new Provider { Id = Guid.NewGuid(), Name = "TechGym Electronics", ContactPerson = "Lê Văn C", Email = "c@techgym.com", PhoneNumber = "0901122334", SupplyType = "Điện tử/Thông minh", IsActive = true, CreatedAt = utcNow }
                });
                await context.SaveChangesAsync();
            }
            var providers = await context.Providers.ToListAsync();

            // 2. Equipment Categories
            if (!await context.EquipmentCategories.AnyAsync())
            {
                context.EquipmentCategories.AddRange(new List<EquipmentCategory> {
                    new EquipmentCategory { Id = Guid.NewGuid(), Name = "Máy Chạy Bộ", Code = "CARDIO", Group = "Tim mạch (Cardio)", CreatedAt = utcNow },
                    new EquipmentCategory { Id = Guid.NewGuid(), Name = "Máy Khối", Code = "STRENGTH", Group = "Sức mạnh/Cơ bắp", CreatedAt = utcNow },
                    new EquipmentCategory { Id = Guid.NewGuid(), Name = "Tạ Đơn/Tạ Đòn", Code = "FREE", Group = "Tạ tự do", CreatedAt = utcNow }
                });
                await context.SaveChangesAsync();
            }
            var categories = await context.EquipmentCategories.ToListAsync();

            // 3. Equipment
            if (!await context.Equipments.AnyAsync())
            {
                for (int i = 1; i <= 5; i++)
                {
                    context.Equipments.Add(new Equipment { 
                        Name = $"Máy chạy Matrix T{i}", 
                        EquipmentCode = $"MTX-{100+i}", 
                        CategoryId = categories[0].Id, 
                        ProviderId = providers[0].Id,
                        Status = i == 5 ? EquipmentStatus.Maintenance : EquipmentStatus.Active,
                        PurchaseDate = today.AddYears(-1),
                        PurchasePrice = 20000000,
                        CreatedAt = utcNow
                    });
                }
                await context.SaveChangesAsync();
            }

            // 4. Products
            if (!await context.Products.AnyAsync())
            {
                context.Products.AddRange(new List<Product> {
                    new Product { Name = "Whey Protein Gold", SKU = "WHEY-01", Price = 1500000, CostPrice = 1100000, Category = "Thực phẩm bổ sung", Unit = "Hộp", StockQuantity = 25, IsActive = true, CreatedAt = utcNow, ProviderId = providers[1].Id },
                    new Product { Name = "Nước suối Aquafina", SKU = "WATER-01", Price = 10000, CostPrice = 5000, Category = "Đồ uống", Unit = "Chai", StockQuantity = 150, IsActive = true, CreatedAt = utcNow, ProviderId = providers[1].Id },
                    new Product { Name = "Găng tay tập Gym", SKU = "GLV-01", Price = 250000, CostPrice = 150000, Category = "Dụng cụ", Unit = "Đôi", StockQuantity = 15, IsActive = true, CreatedAt = utcNow, ProviderId = providers[0].Id },
                    new Product { Name = "Khăn lau mồ hôi", SKU = "TWL-01", Price = 50000, CostPrice = 25000, Category = "Dụng cụ", Unit = "Cái", StockQuantity = 40, IsActive = true, CreatedAt = utcNow, ProviderId = providers[0].Id }
                });
                await context.SaveChangesAsync();
            }

            // 5. Warehouses & Initial Stock
            if (!await context.Warehouses.AnyAsync())
            {
                var w1 = new Warehouse { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Kho Tổng", IsActive = true, CreatedAt = utcNow };
                var w2 = new Warehouse { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Name = "Kho Quầy", IsActive = true, CreatedAt = utcNow };
                context.Warehouses.AddRange(w1, w2);
                await context.SaveChangesAsync();
            }

            // 6. Trainers
            if (!await context.Trainers.AnyAsync(t => t.FullName == "Nguyễn PT Demo"))
            {
                var trainer1 = new Trainer { FullName = "Nguyễn PT Demo", Specialization = "Bodybuilding", IsActive = true, HireDate = today.AddMonths(-12), CreatedAt = utcNow };
                var trainer2 = new Trainer { FullName = "Trần Yoga", Specialization = "Yoga", IsActive = true, HireDate = today.AddMonths(-6), CreatedAt = utcNow };
                context.Trainers.AddRange(trainer1, trainer2);
                await context.SaveChangesAsync();
            }
            var t1 = await context.Trainers.FirstAsync();

            // 7. Packages
            if (!await context.MembershipPackages.AnyAsync(p => p.Name == "Gói Demo 1 Tháng"))
            {
                context.MembershipPackages.AddRange(new List<MembershipPackage>{
                    new MembershipPackage { Name = "Gói Demo 1 Tháng", DurationInMonths = 1, DurationDays = 30, Price = 500000, IsActive = true, CreatedAt = utcNow },
                    new MembershipPackage { Name = "Gói Demo 3 Tháng", DurationInMonths = 3, DurationDays = 90, Price = 1350000, IsActive = true, CreatedAt = utcNow },
                    new MembershipPackage { Name = "Gói Demo 1 Năm", DurationInMonths = 12, DurationDays = 365, Price = 4500000, IsActive = true, CreatedAt = utcNow }
                });
                await context.SaveChangesAsync();
            }
            var packages = await context.MembershipPackages.ToListAsync();

            // 8. Members & Subscriptions
            string[] names = { "Nguyễn Văn Hùng", "Trần Thị Lan", "Lê Minh Tâm", "Phạm Hoàng Nam", "Hoàng Thu Thủy", "Đặng Văn Bình", "Vũ Huy Hoàng", "Đỗ Mỹ Linh", "Bùi Anh Tuấn", "Ngô Phương Anh" };
            
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
                    Status = i == 8 ? MemberStatus.Suspended : (i == 9 ? MemberStatus.Inactive : MemberStatus.Active),
                    CreatedAt = utcNow
                };
                context.Members.Add(member);
                await context.SaveChangesAsync();

                // Tạo kịch bản khác nhau cho các hội viên đầu tiên
                SubscriptionStatus subStatus = SubscriptionStatus.Active;
                DateTime startDate = today.AddDays(-20);
                DateTime endDate = today.AddDays(10);

                if (i == 0) { // Sắp hết hạn (expires in 1 day)
                    startDate = today.AddDays(-29);
                    endDate = today.AddDays(1);
                } 
                else if (i == 1) { // Gói đang chờ thanh toán (Pending)
                    subStatus = SubscriptionStatus.Pending;
                    startDate = today;
                    endDate = today.AddDays(30);
                }
                else if (i == 8) { // Đang bị Suspend
                    subStatus = SubscriptionStatus.Suspended;
                }
                else if (i == 9) { // Đã Hủy
                    subStatus = SubscriptionStatus.Cancelled;
                }

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
                    OriginalPackageName = pkg.Name,
                    CreatedAt = utcNow
                };
                context.MemberSubscriptions.Add(sub);
                await context.SaveChangesAsync();

                // Ghi nhận thanh toán nếu là Active/Suspended
                if (subStatus == SubscriptionStatus.Active || subStatus == SubscriptionStatus.Suspended)
                {
                    context.Payments.Add(new Payment {
                        MemberSubscriptionId = sub.Id,
                        Amount = pkg.Price,
                        PaymentDate = startDate,
                        Status = PaymentStatus.Completed,
                        Method = PaymentMethod.Cash,
                        CreatedAt = startDate
                    });

                    // Add some check-ins for active members
                    for (int d = 0; d < 10; d++)
                    {
                        context.CheckIns.Add(new CheckIn {
                            MemberId = member.Id,
                            SubscriptionId = sub.Id,
                            CheckInTime = today.AddDays(-d).AddHours(random.Next(7, 21)),
                            CheckOutTime = today.AddDays(-d).AddHours(22),
                            CreatedAt = utcNow
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
