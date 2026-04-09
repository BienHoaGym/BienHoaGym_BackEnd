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
        // 1. Lấy sản phẩm (Bỏ qua Service vì Service giờ được quản lý ở bảng Packages)
        var products = await _unitOfWork.Products.GetQueryable()
            .Where(p => !p.IsDeleted && p.IsActive && p.Type != ProductType.Service)
            .Include(p => p.Provider)
            .AsNoTracking()
            .ToListAsync();
            
        // 2. Lấy Gói tập (Packages) để gộp chung vào danh mục POS
        var packages = await _unitOfWork.Packages.GetQueryable()
            .Where(p => !p.IsDeleted && p.IsActive)
            .AsNoTracking()
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        var packageDtos = _mapper.Map<List<ProductDto>>(packages);
        
        // Trộn chung vào một danh sách duy nhất cho POS dễ xử lý
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
            invoice.InvoiceNumber = $"HD-{DateTime.UtcNow:yyyyMMdd}-{(count + 1):D4}";

            // Prepare Details
            decimal total = 0;
            
            // --- XỬ LÝ CHẶN BÁN/GIA HẠN GÓI TẬP (BACKEND BUSINESS LOGIC) ---
            DateTime? nextStartDate = null;
            if (dto.MemberId.HasValue)
            {
                var packageItems = dto.Details.Where(d => d.ItemType == "Package" && !d.SubscriptionId.HasValue).ToList();
                if (packageItems.Any())
                {
                    // 1. Chặn nếu trong giỏ hàng có hơn 1 gói tập mới
                    if (packageItems.Count > 1)
                    {
                        return ResponseDto<InvoiceDto>.FailureResult("Mỗi hóa đơn chỉ được phép đăng ký tối đa 1 gói tập mới. Vui lòng tách hóa đơn hoặc kiểm tra lại giỏ hàng.");
                    }

                    // 2. Kiểm tra gói hiện tại để tính toán ngày bắt đầu (Stacking/Gia hạn)
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
                        // Nếu đang tạm dừng (Suspended), chặn không cho mua gói mới
                        if (activeOrSuspendedSub.Status == SubscriptionStatus.Suspended)
                        {
                            return ResponseDto<InvoiceDto>.FailureResult("Hội viên đang bị tạm dừng gói tập. Vui lòng kích hoạt lại gói tập cũ trước khi mua gói mới.");
                        }

                        // Nếu đang có gói chờ thanh toán (Pending), yêu cầu xử lý gói đó trước
                        if (activeOrSuspendedSub.Status == SubscriptionStatus.Pending)
                        {
                            return ResponseDto<InvoiceDto>.FailureResult("Hội viên đang có một giao dịch đăng ký đang chờ thanh toán. Vui lòng thanh toán hoặc hủy giao dịch đó trước khi bán gói mới.");
                        }

                        // Nếu đang có gói Active, kiểm tra thời hạn còn lại (Nghiệp vụ: Chỉ cho gia hạn nếu còn 1 ngày)
                        if (activeOrSuspendedSub.Status == SubscriptionStatus.Active)
                        {
                            if (activeOrSuspendedSub.EndDate >= today)
                            {
                                var daysLeft = (activeOrSuspendedSub.EndDate - today).Days;
                                if (daysLeft > 1)
                                {
                                    return ResponseDto<InvoiceDto>.FailureResult($"Gói tập hiện tại ('{activeOrSuspendedSub.OriginalPackageName}') vẫn còn hạn đến {activeOrSuspendedSub.EndDate:dd/MM/yyyy} ({daysLeft} ngày). Hệ thống chỉ cho phép gia hạn khi gói tập sắp hết hạn trong vòng 1 ngày. Nếu muốn đổi gói ngay, vui lòng Hủy gói cũ tại hồ sơ hội viên.");
                                }

                                // Nếu còn <= 1 ngày, tính ngày bắt đầu mới là ngày sau ngày hết hạn gói cũ
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
                        // 1. Cập nhật tồn kho tổng (Global)
                        product.StockQuantity -= d.Quantity;
                        _unitOfWork.Products.Update(product);

                        // 2. Cập nhật tồn kho theo kho (Warehouse Inventory)
                        // Mặc định trừ vào "Kho Quầy" (ID: 20000000-0000-0000-0000-000000000002)
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
                            // Nếu chưa có trong kho quầy, tạo mới để ghi nhận (có thể âm nếu bán vượt)
                            var newInv = new Inventory
                            {
                                ProductId = product.Id,
                                WarehouseId = counterWarehouseId,
                                Quantity = -d.Quantity
                            };
                            await _unitOfWork.Inventories.AddAsync(newInv);
                        }

                        // 3. Ghi nhận lịch sử giao dịch kho (Stock Transaction)
                        var stockTx = new StockTransaction
                        {
                            ProductId = product.Id,
                            FromWarehouseId = counterWarehouseId,
                            Quantity = d.Quantity,
                            Type = StockTransactionType.Export, // Xuất bán
                            Date = DateTime.UtcNow,
                            Note = $"Xuất bán tự động từ hóa đơn {invoice.InvoiceNumber}"
                        };
                        await _unitOfWork.StockTransactions.AddAsync(stockTx);
                    }
                }

                // --- XỬ LÝ GÓI TẬP ---
                // Case 1: Thanh toán gói Pending đã có sẵn (Dựa vào SubscriptionId)
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
                // Case 2: Đăng ký gói MỚI (Dựa vào ItemType là Package và KO CÓ SubscriptionId)
                else if (d.ItemType == "Package" && d.ReferenceId.HasValue && dto.MemberId.HasValue)
                {
                    var startDate = nextStartDate ?? DateTime.UtcNow;
                    var subscription = new MemberSubscription
                    {
                        MemberId = dto.MemberId.Value,
                        PackageId = d.ReferenceId.Value,
                        StartDate = startDate,
                        EndDate = startDate.AddDays(30),
                        Status = SubscriptionStatus.Active, // Bán tại POS là Active luôn (hoặc chờ tới ngày start)
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
            
            // NGHIỆP VỤ: Khi đã có gói tập được kích hoạt/tạo mới, cập nhật trạng thái Hội viên là Active
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

            return ResponseDto<InvoiceDto>.SuccessResult(_mapper.Map<InvoiceDto>(invoice), "Tạo hóa đơn thành công");
        }
        catch (Exception ex)
        {
            return ResponseDto<InvoiceDto>.FailureResult($"Lỗi khi tạo hóa đơn: {ex.Message}");
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
            
        if (invoice == null) return ResponseDto<InvoiceDto>.FailureResult("Không tìm thấy hóa đơn");
        
        return ResponseDto<InvoiceDto>.SuccessResult(_mapper.Map<InvoiceDto>(invoice));
    }
}
