using Gym.Application.DTOs.Common;
using Gym.Application.DTOs.Billing;

namespace Gym.Application.Interfaces.Services;

public interface IBillingService
{
    // Products
    Task<ResponseDto<List<ProductDto>>> GetProductsAsync();
    Task<ResponseDto<ProductDto>> CreateProductAsync(CreateProductDto dto);
    
    // Invoices
    Task<ResponseDto<InvoiceDto>> CreateInvoiceAsync(CreateInvoiceDto dto);
    Task<ResponseDto<List<InvoiceDto>>> GetInvoicesAsync();
    Task<ResponseDto<InvoiceDto>> GetInvoiceByIdAsync(Guid id);
}
