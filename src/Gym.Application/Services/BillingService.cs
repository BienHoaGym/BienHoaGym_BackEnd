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
        var products = await _unitOfWork.Products.GetQueryable()
            .Where(p => !p.IsDeleted && p.IsActive && p.Type != ProductType.Service)
            .Include(p => p.Provider)
            .AsNoTracking()
            .ToListAsync();
            
        var packages = await _unitOfWork.Packages.GetQueryable()
            .Where(p => !p.IsDeleted && p.IsActive)
            .AsNoTracking()
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        var packageDtos = _mapper.Map<List<ProductDto>>(packages);
        productDtos.AddRange(packageDtos);
            
        return ResponseDto<List<ProductDto>>.SuccessResult(productDtos);
    }

    public async Task<ResponseDto<ProductDto>> CreateProductAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<ProductDto>.SuccessResult(_mapper.Map<ProductDto>(product));
    }

    public async Task<ResponseDto<InvoiceDto>> CreateInvoiceAsync(CreateInvoiceDto dto, Guid? userId = null, string? userName = null)
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
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = userId,
                CreatedByUserName = userName
            };

            var count = await _unitOfWork.Invoices.GetQueryable().CountAsync();
            invoice.InvoiceNumber = $"HD-{DateTime.UtcNow:yyyyMMdd}-{(count + 1):D4}";

            decimal total = 0;
            DateTime? nextStartDate = null;
            if (dto.MemberId.HasValue)
            {
                var packageItems = dto.Details.Where(d => d.ItemType == "Package" && !d.SubscriptionId.HasValue).ToList();
                if (packageItems.Any())
                {
                    if (packageItems.Count > 1)
                    {
                        return ResponseDto<InvoiceDto>.FailureResult("M\u1ED7i h\u00F3a \u0111\u01A1n ch\u1EC9 \u0111\u01B0\u1EE3c ph\u00E9p \u0111\u0103ng k\u00FD t\u1ED1i \u0111a 1 g\u00F3i t\u1EADp m\u1EDBi.");
                    }

                    var today = DateTime.UtcNow.Date;
                    var activeOrSuspendedSub = await _unitOfWork.Subscriptions.GetQueryable()
                        .Where(s => s.MemberId == dto.MemberId.Value && !s.IsDeleted && 
                                   (s.Status == SubscriptionStatus.Active || 
                                    s.Status == SubscriptionStatus.Pending ||
                                    s.Status == SubscriptionStatus.Suspended))
                        .OrderByDescending(s => s.EndDate)
                        .FirstOrDefaultAsync();
                    
                    if (activeOrSuspendedSub != null)
                    {
                        if (activeOrSuspendedSub.Status == SubscriptionStatus.Suspended)
                        {
                            return ResponseDto<InvoiceDto>.FailureResult("H\u1ED9i vi\u00EAn \u0111ang b\u1ECB t\u1EA1m d\u1EEBng g\u00F3i t\u1EADp.");
                        }

                        if (activeOrSuspendedSub.Status == SubscriptionStatus.Pending)
                        {
                            return ResponseDto<InvoiceDto>.FailureResult("H\u1ED9i vi\u00EAn \u0111ang c\u00F3 giao d\u1ECBch \u0111ang ch\u1EDD thanh to\u00E1n.");
                        }

                        if (activeOrSuspendedSub.Status == SubscriptionStatus.Active)
                        {
                            if (activeOrSuspendedSub.EndDate >= today)
                            {
                                var daysLeft = (activeOrSuspendedSub.EndDate - today).Days;
                                if (daysLeft > 1)
                                {
                                    return ResponseDto<InvoiceDto>.FailureResult($"G\u00F3i t\u1EADp hi\u1EC7n t\u1EA1i v\u1EABn c\u00F2n h\u1EA1n \u0111\u1EBFn {activeOrSuspendedSub.EndDate:dd/MM/yyyy}.");
                                }
                                nextStartDate = activeOrSuspendedSub.EndDate.AddDays(1);
                            }
                        }
                    }
                }
            }

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

                        var counterWarehouseId = Guid.Parse("20000000-0000-0000-0000-000000000002");
                        var inventory = await _unitOfWork.Inventories.GetQueryable()
                            .FirstOrDefaultAsync(i => i.ProductId == product.Id && i.WarehouseId == counterWarehouseId);
                        
                        if (inventory != null)
                        {
                            inventory.Quantity -= d.Quantity;
                            _unitOfWork.Inventories.Update(inventory);
                        }
                        else
                        {
                            await _unitOfWork.Inventories.AddAsync(new Inventory { ProductId = product.Id, WarehouseId = counterWarehouseId, Quantity = -d.Quantity });
                        }

                        await _unitOfWork.StockTransactions.AddAsync(new StockTransaction {
                            ProductId = product.Id, FromWarehouseId = counterWarehouseId, Quantity = d.Quantity,
                            Type = StockTransactionType.Export, Date = DateTime.UtcNow,
                            Note = $"Xu\u1EA5t b\u00E1n t\u1EF1 \u0111\u1ED9ng t\u1EEB h\u00F3a \u0111\u01A1n {invoice.InvoiceNumber}"
                        });
                    }
                }

                if (d.SubscriptionId.HasValue)
                {
                    var existingSub = await _unitOfWork.Subscriptions.GetByIdAsync(d.SubscriptionId.Value);
                    if (existingSub != null && existingSub.Status == SubscriptionStatus.Pending)
                    {
                        existingSub.Status = SubscriptionStatus.Active;
                        _unitOfWork.Subscriptions.Update(existingSub);
                    }
                }
                else if (d.ItemType == "Package" && d.ReferenceId.HasValue && dto.MemberId.HasValue)
                {
                    var startDate = nextStartDate ?? DateTime.UtcNow;
                    var subscription = new MemberSubscription {
                        MemberId = dto.MemberId.Value, PackageId = d.ReferenceId.Value,
                        StartDate = startDate, EndDate = startDate.AddDays(30),
                        Status = SubscriptionStatus.Active, CreatedAt = DateTime.UtcNow,
                        OriginalPackageName = d.ItemName, OriginalPrice = d.UnitPrice, FinalPrice = d.UnitPrice
                    };
                    
                    var pkg = await _unitOfWork.Packages.GetByIdAsync(d.ReferenceId.Value);
                    if (pkg != null) {
                        subscription.EndDate = startDate.AddDays(pkg.DurationDays);
                        subscription.OriginalPackageName = pkg.Name;
                    }
                    await _unitOfWork.Subscriptions.AddAsync(subscription);
                }
            }

            invoice.TotalAmount = total;
            if (dto.MemberId.HasValue && (invoice.Details.Any(d => d.ItemType == "Package") || invoice.Details.Any(d => d.SubscriptionId.HasValue)))
            {
                var member = await _unitOfWork.Members.GetByIdAsync(dto.MemberId.Value);
                if (member != null) { member.Status = MemberStatus.Active; _unitOfWork.Members.Update(member); }
            }
            
            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();
            return ResponseDto<InvoiceDto>.SuccessResult(_mapper.Map<InvoiceDto>(invoice), "Th\u00E0nh c\u00F4ng");
        }
        catch (Exception ex) { return ResponseDto<InvoiceDto>.FailureResult($"L\u1ED7i: {ex.Message}"); }
    }

    public async Task<ResponseDto<List<InvoiceDto>>> GetInvoicesAsync()
    {
        var invoices = await _unitOfWork.Invoices.GetQueryable().Include(i => i.Member).Include(i => i.Details).OrderByDescending(i => i.CreatedAt).ToListAsync();
        return ResponseDto<List<InvoiceDto>>.SuccessResult(_mapper.Map<List<InvoiceDto>>(invoices));
    }

    public async Task<ResponseDto<InvoiceDto>> GetInvoiceByIdAsync(Guid id)
    {
        var invoice = await _unitOfWork.Invoices.GetQueryable().Include(i => i.Member).Include(i => i.Details).FirstOrDefaultAsync(i => i.Id == id);
        if (invoice == null) return ResponseDto<InvoiceDto>.FailureResult("Kh\u00F4ng t\u00ECm th\u1EA5y h\u00F3a \u0111\u01A1n");
        return ResponseDto<InvoiceDto>.SuccessResult(_mapper.Map<InvoiceDto>(invoice));
    }
}
