using Gym.Application.DTOs.Billing;

namespace Gym.Application.Interfaces.Services;

public interface IPdfService
{
    byte[] GenerateInvoicePdf(InvoiceDto invoice);
}
