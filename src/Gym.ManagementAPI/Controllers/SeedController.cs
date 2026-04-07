using Gym.Application.Interfaces;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gym.ManagementAPI.Controllers;

[ApiController]
[Route("api/seed")]
public class SeedController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public SeedController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("dashboard-demo")]
    [AllowAnonymous]
    public async Task<IActionResult> SeedDashboardData()
    {
        try
        {
            var utcNow = DateTime.UtcNow;
            var today = DateTime.SpecifyKind(utcNow.Date, DateTimeKind.Utc); // Ép kiểu UTC cho Postgres
            
            // 2. Trainers
            var trainer1 = new Trainer { FullName = "Nguyễn Văn A", Specialization = "Bodybuilding", IsActive = true, HireDate = today.AddMonths(-12) };
            var trainer2 = new Trainer { FullName = "Trần Thị B", Specialization = "Yoga & Pilates", IsActive = true, HireDate = today.AddMonths(-6) };
            
            await _unitOfWork.Trainers.AddAsync(trainer1);
            await _unitOfWork.Trainers.AddAsync(trainer2);
            await _unitOfWork.SaveChangesAsync();

            // 3. Packages
            var pkg1 = new MembershipPackage { Name = "Gói Tháng Cơ Bản", DurationInMonths = 1, DurationDays = 30, Price = 500000, IsActive = true };
            var pkg2 = new MembershipPackage { Name = "Gói Năm (Ưu đãi)", DurationInMonths = 12, DurationDays = 365, Price = 4500000, IsActive = true };
            var pkg3 = new MembershipPackage { Name = "Gói PT 10 Buổi", DurationInMonths = 3, DurationDays = 90, Price = 2500000, IsActive = true, HasPT = true };
            
            await _unitOfWork.Packages.AddAsync(pkg1);
            await _unitOfWork.Packages.AddAsync(pkg2);
            await _unitOfWork.Packages.AddAsync(pkg3);
            await _unitOfWork.SaveChangesAsync();

            // 4. Members
            var membersList = new List<Member>();
            for (int i = 1; i <= 20; i++)
            {
                var code = $"M{1000 + i}";
                var exists = await _unitOfWork.Members.GetQueryable().AnyAsync(m => m.MemberCode == code);
                if (exists) continue;

                var m = new Member
                {
                    FullName = $"Hội viên Demo {i}",
                    MemberCode = code,
                    PhoneNumber = $"090000{100 + i:D3}",
                    Email = $"demo{i}@gym.vn",
                    JoinedDate = today.AddDays(-i * 5),
                    Status = i % 5 == 0 ? MemberStatus.Inactive : MemberStatus.Active
                };
                membersList.Add(m);
            }
            foreach (var mem in membersList) await _unitOfWork.Members.AddAsync(mem);
            await _unitOfWork.SaveChangesAsync();
            
            // Re-fetch all demo members if some already existed for the rest of seeding logic
            var allDemoMembers = await _unitOfWork.Members.GetQueryable()
                .Where(m => m.MemberCode.StartsWith("M10"))
                .ToListAsync();

            // 5. Subscriptions & Payments (Historical for Charts)
            var random = new Random();
            for (int i = 0; i < allDemoMembers.Count; i++)
            {
                var m = allDemoMembers[i];
                var pkg = i % 2 == 0 ? pkg1 : pkg2;
                
                // Only seed sub+payment if member doesn't have any yet
                var hasSub = await _unitOfWork.Subscriptions.GetQueryable().AnyAsync(s => s.MemberId == m.Id);
                if (hasSub) continue;

                var sub = new MemberSubscription
                {
                    MemberId = m.Id,
                    PackageId = pkg.Id,
                    StartDate = today.AddMonths(-1),
                    EndDate = i == 0 ? today.AddDays(2) : today.AddMonths(pkg.DurationInMonths),
                    Status = i == 0 ? SubscriptionStatus.Active : (i % 8 == 0 ? SubscriptionStatus.Expired : SubscriptionStatus.Active),
                    OriginalPrice = pkg.Price,
                    FinalPrice = pkg.Price,
                    OriginalPackageName = pkg.Name
                };
                await _unitOfWork.Subscriptions.AddAsync(sub);
                await _unitOfWork.SaveChangesAsync();

                // Payment for this sub (Distribute over last 6 months)
                var payDate = today.AddMonths(-random.Next(0, 6)).AddDays(random.Next(1, 28));
                await _unitOfWork.Payments.AddAsync(new Payment
                {
                    MemberSubscriptionId = sub.Id,
                    Amount = pkg.Price,
                    PaymentDate = payDate,
                    Status = PaymentStatus.Completed,
                    Method = (PaymentMethod)random.Next(1, 5)
                });
            }

            // Today's Payments for KPI Visibility (2.1)
            // Link to the first demo member's subscription to satisfy FOREIGN KEY constraint
            var firstSubId = allDemoMembers.Count > 0 
                ? (await _unitOfWork.Subscriptions.GetQueryable().FirstOrDefaultAsync(s => s.MemberId == allDemoMembers[0].Id))?.Id 
                : null;

            if (firstSubId != null)
            {
                await _unitOfWork.Payments.AddAsync(new Payment { MemberSubscriptionId = firstSubId.Value, Amount = 1200000, PaymentDate = DateTime.UtcNow.AddMinutes(-30), Status = PaymentStatus.Completed, Method = PaymentMethod.Cash });
                await _unitOfWork.Payments.AddAsync(new Payment { MemberSubscriptionId = firstSubId.Value, Amount = 500000, PaymentDate = DateTime.UtcNow.AddMinutes(-60), Status = PaymentStatus.Completed, Method = PaymentMethod.BankTransfer });
                await _unitOfWork.SaveChangesAsync();
            }

            // 6. Check-ins (Today & Last 7 Days)
            for (int d = 0; d < 7; d++)
            {
                int count = random.Next(10, 30);
                for (int c = 0; c < count; c++)
                {
                    await _unitOfWork.CheckIns.AddAsync(new CheckIn
                    {
                        MemberId = allDemoMembers[random.Next(allDemoMembers.Count)].Id,
                        CheckInTime = today.AddDays(-d).AddHours(random.Next(8, 20)),
                        CheckOutTime = (d == 0 && c < 5) ? null : today.AddDays(-d).AddHours(22) // Some still in gym today
                    });
                }
            }

            // 7. Classes Today
            var dayOfWeekStr = today.DayOfWeek.ToString();
            var class1 = new Class { ClassName = "Zumba Dance", ScheduleDay = dayOfWeekStr, StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(18, 0, 0), TrainerId = trainer1.Id, MaxCapacity = 20, IsActive = true };
            var class2 = new Class { ClassName = "Power Yoga", ScheduleDay = dayOfWeekStr, StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(9, 0, 0), TrainerId = trainer2.Id, MaxCapacity = 15, IsActive = true };
            await _unitOfWork.Classes.AddAsync(class1);
            await _unitOfWork.Classes.AddAsync(class2);
            await _unitOfWork.SaveChangesAsync();

            // 8. Equipment Health
            await _unitOfWork.Equipments.AddAsync(new Equipment { Name = "Máy chạy bộ Matrix", Status = EquipmentStatus.Broken, PurchaseDate = today.AddYears(-1) });
            await _unitOfWork.Equipments.AddAsync(new Equipment { Name = "Dàn tạ tay 5-30kg", Status = EquipmentStatus.Active, PurchaseDate = today.AddMonths(-3) });
            await _unitOfWork.Equipments.AddAsync(new Equipment { Name = "Xe đạp Impulse", Status = EquipmentStatus.Maintenance, NextMaintenanceDate = today.AddDays(-1), PurchaseDate = today.AddMonths(-6) });

            await _unitOfWork.SaveChangesAsync();

            return Ok(new { Message = "Đã tạo dữ liệu mẫu thành công cho Dashboard!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Lỗi khi tạo dữ liệu mẫu: " + ex.Message });
        }
    }
}
