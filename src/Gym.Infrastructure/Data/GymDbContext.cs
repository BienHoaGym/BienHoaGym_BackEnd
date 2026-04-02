using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gym.Infrastructure.Data;

/// <summary>
/// GymDbContext - Database context cho Gym Management System
/// </summary>
public class GymDbContext : DbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public GymDbContext(DbContextOptions<GymDbContext> options, IHttpContextAccessor? httpContextAccessor = null) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // DbSets - Các bảng trong database
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<MembershipPackage> MembershipPackages => Set<MembershipPackage>();
    public DbSet<MemberSubscription> MemberSubscriptions => Set<MemberSubscription>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<CheckIn> CheckIns => Set<CheckIn>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<ClassEnrollment> ClassEnrollments => Set<ClassEnrollment>();
    public DbSet<TrainerMemberAssignment> TrainerMemberAssignments => Set<TrainerMemberAssignment>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceDetail> InvoiceDetails => Set<InvoiceDetail>();
    
    // Inventory
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();
    
    // Orders (Sản phẩm)
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

    // Equipment
    public DbSet<Equipment> Equipments => Set<Equipment>();
    public DbSet<EquipmentTransaction> EquipmentTransactions => Set<EquipmentTransaction>();
    public DbSet<MaintenanceLog> MaintenanceLogs => Set<MaintenanceLog>();
    public DbSet<Depreciation> Depreciations => Set<Depreciation>();

    // Bảng AuditLogs để lưu vết hệ thống
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<EquipmentCategory> EquipmentCategories => Set<EquipmentCategory>();
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<IncidentLog> IncidentLogs => Set<IncidentLog>();
    public DbSet<EquipmentProviderHistory> EquipmentProviderHistories => Set<EquipmentProviderHistory>();
    public DbSet<MaintenanceMaterial> MaintenanceMaterials => Set<MaintenanceMaterial>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => { });
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure table names
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<UserRole>().ToTable("UserRoles");
        modelBuilder.Entity<Member>().ToTable("Members");
        modelBuilder.Entity<MembershipPackage>().ToTable("MembershipPackages");
        modelBuilder.Entity<MemberSubscription>().ToTable("MemberSubscriptions");
        modelBuilder.Entity<Payment>().ToTable("Payments");
        modelBuilder.Entity<CheckIn>().ToTable("CheckIns");
        modelBuilder.Entity<Trainer>().ToTable("Trainers");
        modelBuilder.Entity<Class>().ToTable("Classes");
        modelBuilder.Entity<ClassEnrollment>().ToTable("ClassEnrollments");
        modelBuilder.Entity<TrainerMemberAssignment>().ToTable("TrainerMemberAssignments");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<Invoice>().ToTable("Invoices");
        modelBuilder.Entity<InvoiceDetail>().ToTable("InvoiceDetails");
        
        modelBuilder.Entity<Warehouse>().ToTable("Warehouses");
        modelBuilder.Entity<Inventory>().ToTable("Inventories");
        modelBuilder.Entity<StockTransaction>().ToTable("StockTransactions");
        
        modelBuilder.Entity<Order>().ToTable("Orders");
        modelBuilder.Entity<OrderDetail>().ToTable("OrderDetails");

        modelBuilder.Entity<Equipment>().ToTable("Equipments");
        modelBuilder.Entity<EquipmentTransaction>().ToTable("EquipmentTransactions");
        modelBuilder.Entity<MaintenanceLog>().ToTable("MaintenanceLogs");
        modelBuilder.Entity<Depreciation>().ToTable("Depreciations");
        modelBuilder.Entity<EquipmentCategory>().ToTable("EquipmentCategories");
        modelBuilder.Entity<Provider>().ToTable("Providers");
        modelBuilder.Entity<IncidentLog>().ToTable("IncidentLogs");
        modelBuilder.Entity<EquipmentProviderHistory>().ToTable("EquipmentProviderHistories");
        modelBuilder.Entity<AuditLog>().ToTable("AuditLogs");

        modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        // --- CẤU HÌNH THUỘC TÍNH CHI TIẾT ---
        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.MemberCode)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.FaceEncoding);

            entity.Property(e => e.QRCode)
                .HasMaxLength(255);

            entity.HasIndex(e => e.FullName);
        });

        modelBuilder.Entity<MemberSubscription>(entity =>
        {
            entity.Property(s => s.OriginalPackageName)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValue("");

            entity.Property(s => s.OriginalPrice).HasPrecision(18, 2);
            entity.Property(s => s.DiscountApplied).HasPrecision(18, 2);
            entity.Property(s => s.FinalPrice).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.SKU)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.HasIndex(p => p.SKU).IsUnique();
            
            entity.Property(p => p.Price).HasPrecision(18, 2);
            entity.Property(p => p.CostPrice).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.Property(i => i.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(i => i.TotalAmount).HasPrecision(18, 2);
            entity.Property(i => i.DiscountAmount).HasPrecision(18, 2);
            
            entity.HasOne(i => i.Member)
                .WithMany()
                .HasForeignKey(i => i.MemberId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.Property(d => d.ItemName)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(d => d.UnitPrice).HasPrecision(18, 2);
            
            entity.HasOne(d => d.Invoice)
                .WithMany(i => i.Details)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure relationships & indexes
        ConfigureUserRelationships(modelBuilder);
        ConfigureMemberRelationships(modelBuilder);
        ConfigureSubscriptionRelationships(modelBuilder);
        ConfigureTrainerRelationships(modelBuilder);
        ConfigureClassRelationships(modelBuilder);
        ConfigureAssignmentRelationships(modelBuilder);
        ConfigureEquipmentRelationships(modelBuilder); // Tách riêng equipment
        ConfigureIndexes(modelBuilder);
        ConfigureEnums(modelBuilder);
        ConfigureDecimalPrecision(modelBuilder);

        // Seed initial data
        SeedData(modelBuilder);
    }

    private static void ConfigureEquipmentRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.EquipmentCategory)
            .WithMany(c => c.Equipments)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Provider)
            .WithMany(p => p.Equipments)
            .HasForeignKey(e => e.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MaintenanceLog>()
            .HasOne(m => m.Equipment)
            .WithMany(e => e.MaintenanceLogs)
            .HasForeignKey(m => m.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<IncidentLog>()
            .HasOne(i => i.Equipment)
            .WithMany(e => e.IncidentLogs)
            .HasForeignKey(i => i.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EquipmentProviderHistory>()
            .HasOne(h => h.Equipment)
            .WithMany(e => e.ProviderHistories)
            .HasForeignKey(h => h.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MaintenanceMaterial>(entity =>
        {
            entity.HasOne(m => m.MaintenanceLog)
                .WithMany(l => l.Materials)
                .HasForeignKey(m => m.MaintenanceLogId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.Product)
                .WithMany()
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    // Đã chuyển các hàm cấu hình thành static theo khuyến nghị của IDE để tối ưu bộ nhớ
    private static void ConfigureUserRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Member)
            .WithOne(m => m.User)
            .HasForeignKey<Member>(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Trainer)
            .WithOne(t => t.User)
            .HasForeignKey<Trainer>(t => t.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureMemberRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>()
            .HasMany(m => m.Subscriptions)
            .WithOne(s => s.Member)
            .HasForeignKey(s => s.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Member>()
            .HasMany(m => m.CheckIns)
            .WithOne(c => c.Member)
            .HasForeignKey(c => c.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Member>()
            .HasMany(m => m.ClassEnrollments)
            .WithOne(ce => ce.Member)
            .HasForeignKey(ce => ce.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureSubscriptionRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MemberSubscription>()
            .HasOne(s => s.Package)
            .WithMany(p => p.Subscriptions)
            .HasForeignKey(s => s.PackageId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MemberSubscription>()
            .HasMany(s => s.Payments)
            .WithOne(p => p.Subscription)
            .HasForeignKey(p => p.MemberSubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MemberSubscription>()
            .HasMany(s => s.CheckIns)
            .WithOne(c => c.Subscription)
            .HasForeignKey(c => c.SubscriptionId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureTrainerRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trainer>()
            .HasMany(t => t.Classes)
            .WithOne(c => c.Trainer)
            .HasForeignKey(c => c.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureClassRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>()
            .HasMany(c => c.ClassEnrollments)
            .WithOne(ce => ce.Class)
            .HasForeignKey(ce => ce.ClassId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ClassEnrollment>()
            .HasIndex(ce => new { ce.ClassId, ce.MemberId })
            .IsUnique();
    }

    private static void ConfigureAssignmentRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrainerMemberAssignment>()
            .HasOne(a => a.Trainer)
            .WithMany(t => t.TrainerMemberAssignments)
            .HasForeignKey(a => a.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TrainerMemberAssignment>()
            .HasOne(a => a.Member)
            .WithMany(m => m.TrainerMemberAssignments)
            .HasForeignKey(a => a.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Provider)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockTransaction>(entity =>
        {
            entity.HasOne(t => t.Product)
                .WithMany()
                .HasForeignKey(t => t.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.FromWarehouse)
                .WithMany()
                .HasForeignKey(t => t.FromWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.ToWarehouse)
                .WithMany()
                .HasForeignKey(t => t.ToWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasOne(d => d.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureIndexes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<Member>().HasIndex(m => m.MemberCode).IsUnique().HasFilter("IsDeleted = 0");
        modelBuilder.Entity<Member>().HasIndex(m => m.Email);
        modelBuilder.Entity<Member>().HasIndex(m => m.PhoneNumber);
        modelBuilder.Entity<Member>().HasIndex(m => m.UserId);
        modelBuilder.Entity<Member>().HasIndex(m => m.Status).HasFilter("IsDeleted = 0");
        modelBuilder.Entity<MembershipPackage>().HasIndex(p => p.IsActive).HasFilter("IsDeleted = 0");
        modelBuilder.Entity<MemberSubscription>().HasIndex(s => s.MemberId);
        modelBuilder.Entity<MemberSubscription>().HasIndex(s => s.PackageId);
        modelBuilder.Entity<MemberSubscription>().HasIndex(s => s.Status).HasFilter("IsDeleted = 0");
        modelBuilder.Entity<MemberSubscription>().HasIndex(s => s.EndDate).HasFilter("Status = 2");
        modelBuilder.Entity<MemberSubscription>().HasIndex(s => new { s.MemberId, s.Status });
        modelBuilder.Entity<Payment>().HasIndex(p => p.MemberSubscriptionId);
        modelBuilder.Entity<Payment>().HasIndex(p => p.PaymentDate);
        modelBuilder.Entity<Payment>().HasIndex(p => p.Status);
        modelBuilder.Entity<Payment>().HasIndex(p => p.TransactionId).HasFilter("TransactionId IS NOT NULL");
        modelBuilder.Entity<CheckIn>().HasIndex(c => c.MemberId);
        modelBuilder.Entity<CheckIn>().HasIndex(c => c.SubscriptionId);
        modelBuilder.Entity<CheckIn>().HasIndex(c => c.CheckInTime);
        modelBuilder.Entity<CheckIn>().HasIndex(c => new { c.MemberId, c.CheckInTime });
        modelBuilder.Entity<Trainer>().HasIndex(t => t.UserId).HasFilter("UserId IS NOT NULL");
        modelBuilder.Entity<Trainer>().HasIndex(t => t.Email).HasFilter("Email IS NOT NULL");
        modelBuilder.Entity<Trainer>().HasIndex(t => t.IsActive).HasFilter("IsDeleted = 0");
        modelBuilder.Entity<Class>().HasIndex(c => c.TrainerId);
        modelBuilder.Entity<Class>().HasIndex(c => c.ScheduleDay).HasFilter("IsActive = 1");
        modelBuilder.Entity<Class>().HasIndex(c => new { c.ScheduleDay, c.StartTime }).HasFilter("IsActive = 1");
        modelBuilder.Entity<ClassEnrollment>().HasIndex(ce => ce.MemberId);
        modelBuilder.Entity<ClassEnrollment>().HasIndex(ce => ce.ClassId);
        modelBuilder.Entity<ClassEnrollment>().HasIndex(ce => ce.EnrolledDate);
        modelBuilder.Entity<Product>().HasIndex(p => p.SKU).IsUnique().HasFilter("IsDeleted = 0");
        modelBuilder.Entity<Invoice>().HasIndex(i => i.InvoiceNumber).IsUnique().HasFilter("IsDeleted = 0");
    }

    private static void ConfigureEnums(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>().Property(m => m.Status).HasConversion<int>();
        modelBuilder.Entity<MemberSubscription>().Property(s => s.Status).HasConversion<int>();
        modelBuilder.Entity<Payment>().Property(p => p.Status).HasConversion<int>();
        modelBuilder.Entity<Payment>().Property(p => p.Method).HasConversion<int>();
        modelBuilder.Entity<AuditLog>().Property(a => a.Severity).HasConversion<int>();
        modelBuilder.Entity<Equipment>().Property(e => e.Status).HasConversion<int>();
        modelBuilder.Entity<Equipment>().Property(e => e.Priority).HasConversion<int>();
        modelBuilder.Entity<MaintenanceLog>().Property(m => m.Status).HasConversion<int>();
        modelBuilder.Entity<EquipmentTransaction>().Property(t => t.Type).HasConversion<int>();
        modelBuilder.Entity<StockTransaction>().Property(t => t.Type).HasConversion<int>();
    }

    private static void ConfigureDecimalPrecision(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MembershipPackage>().Property(p => p.Price).HasPrecision(18, 2);
        modelBuilder.Entity<MembershipPackage>().Property(p => p.DiscountPrice).HasPrecision(18, 2);
        modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Trainer>().Property(t => t.Salary).HasPrecision(18, 2);
        modelBuilder.Entity<Trainer>().Property(t => t.SessionRate).HasPrecision(18, 2);
        
        modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 2);
        modelBuilder.Entity<OrderDetail>().Property(d => d.Price).HasPrecision(18, 2);
        
        modelBuilder.Entity<Equipment>().Property(e => e.PurchasePrice).HasPrecision(18, 2);
        modelBuilder.Entity<Equipment>().Property(e => e.SalvageValue).HasPrecision(18, 2);
        modelBuilder.Entity<MaintenanceLog>().Property(m => m.Cost).HasPrecision(18, 2);
        modelBuilder.Entity<Depreciation>().Property(d => d.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Depreciation>().Property(d => d.RemainingValue).HasPrecision(18, 2);
        
        modelBuilder.Entity<Equipment>().Property(e => e.MonthlyDepreciationAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Equipment>().Property(e => e.AccumulatedDepreciation).HasPrecision(18, 2);
        modelBuilder.Entity<Equipment>().Property(e => e.RemainingValue).HasPrecision(18, 2);
        modelBuilder.Entity<MaintenanceMaterial>().Property(m => m.UnitPrice).HasPrecision(18, 2);
        
        modelBuilder.Entity<EquipmentCategory>().Property(ec => ec.AvgMaintenanceCost).HasPrecision(18, 2);
        
        modelBuilder.Entity<StockTransaction>().Property(t => t.UnitPrice).HasPrecision(18, 2);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var hasher = new PasswordHasher<User>();
        string defaultPassword = "Admin@123";

        // Đảm bảo CreatedAt có giá trị cố định để tránh tạo migration thừa
        DateTime seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Role>().HasData(
            new Role 
            { 
                Id = 1, RoleName = "Admin", Description = "👑 Admin/Manager (Quản lý chủ gym) - TOÀN QUYỀN", 
                Permissions = "\"*\"", CreatedAt = seedDate 
            },
            new Role 
            { 
                Id = 2, RoleName = "Manager", Description = "Tiểu quản lý vận hành", 
                Permissions = System.Text.Json.JsonSerializer.Serialize(new List<string> { "*" }), 
                CreatedAt = seedDate 
            },
            new Role 
            { 
                Id = 3, RoleName = "Trainer", Description = "🏋️ Trainer - Chuyên môn", 
                Permissions = System.Text.Json.JsonSerializer.Serialize(new List<string> 
                { 
                    "member.read", "class.manage", "equipment.read", "equipment.report", "inventory.consume", "product.read", "trainer.read" 
                }), 
                CreatedAt = seedDate 
            },
            new Role 
            { 
                Id = 4, RoleName = "Receptionist", Description = "👩💼 Receptionist - Vận hành hàng ngày", 
                Permissions = System.Text.Json.JsonSerializer.Serialize(new List<string> 
                { 
                    "member.read", "member.create", "member.update", "checkin.create", "checkin.read", "package.read", "inventory.read", "inventory.consume", "report.read", "subscription.read", "subscription.create", "subscription.update", "payment.read", "payment.create", "billing.read", "billing.create", "product.read", "class.read", "trainer.read"
                }), 
                CreatedAt = seedDate 
            },
            new Role 
            { 
                Id = 5, RoleName = "Member", Description = "👤 Member - Tự phục vụ", 
                Permissions = System.Text.Json.JsonSerializer.Serialize(new List<string> 
                { 
                    "member.read", "checkin.self", "class.read", "subscription.read" 
                }), 
                CreatedAt = seedDate 
            },
            new Role
            {
                Id = 6, RoleName = "Accountant", Description = "💰 Accountant - Quản lý tài chính",
                Permissions = System.Text.Json.JsonSerializer.Serialize(new List<string>
                {
                    "billing.read", "billing.create", "payment.read", "payment.create", "report.read", "package.read", "member.read", "subscription.read", "subscription.create", "subscription.update", "checkin.read", "product.read", "class.read", "trainer.read"
                }),
                CreatedAt = seedDate
            }
        );

        var adminUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var managerUserId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var trainerUserId1 = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var trainerUserId2 = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var receptionistUserId = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var multiRoleUserId = Guid.Parse("99999999-9999-9999-9999-999999999999"); // Lê Văn C - Multi Role
        var memberUserId1 = Guid.Parse("66666666-6666-6666-6666-666666666666");
        var memberUserId2 = Guid.Parse("77777777-7777-7777-7777-777777777777"); // Added back
        var memberUserId3 = Guid.Parse("88888888-8888-8888-8888-888888888888"); // Added back

        var users = new List<User>
        {
            new User { Id = adminUserId, Username = "admin", Email = "admin@gym.com", FullName = "System Administrator", PhoneNumber = "0901234567", IsActive = true, CreatedAt = seedDate },
            new User { Id = managerUserId, Username = "manager", Email = "manager@gym.com", FullName = "Nguyễn Văn Manager", PhoneNumber = "0902345678", IsActive = true, CreatedAt = seedDate },
            new User { Id = trainerUserId1, Username = "trainer1", Email = "trainer1@gym.com", FullName = "Trần Thị Hương", PhoneNumber = "0903456789", IsActive = true, CreatedAt = seedDate },
            new User { Id = trainerUserId2, Username = "trainer2", Email = "trainer2@gym.com", FullName = "Lê Văn Nam", PhoneNumber = "0904567890", IsActive = true, CreatedAt = seedDate },
            new User { Id = receptionistUserId, Username = "receptionist", Email = "receptionist@gym.com", FullName = "Phạm Thị Lan", PhoneNumber = "0905678901", IsActive = true, CreatedAt = seedDate },
            new User { Id = multiRoleUserId, Username = "levanc", Email = "levanc@gym.com", FullName = "Lê Văn C", PhoneNumber = "0909999999", IsActive = true, CreatedAt = seedDate },
            new User { Id = memberUserId1, Username = "member001", Email = "nguyenvana@gmail.com", FullName = "Nguyễn Văn A", PhoneNumber = "0906789012", IsActive = true, CreatedAt = seedDate },
            new User { Id = memberUserId2, Username = "member002", Email = "tranthib@gmail.com", FullName = "Trần Thị B", PhoneNumber = "0907890123", IsActive = true, CreatedAt = seedDate },
            new User { Id = memberUserId3, Username = "member003", Email = "levanc_member@gmail.com", FullName = "Lê Văn C (Member)", PhoneNumber = "0908901234", IsActive = true, CreatedAt = seedDate }
        };

        foreach (var user in users)
        {
            user.PasswordHash = hasher.HashPassword(user, defaultPassword);
        }

        modelBuilder.Entity<User>().HasData(users);

        // Gán quyền cho User (Seed UserRoles)
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { UserId = adminUserId, RoleId = 1 },
            new UserRole { UserId = managerUserId, RoleId = 2 },
            new UserRole { UserId = trainerUserId1, RoleId = 3 },
            new UserRole { UserId = trainerUserId2, RoleId = 3 },
            new UserRole { UserId = receptionistUserId, RoleId = 4 },
            new UserRole { UserId = multiRoleUserId, RoleId = 4 }, // Lễ tân
            new UserRole { UserId = multiRoleUserId, RoleId = 3 }, // + Trainer (Multi-role)
            new UserRole { UserId = memberUserId1, RoleId = 5 },
            new UserRole { UserId = memberUserId2, RoleId = 5 },
            new UserRole { UserId = memberUserId3, RoleId = 5 }
        );

        var trainerId1 = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var trainerId2 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        modelBuilder.Entity<Trainer>().HasData(
            new() { Id = trainerId1, UserId = trainerUserId1, FullName = "Trần Thị Hương", Email = "trainer1@gym.com", PhoneNumber = "0903456789", Specialization = "Yoga, Pilates, Meditation", Bio = "HLV Yoga với 8 năm kinh nghiệm, chứng chỉ quốc tế RYT-500", HireDate = new DateTime(2020, 3, 15), IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = trainerId2, UserId = trainerUserId2, FullName = "Lê Văn Nam", Email = "trainer2@gym.com", PhoneNumber = "0904567890", Specialization = "Boxing, Muay Thai, CrossFit", Bio = "Võ sư Boxing chuyên nghiệp, từng thi đấu SEA Games", HireDate = new DateTime(2021, 6, 1), IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        var package1Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        var package2Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
        var package3Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var package4Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        var package5Id = Guid.Parse("10101010-1010-1010-1010-101010101010");

        modelBuilder.Entity<MembershipPackage>().HasData(
            new() { Id = package1Id, Name = "Gói Cơ Bản 1 Tháng", Description = "Truy cập phòng gym không giới hạn trong 30 ngày", DurationInDays = 30, DurationInMonths = 1, Price = 500000m, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = package2Id, Name = "Gói Premium 1 Tháng", Description = "Gym + Tất cả lớp học trong 30 ngày", DurationInDays = 30, DurationInMonths = 1, Price = 800000m, DiscountPrice = 750000m, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = package3Id, Name = "Gói 10 Buổi Tập", Description = "10 buổi tập, hạn sử dụng 60 ngày", DurationInDays = 60, DurationInMonths = 2, Price = 450000m, SessionLimit = 10, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = package4Id, Name = "Gói 3 Tháng", Description = "Truy cập không giới hạn 90 ngày", DurationInDays = 90, DurationInMonths = 3, Price = 1200000m, DiscountPrice = 1100000m, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = package5Id, Name = "Gói VIP 1 Năm", Description = "Toàn bộ dịch vụ + PT cá nhân trong 365 ngày", DurationInDays = 365, DurationInMonths = 12, Price = 4500000m, DiscountPrice = 4000000m, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        var memberId1 = Guid.Parse("20202020-2020-2020-2020-202020202020");
        var memberId2 = Guid.Parse("30303030-3030-3030-3030-303030303030");
        var memberId3 = Guid.Parse("40404040-4040-4040-4040-404040404040");

        modelBuilder.Entity<Member>().HasData(
            new() { Id = memberId1, UserId = memberUserId1, FullName = "Nguyễn Văn A", Email = "nguyenvana@gmail.com", PhoneNumber = "0906789012", DateOfBirth = new DateTime(1995, 5, 15), MemberCode = "GYM2024001", Status = MemberStatus.Active, JoinedDate = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc), FaceEncoding = "MOCK_FACE_VECTOR_GYM2024001" },
            new() { Id = memberId2, UserId = memberUserId2, FullName = "Trần Thị B", Email = "tranthib@gmail.com", PhoneNumber = "0907890123", DateOfBirth = new DateTime(1992, 8, 20), MemberCode = "GYM2024002", Status = MemberStatus.Active, JoinedDate = new DateTime(2024, 2, 5, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2024, 2, 5, 0, 0, 0, DateTimeKind.Utc), FaceEncoding = "MOCK_FACE_VECTOR_GYM2024002" },
            new() { Id = memberId3, UserId = memberUserId3, FullName = "Lê Văn C", Email = "levanc@gmail.com", PhoneNumber = "0908901234", DateOfBirth = new DateTime(1998, 12, 10), MemberCode = "GYM2024003", Status = MemberStatus.Active, JoinedDate = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc), FaceEncoding = "MOCK_FACE_VECTOR_GYM2024003" }
        );

        var classId1 = Guid.Parse("50505050-5050-5050-5050-505050505050");
        var classId2 = Guid.Parse("60606060-6060-6060-6060-606060606060");
        var classId3 = Guid.Parse("70707070-7070-7070-7070-707070707070");

        modelBuilder.Entity<Class>().HasData(
            new() { Id = classId1, ClassName = "Yoga Buổi Sáng", Description = "Yoga cơ bản cho người mới bắt đầu", TrainerId = trainerId1, ScheduleDay = "Monday", StartTime = new TimeSpan(6, 0, 0), EndTime = new TimeSpan(7, 30, 0), MaxCapacity = 20, CurrentEnrollment = 0, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = classId2, ClassName = "Boxing Tối", Description = "Boxing căn bản và nâng cao", TrainerId = trainerId2, ScheduleDay = "Wednesday", StartTime = new TimeSpan(18, 0, 0), EndTime = new TimeSpan(19, 30, 0), MaxCapacity = 15, CurrentEnrollment = 0, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = classId3, ClassName = "Pilates Chiều", Description = "Pilates cho sức khỏe và vóc dáng", TrainerId = trainerId1, ScheduleDay = "Friday", StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(17, 0, 0), MaxCapacity = 12, CurrentEnrollment = 0, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        var subscriptionId1 = Guid.Parse("80808080-8080-8080-8080-808080808080");
        var subscriptionId2 = Guid.Parse("90909090-9090-9090-9090-909090909090");

        modelBuilder.Entity<MemberSubscription>().HasData(
            new() { Id = subscriptionId1, MemberId = memberId1, PackageId = package2Id, StartDate = new DateTime(2024, 2, 1), EndDate = new DateTime(2024, 3, 2), Status = SubscriptionStatus.Active, CreatedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = subscriptionId2, MemberId = memberId2, PackageId = package3Id, StartDate = new DateTime(2024, 2, 5), EndDate = new DateTime(2024, 4, 5), Status = SubscriptionStatus.Active, RemainingSessions = 10, CreatedAt = new DateTime(2024, 2, 5, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Payment>().HasData(
            new() { Id = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), MemberSubscriptionId = subscriptionId1, Amount = 750000m, Method = PaymentMethod.BankTransfer, PaymentDate = new DateTime(2024, 2, 1, 10, 30, 0, DateTimeKind.Utc), Status = PaymentStatus.Completed, TransactionId = "TXN20240201001", Note = "Thanh toán gói Premium 1 tháng", CreatedAt = new DateTime(2024, 2, 1, 10, 30, 0, DateTimeKind.Utc) },
            new() { Id = Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), MemberSubscriptionId = subscriptionId2, Amount = 450000m, Method = PaymentMethod.Cash, PaymentDate = new DateTime(2024, 2, 5, 14, 15, 0, DateTimeKind.Utc), Status = PaymentStatus.Completed, Note = "Thanh toán gói 10 buổi tập", CreatedAt = new DateTime(2024, 2, 5, 14, 15, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Warehouse>().HasData(
            new Warehouse { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Kho Tổng (Main)", Description = "Kho nhập hàng lớn và lưu trữ chính", Location = "Tầng hầm", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Warehouse { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Name = "Kho Quầy", Description = "Kho bán lẻ & vật tư tại quầy", Location = "Sảnh chính", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        var providerId1 = Guid.Parse("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1");
        modelBuilder.Entity<Provider>().HasData(
            new Provider { Id = providerId1, Name = "Công ty Thiết bị Gym Toàn Cầu", ContactPerson = "Nguyễn Văn Cung", Email = "contact@gymglobal.com", PhoneNumber = "02838445566", Address = "123 Đường số 7, TP.HCM", TaxCode = "0314567890", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        var categoryId1 = Guid.Parse("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1");
        var categoryId2 = Guid.Parse("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2");
        var categoryId3 = Guid.Parse("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3");
        var categoryId4 = Guid.Parse("f4f4f4f4-f4f4-f4f4-f4f4-f4f4f4f4f4f4");

        modelBuilder.Entity<EquipmentCategory>().HasData(
            new EquipmentCategory { Id = categoryId1, Name = "Máy Cardio", Code = "CARDIO", Description = "Máy chạy bộ, xe đạp, trượt tuyết", Group = "Máy móc", AvgMaintenanceCost = 200000m, CreatedAt = seedDate },
            new EquipmentCategory { Id = categoryId2, Name = "Máy Tập Khối (Strength)", Code = "STRENGTH", Description = "Máy tập cơ ngực, xô, vai, chân", Group = "Máy móc", AvgMaintenanceCost = 150000m, CreatedAt = seedDate },
            new EquipmentCategory { Id = categoryId3, Name = "Tạ & Phụ kiện", Code = "FREEWEIGHT", Description = "Tạ tay, tạ đòn, bóng tập", Group = "Dụng cụ", AvgMaintenanceCost = 50000m, CreatedAt = seedDate },
            new EquipmentCategory { Id = categoryId4, Name = "Thiết bị Điện tử", Code = "ELECTRONIC", Description = "Màn hình, hệ thống âm thanh, cổng từ", Group = "Nội thất", AvgMaintenanceCost = 300000m, CreatedAt = seedDate }
        );

        var productId1 = Guid.Parse("91919191-9191-9191-9191-919191919191");
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = productId1, Name = "Nước suối Lavie 500ml", Description = "Nước khoáng thiên nhiên", SKU = "WTR-001", Price = 10000m, CostPrice = 5000m, Unit = "Chai", Type = ProductType.Retail, StockQuantity = 100, MinStockThreshold = 20, MaxStockThreshold = 200, TrackInventory = true, Category = "Đồ uống", IsActive = true, ProviderId = providerId1, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Equipment>().HasData(
            new Equipment { Id = Guid.Parse("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), EquipmentCode = "EQP-RUN-001", Name = "Máy chạy bộ Matrix T7", SerialNumber = "MTX7-2024-X1", CategoryId = categoryId1, ProviderId = providerId1, Quantity = 5, PurchaseDate = new DateTime(2024, 1, 1), PurchasePrice = 25000000m, SalvageValue = 5000000m, UsefulLifeMonths = 60, Status = EquipmentStatus.Active, Priority = EquipmentPriority.High, Location = "Khu Cardio Tầng 1", MaintenanceIntervalDays = 90, MonthlyDepreciationAmount = 333333m, RemainingValue = 25000000m, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        ProcessAuditLogs();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        ProcessAuditLogs();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Gym.Domain.Common.BaseEntity &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Gym.Domain.Common.BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }

    private void ProcessAuditLogs()
    {
        var context = _httpContextAccessor?.HttpContext;
        var userId = context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "System";
        var userName = context?.User?.FindFirst(ClaimTypes.Name)?.Value ?? (context?.User?.Identity?.Name ?? "System");
        var role = context?.User?.FindFirst(ClaimTypes.Role)?.Value ?? "N/A";
        var ip = context?.Connection?.RemoteIpAddress?.ToString();
        var ua = context?.Request?.Headers["User-Agent"].ToString();
        var reason = context?.Request?.Headers["X-Audit-Reason"].ToString();

        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is not AuditLog &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
            .ToList();

        var auditLogs = new List<AuditLog>();

        foreach (var entry in entries)
        {
            var entityType = entry.Entity.GetType();
            var entityName = entityType.Name;

            // XÁC ĐỊNH SEVERITY TỰ ĐỘNG
            var severity = AuditSeverity.Normal;
            if (entityName == "User" || entityName == "Role" || entityName == "MembershipPackage" || entityName == "Product")
            {
                severity = AuditSeverity.Critical; // Các thay đổi về người dùng, giá, quyền đều là Critical
            }
            else if (entry.State == EntityState.Deleted)
            {
                severity = AuditSeverity.Critical; // Mọi hành vi XÓA đều là Critical
            }
            else if (entityName == "Inventory" || entityName == "StockTransaction" || entityName == "Equipment")
            {
                severity = AuditSeverity.Important;
            }

            // Lấy ResourceId và ResourceName (dùng Reflection để linh hoạt)
            string? resourceId = entry.Property("Id")?.CurrentValue?.ToString() ?? (entry.Property("Id")?.OriginalValue?.ToString());
            string? resourceName = "";
            var nameProp = entityType.GetProperty("Name") ?? entityType.GetProperty("FullName") ?? entityType.GetProperty("ClassName") ?? entityType.GetProperty("MemberCode");
            if (nameProp != null)
            {
                resourceName = nameProp.GetValue(entry.Entity)?.ToString();
            }

            var auditLog = new AuditLog
            {
                UserId = userId,
                UserName = userName,
                UserRole = role,
                Action = $"{entry.State}.{entityName.ToLower()}",
                EntityName = entityName,
                ResourceId = resourceId,
                ResourceName = resourceName,
                IPAddress = ip,
                UserAgent = ua,
                Reason = reason,
                Severity = severity,
                Timestamp = DateTime.UtcNow
            };

            var oldValues = new Dictionary<string, object?>();
            var newValues = new Dictionary<string, object?>();

            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;

                if (entry.State == EntityState.Added)
                {
                    newValues[propertyName] = property.CurrentValue;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    oldValues[propertyName] = property.OriginalValue;
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (property.IsModified && !Equals(property.OriginalValue, property.CurrentValue))
                    {
                        oldValues[propertyName] = property.OriginalValue;
                        newValues[propertyName] = property.CurrentValue;
                    }
                }
            }

            auditLog.OldValues = oldValues.Count == 0 ? "{}" : JsonSerializer.Serialize(oldValues);
            auditLog.NewValues = newValues.Count == 0 ? "{}" : JsonSerializer.Serialize(newValues);

            // GHI LOG: 
            // 1. Critical ghi 100%
            // 2. Important ghi 80% (tạm thời ghi 100% để debug dễ, sau này random)
            // 3. Normal ghi 20% (tạm thời ghi 100%)
            // Lưu ý: Trong môi trường thực tế sẽ dùng Random để giảm tải
            if (oldValues.Count > 0 || newValues.Count > 0 || entry.State == EntityState.Deleted)
            {
                auditLogs.Add(auditLog);
            }
        }

        if (auditLogs.Count > 0)
        {
            AuditLogs.AddRange(auditLogs);
        }
    }
}
