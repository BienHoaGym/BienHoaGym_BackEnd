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
        // 1. Láº¥y sáº£n pháº©m (Bá» qua Service vÃ¬ Service giá» Ä‘Æ°á»£c quáº£n lÃ½ á»Ÿ báº£ng Packages)
        var products = await _unitOfWork.Products.GetQueryable()
            .Where(p => !p.IsDeleted && p.IsActive && p.Type != ProductType.Service)
            .Include(p => p.Provider)
            .AsNoTracking()
            .ToListAsync();
            
        // 2. Láº¥y GÃ³i táº­p (Packages) Ä‘á»ƒ gá»™p chung vÃ o danh má»¥c POS
        var packages = await _unitOfWork.Packages.GetQueryable()
            .Where(p => !p.IsDeleted && p.IsActive)
            .AsNoTracking()
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        var packageDtos = _mapper.Map<List<ProductDto>>(packages);
        
        // Trá»™n chung vÃ o má»™t danh sÃ¡ch duy nháº¥t cho POS dá»… xá»­ lÃ½
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

            // Generate Invoice Number - Consider using a sequence in DB for production
            var count = await _unitOfWork.Invoices.GetQueryable().CountAsync();
            invoice.InvoiceNumber = $"HD-{DateTime.UtcNow:yyyyMMdd}-{(count + 1):D4}";

            // Prepare Details
            decimal total = 0;
            
            // --- Xá»¬ LÃ CHáº¶N BÃN/GIA Háº N GÃ“I Táº¬P (BACKEND BUSINESS LOGIC) ---
            DateTime? nextStartDate = null;
            if (dto.MemberId.HasValue)
            {
                var packageItems = dto.Details.Where(d => d.ItemType == "Package" && !d.SubscriptionId.HasValue).ToList();
                if (packageItems.Any())
                {
                    // 1. Cháº·n náº¿u trong giá» hÃ ng cÃ³ hÆ¡n 1 gÃ³i táº­p má»›i
                    if (packageItems.Count > 1)
                    {
                        return ResponseDto<InvoiceDto>.FailureResult("Má»—i hÃ³a Ä‘Æ¡n chá»‰ Ä‘Æ°á»£c phÃ©p Ä‘Äƒng kÃ½ tá»‘i Ä‘a 1 gÃ³i táº­p má»›i. Vui lÃ²ng tÃ¡ch hÃ³a Ä‘Æ¡n hoáº·c kiá»ƒm tra láº¡i giá» hÃ ng.");
                    }

                    // 2. Kiá»ƒm tra gÃ³i hiá»‡n táº¡i Ä‘á»ƒ tÃ­nh toÃ¡n ngÃ y báº¯t Ä‘áº§u (Stacking/Gia háº¡n)
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
                        // Náº¿u Ä‘ang táº¡m dá»«ng (Suspended), cháº·n khÃ´ng cho mua gÃ³i má»›i
                        if (activeOrSuspendedSub.Status == SubscriptionStatus.Suspended)
                        {
                            return ResponseDto<InvoiceDto>.FailureResult("Há»™i viÃªn Ä‘ang bá»‹ táº¡m dá»«ng gÃ³i táº­p. Vui lÃ²ng kÃ­ch hoáº¡t láº¡i gÃ³i táº­p cÅ© trÆ°á»›c khi mua gÃ³i má»›i.");
                        }

                        // Náº¿u Ä‘ang cÃ³ gÃ³i chá» thanh toÃ¡n (Pending), yÃªu cáº§u xá»­ lÃ½ gÃ³i Ä‘Ã³ trÆ°á»›c
                        if (activeOrSuspendedSub.Status == SubscriptionStatus.Pending)
                        {
                            return ResponseDto<InvoiceDto>.FailureResult("Há»™i viÃªn Ä‘ang cÃ³ má»™t giao dá»‹ch Ä‘Äƒng kÃ½ Ä‘ang chá» thanh toÃ¡n. Vui lÃ²ng thanh toÃ¡n hoáº·c há»§y giao dá»‹ch Ä‘Ã³ trÆ°á»›c khi bÃ¡n gÃ³i má»›i.");
                        }

                        // Náº¿u Ä‘ang cÃ³ gÃ³i Active, kiá»ƒm tra thá»i háº¡n cÃ²n láº¡i (Nghiá»‡p vá»¥: Chá»‰ cho gia háº¡n náº¿u cÃ²n 1 ngÃ y)
                        if (activeOrSuspendedSub.Status == SubscriptionStatus.Active)
                        {
                            if (activeOrSuspendedSub.EndDate >= today)
                            {
                                var daysLeft = (activeOrSuspendedSub.EndDate - today).Days;
                                if (daysLeft > 1)
                                {
                                    return ResponseDto<InvoiceDto>.FailureResult($"GÃ³i táº­p hiá»‡n táº¡i ('{activeOrSuspendedSub.OriginalPackageName}') váº«n cÃ²n háº¡n Ä‘áº¿n {activeOrSuspendedSub.EndDate:dd/MM/yyyy} ({daysLeft} ngÃ y). Há»‡ thá»‘ng chá»‰ cho phÃ©p gia háº¡n khi gÃ³i táº­p sáº¯p háº¿t háº¡n trong vÃ²ng 1 ngÃ y. Náº¿u muá»‘n Ä‘á»•i gÃ³i ngay, vui lÃ²ng Há»§y gÃ³i cÅ© táº¡i há»“ sÆ¡ há»™i viÃªn.");
                                }

                                // Náº¿u cÃ²n <= 1 ngÃ y, tÃ­nh ngÃ y báº¯t Ä‘áº§u má»›i lÃ  ngÃ y sau ngÃ y háº¿t háº¡n gÃ³i cÅ©
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
                        // 1. Cáº­p nháº­t tá»“n kho tá»•ng (Global)
                        product.StockQuantity -= d.Quantity;
                        _unitOfWork.Products.Update(product);

                        // 2. Cáº­p nháº­t tá»“n kho theo kho (Warehouse Inventory)
                        // Máº·c Ä‘á»‹nh trá»« vÃ o "Kho Quáº§y" (ID: 20000000-0000-0000-0000-000000000002)
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
                            // Náº¿u chÆ°a cÃ³ trong kho quáº§y, táº¡o má»›i Ä‘á»ƒ ghi nháº­n (cÃ³ thá»ƒ Ã¢m náº¿u bÃ¡n vÆ°á»£t)
                            var newInv = new Inventory
                            {
                                ProductId = product.Id,
                                WarehouseId = counterWarehouseId,
                                Quantity = -d.Quantity
                            };
                            await _unitOfWork.Inventories.AddAsync(newInv);
                        }

                        // 3. Ghi nháº­n lá»‹ch sá»­ giao dá»‹ch kho (Stock Transaction)
                        var stockTx = new StockTransaction
                        {
                            ProductId = product.Id,
                            FromWarehouseId = counterWarehouseId,
                            Quantity = d.Quantity,
                            Type = StockTransactionType.Export, // Xuáº¥t bÃ¡n
                            Date = DateTime.UtcNow,
                            Note = $"Xuáº¥t bÃ¡n tá»± Ä‘á»™ng tá»« hÃ³a Ä‘Æ¡n {invoice.InvoiceNumber}"
                        };
                        await _unitOfWork.StockTransactions.AddAsync(stockTx);
                    }
                }

                // --- Xá»¬ LÃ GÃ“I Táº¬P ---
                // Case 1: Thanh toÃ¡n gÃ³i Pending Ä‘Ã£ cÃ³ sáºµn (Dá»±a vÃ o SubscriptionId)
                if (d.SubscriptionId.HasValue)
                {
                    var existingSub = await _unitOfWork.Subscriptions.GetByIdAsync(d.SubscriptionId.Value);
                    if (existingSub != null && existingSub.Status == SubscriptionStatus.Pending)
                    {
                        existingSub.Status = SubscriptionStatus.Active;
                        existingSub.UpdatedAt = DateTime.UtcNow;
                        _unitOfWork.Subscriptions.Update(existingSub);
                    }
                }
                // Case 2: ÄÄƒng kÃ½ gÃ³i Má»šI (Dá»±a vÃ o ItemType lÃ  Package vÃ  KO CÃ“ SubscriptionId)
                else if (d.ItemType == "Package" && d.ReferenceId.HasValue && dto.MemberId.HasValue)
                {
                    var startDate = nextStartDate ?? DateTime.UtcNow;
                    var subscription = new MemberSubscription
                    {
                        MemberId = dto.MemberId.Value,
                        PackageId = d.ReferenceId.Value,
                        StartDate = startDate,
                        EndDate = startDate.AddDays(30),
                        Status = SubscriptionStatus.Active, // BÃ¡n táº¡i POS lÃ  Active luÃ´n (hoáº·c chá» tá»›i ngÃ y start)
                        CreatedAt = DateTime.UtcNow,
                        // Snapshot data
                        OriginalPackageName = d.ItemName,
                        OriginalPrice = d.UnitPrice,
                        FinalPrice = d.UnitPrice
                    };
                    
                    var pkg = await _unitOfWork.Packages.GetByIdAsync(d.ReferenceId.Value);
                    if (pkg != null)
                    {
                        subscription.EndDate = startDate.AddDays(pkg.DurationDays);
                        subscription.OriginalPackageName = pkg.Name;
                    }

                    await _unitOfWork.Subscriptions.AddAsync(subscription);
                }
            }

            invoice.TotalAmount = total;
            
            // NGHIá»†P Vá»¤: Khi Ä‘Ã£ cÃ³ gÃ³i táº­p Ä‘Æ°á»£c kÃ­ch hoáº¡t/táº¡o má»›i, cáº­p nháº­t tráº¡ng thÃ¡i Há»™i viÃªn lÃ  Active
            if (dto.MemberId.HasValue && (invoice.Details.Any(d => d.ItemType == "Package") || invoice.Details.Any(d => d.SubscriptionId.HasValue)))
            {
                var member = await _unitOfWork.Members.GetByIdAsync(dto.MemberId.Value);
                if (member != null)
                {
                    member.Status = MemberStatus.Active;
                    _unitOfWork.Members.Update(member);
                }
            }
            
            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync(); // Atomic operation

            return ResponseDto<InvoiceDto>.SuccessResult(_mapper.Map<InvoiceDto>(invoice), "Táº¡o hÃ³a Ä‘Æ¡n thÃ nh cÃ´ng");
        }
        catch (Exception ex)
        {
            return ResponseDto<InvoiceDto>.FailureResult($"Lá»—i khi táº¡o hÃ³a Ä‘Æ¡n: {ex.Message}");
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
            
        if (invoice == null) return ResponseDto<InvoiceDto>.FailureResult("KhÃ´ng tÃ¬m tháº¥y hÃ³a Ä‘Æ¡n");
        
        return ResponseDto<InvoiceDto>.SuccessResult(_mapper.Map<InvoiceDto>(invoice));
    }
}
