using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Repositories;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gym.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GymDbContext _context;
    private IMemberRepository? _memberRepository;
    private IPackageRepository? _packageRepository;
    private ISubscriptionRepository? _subscriptionRepository;
    private IPaymentRepository? _paymentRepository;
    private ICheckInRepository? _checkInRepository;
    private ITrainerRepository? _trainerRepository;
    private IClassRepository? _classRepository;
    private IUserRepository? _userRepository;
    private IRoleRepository? _roleRepository;
    private IClassEnrollmentRepository? _classEnrollmentRepository;

    public UnitOfWork(GymDbContext context)
    {
        _context = context;
    }

    public IMemberRepository Members =>
        _memberRepository ??= new MemberRepository(_context);

    public IPackageRepository Packages =>
        _packageRepository ??= new PackageRepository(_context);

    public ISubscriptionRepository Subscriptions =>
        _subscriptionRepository ??= new SubscriptionRepository(_context);

    public IPaymentRepository Payments =>
        _paymentRepository ??= new PaymentRepository(_context);

    public ICheckInRepository CheckIns =>
        _checkInRepository ??= new CheckInRepository(_context);

    public ITrainerRepository Trainers =>
        _trainerRepository ??= new TrainerRepository(_context);

    public IClassRepository Classes =>
        _classRepository ??= new ClassRepository(_context);

    public IUserRepository Users =>
        _userRepository ??= new UserRepository(_context);

    public IRoleRepository Roles =>
        _roleRepository ??= new RoleRepository(_context);

    public IClassEnrollmentRepository ClassEnrollments =>
        _classEnrollmentRepository ??= new ClassEnrollmentRepository(_context);
    
    public IGenericRepository<TrainerMemberAssignment> TrainerMemberAssignments => 
        new GenericRepository<TrainerMemberAssignment>(_context);

    public IGenericRepository<Product> Products => new GenericRepository<Product>(_context);
    public IGenericRepository<Invoice> Invoices => new GenericRepository<Invoice>(_context);

    // Inventory module
    public IGenericRepository<Warehouse> Warehouses => new GenericRepository<Warehouse>(_context);
    public IGenericRepository<Inventory> Inventories => new GenericRepository<Inventory>(_context);
    public IGenericRepository<StockTransaction> StockTransactions => new GenericRepository<StockTransaction>(_context);
    public IGenericRepository<StockAudit> StockAudits => new GenericRepository<StockAudit>(_context);
    public IGenericRepository<StockAuditDetail> StockAuditDetails => new GenericRepository<StockAuditDetail>(_context);
    public IGenericRepository<Order> Orders => new GenericRepository<Order>(_context);
    public IGenericRepository<OrderDetail> OrderDetails => new GenericRepository<OrderDetail>(_context);

    // Equipment module
    public IGenericRepository<Equipment> Equipments => new GenericRepository<Equipment>(_context);
    public IGenericRepository<EquipmentTransaction> EquipmentTransactions => new GenericRepository<EquipmentTransaction>(_context);
    public IGenericRepository<MaintenanceLog> MaintenanceLogs => new GenericRepository<MaintenanceLog>(_context);
    public IGenericRepository<Depreciation> Depreciations => new GenericRepository<Depreciation>(_context);
    public IGenericRepository<EquipmentCategory> EquipmentCategories => new GenericRepository<EquipmentCategory>(_context);
    public IGenericRepository<Provider> Providers => new GenericRepository<Provider>(_context);
    public IGenericRepository<IncidentLog> IncidentLogs => new GenericRepository<IncidentLog>(_context);
    public IGenericRepository<EquipmentProviderHistory> EquipmentProviderHistories => new GenericRepository<EquipmentProviderHistory>(_context);
    public IGenericRepository<ProviderPayment> ProviderPayments => new GenericRepository<ProviderPayment>(_context);

    public IGenericRepository<AuditLog> AuditLogs => new GenericRepository<AuditLog>(_context);

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();

    public async Task CommitAsync() => await _context.Database.CommitTransactionAsync();

    public async Task RollbackAsync() => await _context.Database.RollbackTransactionAsync();
    
    public async Task ExecuteStrategyAsync(Func<Task> action)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(action);
    }

    public void Dispose() => _context.Dispose();
}
