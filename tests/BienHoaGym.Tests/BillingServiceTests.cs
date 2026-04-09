using Microsoft.EntityFrameworkCore;
using Moq;
using AutoMapper;
using Gym.Application.Services;
using Gym.Application.DTOs.Billing;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Gym.Infrastructure.Data;
using Gym.Infrastructure.Repositories;
using Xunit;

namespace BienHoaGym.Tests;

public class BillingServiceTests
{
    private readonly GymDbContext _context;
    private readonly UnitOfWork _uow;
    private readonly Mock<IMapper> _mapperMock;
    private readonly BillingService _service;

    public BillingServiceTests()
    {
        var options = new DbContextOptionsBuilder<GymDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new GymDbContext(options);
        _uow = new UnitOfWork(_context);
        _mapperMock = new Mock<IMapper>();
        _service = new BillingService(_uow, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateInvoiceAsync_SellingProduct_ShouldReduceStockQuantity()
    {
        // 1. Arrange
        var productId = Guid.NewGuid();
        var product = new Product 
        { 
            Id = productId, 
            Name = "Energy Drink", 
            SKU = "ED-001", 
            StockQuantity = 100, 
            IsActive = true, 
            IsDeleted = false 
        };
        _context.Products.Add(product);
        
        // Mock Warehouse for inventory deduction (InventoryService uses Guid.Parse("20000000-0000-0000-0000-000000000002"))
        var counterWarehouseId = Guid.Parse("20000000-0000-0000-0000-000000000002");
        var inventory = new Inventory { ProductId = productId, WarehouseId = counterWarehouseId, Quantity = 100 };
        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();

        var dto = new CreateInvoiceDto
        {
            PaymentMethod = PaymentMethod.Cash,
            Details = new List<CreateInvoiceDetailDto>
            {
                new CreateInvoiceDetailDto 
                { 
                    ItemType = "Product", 
                    ReferenceId = productId, 
                    Quantity = 5, 
                    UnitPrice = 20000,
                    ItemName = "Energy Drink"
                }
            }
        };

        _mapperMock.Setup(m => m.Map<InvoiceDetail>(It.IsAny<CreateInvoiceDetailDto>()))
            .Returns((CreateInvoiceDetailDto d) => new InvoiceDetail { ItemName = d.ItemName, UnitPrice = d.UnitPrice, Quantity = d.Quantity });

        // 2. Act
        var result = await _service.CreateInvoiceAsync(dto);

        // 3. Assert
        Assert.True(result.Success);
        
        var updatedProduct = await _context.Products.FindAsync(productId);
        Assert.Equal(95, updatedProduct.StockQuantity);

        var updatedInv = await _context.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);
        Assert.Equal(95, updatedInv.Quantity);
    }

    [Fact]
    public async Task CreateInvoiceAsync_SellingPackage_ShouldCreateActiveSubscription()
    {
        // 1. Arrange
        var memberId = Guid.NewGuid();
        var packageId = Guid.NewGuid();
        var member = new Member { Id = memberId, FullName = "John Gym", MemberCode = "G001" };
        var package = new MembershipPackage { Id = packageId, Name = "VIP 1 Month", DurationDays = 30, IsActive = true };
        
        _context.Members.Add(member);
        _context.MembershipPackages.Add(package);
        await _context.SaveChangesAsync();

        var dto = new CreateInvoiceDto
        {
            MemberId = memberId,
            PaymentMethod = PaymentMethod.BankTransfer,
            Details = new List<CreateInvoiceDetailDto>
            {
                new CreateInvoiceDetailDto 
                { 
                    ItemType = "Package", 
                    ReferenceId = packageId, 
                    Quantity = 1, 
                    UnitPrice = 500000,
                    ItemName = "VIP 1 Month"
                }
            }
        };

        _mapperMock.Setup(m => m.Map<InvoiceDetail>(It.IsAny<CreateInvoiceDetailDto>()))
            .Returns((CreateInvoiceDetailDto d) => new InvoiceDetail { ItemName = d.ItemName, UnitPrice = d.UnitPrice, Quantity = d.Quantity });

        // 2. Act
        var result = await _service.CreateInvoiceAsync(dto);

        // 3. Assert
        Assert.True(result.Success);
        
        var subscription = await _context.MemberSubscriptions.FirstOrDefaultAsync(s => s.MemberId == memberId);
        Assert.NotNull(subscription);
        Assert.Equal(SubscriptionStatus.Active, subscription.Status);
        Assert.Equal("VIP 1 Month", subscription.OriginalPackageName);
    }

    [Fact]
    public async Task CreateInvoiceAsync_WhenMemberAlreadyHasActiveSubscription_ShouldReturnFailure()
    {
        // 1. Arrange
        var memberId = Guid.NewGuid();
        var packageId = Guid.NewGuid();
        
        _context.MemberSubscriptions.Add(new MemberSubscription 
        { 
            MemberId = memberId, 
            Status = SubscriptionStatus.Active, 
            EndDate = DateTime.UtcNow.AddDays(10) 
        });
        await _context.SaveChangesAsync();

        var dto = new CreateInvoiceDto
        {
            MemberId = memberId,
            Details = new List<CreateInvoiceDetailDto>
            {
                new CreateInvoiceDetailDto { ItemType = "Package", ReferenceId = packageId }
            }
        };

        // 2. Act
        var result = await _service.CreateInvoiceAsync(dto);

        // 3. Assert
        Assert.False(result.Success);
        Assert.Contains("vẫn còn hạn", result.Message);
    }
}
