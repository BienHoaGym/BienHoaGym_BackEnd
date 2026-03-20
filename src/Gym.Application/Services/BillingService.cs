using AutoMapper;
using Gym.Application.DTOs.Billing;
using Gym.Application.DTOs.Common;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Services;

public class BillingService : IBillingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BillingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto<List<ProductDto>>> GetProductsAsync()
    {
        var products = await _unitOfWork.Products.FindAsync(p => !p.IsDeleted);
        return ResponseDto<List<ProductDto>>.SuccessResult(_mapper.Map<List<ProductDto>>(products));
    }

    public async Task<ResponseDto<ProductDto>> CreateProductAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<ProductDto>.SuccessResult(_mapper.Map<ProductDto>(product));
    }

    public async Task<ResponseDto<InvoiceDto>> CreateInvoiceAsync(CreateInvoiceDto dto)
    {
        try
        {
            var invoice = new Invoice
            {
                MemberId = dto.MemberId,
                PaymentMethod = dto.PaymentMethod,
                DiscountAmount = dto.DiscountAmount,
                Note = dto.Note,
                Status = PaymentStatus.Completed,
                CreatedAt = DateTime.UtcNow
            };

            // Generate Invoice Number - Consider using a sequence in DB for production
            var count = await _unitOfWork.Invoices.GetQueryable().CountAsync();
            invoice.InvoiceNumber = $"HD-{DateTime.Now:yyyyMMdd}-{(count + 1):D4}";

            decimal total = 0;
            foreach (var d in dto.Details)
            {
                var detail = _mapper.Map<InvoiceDetail>(d);
                detail.UnitPrice = d.UnitPrice;
                detail.Quantity = d.Quantity;
                total += detail.Subtotal;
                
                invoice.Details.Add(detail);

                if (d.ItemType == "Product" && d.ReferenceId.HasValue)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(d.ReferenceId.Value);
                    if (product != null)
                    {
                        product.StockQuantity -= d.Quantity;
                        _unitOfWork.Products.Update(product);
                    }
                }

                // Handle Subscriptions if Package
                if (d.ItemType == "Package")
                {
                    if (d.SubscriptionId.HasValue)
                    {
                        // CASE 1: Paying for an existing PENDING subscription
                        var existingSub = await _unitOfWork.Subscriptions.GetByIdAsync(d.SubscriptionId.Value);
                        if (existingSub != null && existingSub.Status == SubscriptionStatus.Pending)
                        {
                            existingSub.Status = SubscriptionStatus.Active;
                            existingSub.UpdatedAt = DateTime.UtcNow;
                            _unitOfWork.Subscriptions.Update(existingSub);
                        }
                    }
                    else if (d.ReferenceId.HasValue && dto.MemberId.HasValue)
                    {
                        // CASE 2: Buying a NEW package at the counter
                        var subscription = new MemberSubscription
                        {
                            MemberId = dto.MemberId.Value,
                            PackageId = d.ReferenceId.Value,
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddDays(30),
                            Status = SubscriptionStatus.Active,
                            CreatedAt = DateTime.UtcNow,
                            // Snapshot data
                            OriginalPackageName = d.ItemName,
                            OriginalPrice = d.UnitPrice,
                            FinalPrice = d.UnitPrice
                        };
                        
                        var pkg = await _unitOfWork.Packages.GetByIdAsync(d.ReferenceId.Value);
                        if (pkg != null)
                        {
                            subscription.EndDate = DateTime.UtcNow.AddDays(pkg.DurationInDays);
                            subscription.OriginalPackageName = pkg.Name;
                        }

                        await _unitOfWork.Subscriptions.AddAsync(subscription);
                    }
                }
            }

            invoice.TotalAmount = total;
            
            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync(); // Atomic operation

            return ResponseDto<InvoiceDto>.SuccessResult(_mapper.Map<InvoiceDto>(invoice), "Invoice created successfully");
        }
        catch (Exception ex)
        {
            return ResponseDto<InvoiceDto>.FailureResult($"Error creating invoice: {ex.Message}");
        }
    }

    public async Task<ResponseDto<List<InvoiceDto>>> GetInvoicesAsync()
    {
        var invoices = await _unitOfWork.Invoices.GetQueryable()
            .Include(i => i.Member)
            .Include(i => i.Details)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
            
        return ResponseDto<List<InvoiceDto>>.SuccessResult(_mapper.Map<List<InvoiceDto>>(invoices));
    }

    public async Task<ResponseDto<InvoiceDto>> GetInvoiceByIdAsync(Guid id)
    {
        var invoice = await _unitOfWork.Invoices.GetQueryable()
            .Include(i => i.Member)
            .Include(i => i.Details)
            .FirstOrDefaultAsync(i => i.Id == id);
            
        if (invoice == null) return ResponseDto<InvoiceDto>.FailureResult("Invoice not found");
        
        return ResponseDto<InvoiceDto>.SuccessResult(_mapper.Map<InvoiceDto>(invoice));
    }
}
