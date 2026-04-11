using Gym.Application.Interfaces.Repositories;
using Gym.Domain.Entities;

namespace Gym.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IMemberRepository Members { get; }
    IPackageRepository Packages { get; }
    ISubscriptionRepository Subscriptions { get; }
    IPaymentRepository Payments { get; }
    ICheckInRepository CheckIns { get; }
    ITrainerRepository Trainers { get; }
    IClassRepository Classes { get; }
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IClassEnrollmentRepository ClassEnrollments { get; }
    IGenericRepository<TrainerMemberAssignment> TrainerMemberAssignments { get; }
    IGenericRepository<Product> Products { get; }
    IGenericRepository<Invoice> Invoices { get; }
    
    // Inventory module
    IGenericRepository<Warehouse> Warehouses { get; }
    IGenericRepository<Inventory> Inventories { get; }
    IGenericRepository<StockTransaction> StockTransactions { get; }
    IGenericRepository<StockAudit> StockAudits { get; }
    IGenericRepository<StockAuditDetail> StockAuditDetails { get; }
    IGenericRepository<Order> Orders { get; }
    IGenericRepository<OrderDetail> OrderDetails { get; }

    // Equipment module
    IGenericRepository<Equipment> Equipments { get; }
    IGenericRepository<EquipmentTransaction> EquipmentTransactions { get; }
    IGenericRepository<MaintenanceLog> MaintenanceLogs { get; }
    IGenericRepository<Depreciation> Depreciations { get; }
    IGenericRepository<EquipmentCategory> EquipmentCategories { get; }
    IGenericRepository<Provider> Providers { get; }
    IGenericRepository<IncidentLog> IncidentLogs { get; }
    IGenericRepository<EquipmentProviderHistory> EquipmentProviderHistories { get; }
    IGenericRepository<ProviderPayment> ProviderPayments { get; }

    IGenericRepository<AuditLog> AuditLogs { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task ExecuteStrategyAsync(Func<Task> action);
}
