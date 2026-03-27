namespace Gym.Application.DTOs.Providers;

public class ProviderDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? TaxCode { get; set; }
    public string? SupplyType { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankName { get; set; }
    public string? Note { get; set; }
    public bool IsActive { get; set; }
}

public class CreateProviderDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? TaxCode { get; set; }
    public string? SupplyType { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankName { get; set; }
    public string? Note { get; set; }
    public bool IsActive { get; set; } = true;
}

public class ProviderSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? ContactPerson { get; set; }
    public string? TaxCode { get; set; }
    public string? SupplyType { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public int EquipmentCount { get; set; }
    public int ProductCount { get; set; }
}

public class ProviderTransactionHistoryDto
{
    public Guid Id { get; set; }
    public string TransactionCode { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Purchase, Maintenance, Contract
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
}
