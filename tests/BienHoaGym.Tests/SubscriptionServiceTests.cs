using Microsoft.EntityFrameworkCore;
using Moq;
using AutoMapper;
using Gym.Application.Services;
using Gym.Application.DTOs.Subscriptions;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Gym.Infrastructure.Data;
using Gym.Infrastructure.Repositories;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Gym.Application.Interfaces.Services;

namespace BienHoaGym.Tests;

public class SubscriptionServiceTests
{
    private readonly GymDbContext _context;
    private readonly UnitOfWork _uow;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IAuditLogService> _auditLogMock;
    private readonly Mock<IHttpContextAccessor> _httpContextMock;
    private readonly SubscriptionService _service;

    public SubscriptionServiceTests()
    {
        var options = new DbContextOptionsBuilder<GymDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new GymDbContext(options);
        _uow = new UnitOfWork(_context);
        _mapperMock = new Mock<IMapper>();
        _auditLogMock = new Mock<IAuditLogService>();
        _httpContextMock = new Mock<IHttpContextAccessor>();

        _service = new SubscriptionService(_uow, _mapperMock.Object, _auditLogMock.Object, _httpContextMock.Object);
    }

    [Fact]
    public async Task CreateAsync_NewSubscription_ShouldSetStatusToPendingAndSnapshotPrice()
    {
        // 1. Arrange
        var memberId = Guid.NewGuid();
        var packageId = Guid.NewGuid();
        var member = new Member { Id = memberId, FullName = "Test Member", MemberCode = "M001" };
        var package = new MembershipPackage { Id = packageId, Name = "Basic Month", Price = 500000, DurationInDays = 30, IsActive = true };

        _context.Members.Add(member);
        _context.MembershipPackages.Add(package);
        await _context.SaveChangesAsync();

        var dto = new CreateSubscriptionDto
        {
            MemberId = memberId,
            PackageId = packageId,
            StartDate = DateTime.UtcNow.Date,
            FinalPrice = 500000
        };

        _mapperMock.Setup(m => m.Map<MemberSubscription>(It.IsAny<CreateSubscriptionDto>()))
            .Returns((CreateSubscriptionDto d) => new MemberSubscription { MemberId = d.MemberId, PackageId = d.PackageId, StartDate = d.StartDate });

        // 2. Act
        var result = await _service.CreateAsync(dto);

        // 3. Assert
        Assert.True(result.Success);
        var sub = await _context.MemberSubscriptions.FirstOrDefaultAsync(s => s.MemberId == memberId);
        Assert.NotNull(sub);
        Assert.Equal(SubscriptionStatus.Pending, sub.Status);
        Assert.Equal(500000, sub.OriginalPrice);
        Assert.Equal("Basic Month", sub.OriginalPackageName);
        Assert.Equal(sub.StartDate.AddDays(30), sub.EndDate);
    }

    [Fact]
    public async Task CreateAsync_ReceptionistDiscountMoreThan10Percent_ShouldFail()
    {
        // 1. Arrange
        var memberId = Guid.NewGuid();
        var packageId = Guid.NewGuid();
        var member = new Member { Id = memberId, FullName = "Test Member", MemberCode = "M001" };
        var package = new MembershipPackage { Id = packageId, Name = "Basic Month", Price = 500000, DurationInDays = 30, IsActive = true };

        _context.Members.Add(member);
        _context.MembershipPackages.Add(package);
        await _context.SaveChangesAsync();

        // Mock Receptionist Role
        var claims = new List<Claim> { new Claim(ClaimTypes.Role, "Receptionist") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextMock.Setup(h => h.HttpContext).Returns(httpContext);

        var dto = new CreateSubscriptionDto
        {
            MemberId = memberId,
            PackageId = packageId,
            StartDate = DateTime.UtcNow.Date,
            FinalPrice = 400000 // 20% discount (allowed is 10%)
        };

        // Mock Mapper (Quan trá»ng: Pháº£i setup cho má»—i test hoáº·c dÃ¹ng constructor chung)
        _mapperMock.Setup(m => m.Map<MemberSubscription>(It.IsAny<CreateSubscriptionDto>()))
            .Returns((CreateSubscriptionDto d) => new MemberSubscription { MemberId = d.MemberId, PackageId = d.PackageId, StartDate = d.StartDate });

        // 2. Act
        var result = await _service.CreateAsync(dto);

        // 3. Assert
        Assert.False(result.Success);
        Assert.Contains("10%", result.Message);
    }

    [Fact]
    public async Task PauseAsync_FixedDuration_ShouldImmediatelyExtendEndDate()
    {
        // 1. Arrange
        var subId = Guid.NewGuid();
        var originalEndDate = DateTime.UtcNow.Date.AddDays(20);
        var sub = new MemberSubscription
        {
            Id = subId,
            Status = SubscriptionStatus.Active,
            EndDate = originalEndDate,
            IsDeleted = false
        };
        _context.MemberSubscriptions.Add(sub);
        await _context.SaveChangesAsync();

        // 2. Act
        var result = await _service.PauseAsync(subId, 7); // Pause for 7 days

        // 3. Assert
        Assert.True(result.Success);
        var updatedSub = await _context.MemberSubscriptions.FindAsync(subId);
        Assert.Equal(SubscriptionStatus.Suspended, updatedSub.Status);
        Assert.Equal(originalEndDate.AddDays(7), updatedSub.EndDate);
        Assert.Equal(7, updatedSub.AutoPauseExtensionDays);
        Assert.NotNull(updatedSub.LastPausedAt);
    }

    [Fact]
    public async Task ResumeAsync_FixedDuration_ShouldAdjustEndDateBasedOnActualDays()
    {
        // 1. Arrange
        var subId = Guid.NewGuid();
        // Giáº£ sá»­: NgÃ y háº¿t háº¡n ban Ä‘áº§u lÃ  2024-01-20.
        // Táº¡m dá»«ng 7 ngÃ y dá»± kiáº¿n => EndDate thÃ nh 2024-01-27.
        // NhÆ°ng thá»±c táº¿ nghá»‰ 10 ngÃ y. 
        // Sá»­ dá»¥ng 9.5 ngÃ y Ä‘á»ƒ khi Ceiling lÃªn sáº½ lÃ  Ä‘Ãºng 10 ngÃ y, trÃ¡nh sai sá»‘ miligiÃ¢y.
        var originalEndDatePlusExpected = new DateTime(2024, 1, 27);
        var lastPausedAt = DateTime.UtcNow.AddHours(-230); // ~9.5 ngÃ y

        var sub = new MemberSubscription
        {
            Id = subId,
            Status = SubscriptionStatus.Suspended,
            EndDate = originalEndDatePlusExpected,
            LastPausedAt = lastPausedAt,
            AutoPauseExtensionDays = 7,
            IsDeleted = false
        };
        _context.MemberSubscriptions.Add(sub);
        _context.Members.Add(new Member { Id = Guid.NewGuid(), Status = MemberStatus.Suspended, IsDeleted = false });
        sub.MemberId = _context.Members.Local.First().Id;
        await _context.SaveChangesAsync();

        // 2. Act
        var result = await _service.ResumeAsync(subId);

        // 3. Assert
        Assert.True(result.Success);
        var updatedSub = await _context.MemberSubscriptions.FindAsync(subId);
        Assert.Equal(SubscriptionStatus.Active, updatedSub.Status);
        
        // CÃ´ng thá»©c: 
        // ActualDays = 10
        // Adjustment = 10 - 7 = 3
        // EndDate = 2024-01-27 + 3 days = 2024-01-30
        Assert.Equal(originalEndDatePlusExpected.AddDays(3), updatedSub.EndDate);
    }

    [Fact]
    public async Task RenewAsync_CurrentSubHasMoreThanOneDay_ShouldFail()
    {
        // 1. Arrange
        var memberId = Guid.NewGuid();
        var subId = Guid.NewGuid();
        var packageId = Guid.NewGuid();
        
        var sub = new MemberSubscription 
        { 
            Id = subId, 
            MemberId = memberId, 
            Status = SubscriptionStatus.Active, 
            EndDate = DateTime.UtcNow.Date.AddDays(5) // Still has 5 days
        };
        _context.MemberSubscriptions.Add(sub);
        
        var newPackage = new MembershipPackage { Id = packageId, Name = "Gold Year", Price = 5000000, DurationInDays = 365, IsActive = true };
        _context.MembershipPackages.Add(newPackage);
        await _context.SaveChangesAsync();

        // 2. Act
        var result = await _service.RenewAsync(subId, packageId);

        // 3. Assert
        Assert.False(result.Success);
        Assert.Contains("vẫn còn hạn 5 ngày", result.Message);
    }
}
