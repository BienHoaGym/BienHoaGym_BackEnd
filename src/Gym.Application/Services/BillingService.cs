using AutoMapper;
using Gym.Application.DTOs.Billing;
using Gym.Application.DTOs.Common;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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

    public async Task<byte[]> ExportInvoicePdfAsync(Guid id)
    {
        var invoice = await _unitOfWork.Invoices.GetQueryable()
            .Include(i => i.Member)
            .Include(i => i.Details)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null) return Array.Empty<byte>();

        // Gi\u1EA3 l\u1EADp PDF generator b\u1EB1ng QuestPDF (Y\u00EAu c\u1EA7u QuestPDF package)
        // Lưu ý: Trong thực tế bạn cần QuestPDF.Settings.License = LicenseType.Community; ở Program.cs
        
        using var ms = new System.IO.MemoryStream();
        
        try {
            QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("BIEN HOA GYM").FontSize(20).SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Medium);
                            col.Item().Text("S\u1ED1 123, \u0110\u01B0\u1EDDng \u0110\u1ED3ng Kh\u1EDFi, Bi\u00EAn H\u00F2a").FontSize(10);
                            col.Item().Text("Hotline: 0900 123 456").FontSize(10);
                        });

                        row.RelativeItem().AlignRight().Column(col =>
                        {
                            col.Item().Text("HO\u00C1 \u0110\u01A0N THANH TO\u00C1N").FontSize(16).Bold();
                            col.Item().Text($"S\u1ED1: {invoice.InvoiceNumber}");
                            col.Item().Text($"Ng\u00E0y: {invoice.CreatedAt:dd/MM/yyyy HH:mm}");
                        });
                    });

                    page.Content().PaddingVertical(20).Column(col =>
                    {
                        col.Item().PaddingBottom(10).Text(t =>
                        {
                            t.Span("Kh\u00E1ch h\u00E0ng: ").Bold();
                            t.Span(invoice.Member?.FullName ?? "Kh\u00E1ch l\u1EBB");
                        });

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn();
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("STT");
                                header.Cell().Text("N\u1ED9i dung");
                                header.Cell().Text("SL");
                                header.Cell().AlignRight().Text("\u0110\u01A1n gi\u00E1");
                                header.Cell().AlignRight().Text("Th\u00E0nh ti\u1EC1n");
                                
                                header.Cell().ColumnSpan(5).PaddingVertical(5).BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black);
                            });

                            int i = 1;
                            foreach (var detail in invoice.Details)
                            {
                                table.Cell().Text(i++.ToString());
                                table.Cell().Text(detail.ItemName);
                                table.Cell().Text(detail.Quantity.ToString());
                                table.Cell().AlignRight().Text(detail.UnitPrice.ToString("N0") + " \u0111");
                                table.Cell().AlignRight().Text(detail.Subtotal.ToString("N0") + " \u0111");
                            }
                        });

                        col.Item().AlignRight().PaddingTop(20).Column(c =>
                        {
                            c.Item().Text(t => {
                                t.Span("T\u1ED5ng c\u1ED9ng: ").FontSize(12);
                                t.Span(invoice.TotalAmount.ToString("N0") + " \u0111").FontSize(12).Bold();
                            });
                            if (invoice.DiscountAmount > 0) {
                                c.Item().Text(t => {
                                    t.Span("Gi\u1EA3m gi\u00E1: ").FontSize(10);
                                    t.Span("-" + invoice.DiscountAmount.ToString("N0") + " \u0111").FontSize(10);
                                });
                            }
                            c.Item().Text(t => {
                                t.Span("THANH TO\u00C1N: ").FontSize(14).Bold().FontColor(QuestPDF.Helpers.Colors.Red.Medium);
                                t.Span(invoice.FinalAmount.ToString("N0") + " \u0111").FontSize(14).Bold().FontColor(QuestPDF.Helpers.Colors.Red.Medium);
                            });
                            c.Item().PaddingTop(5).Text($"PTTT: {invoice.PaymentMethod}").FontSize(10).Italic();
                        });
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("C\u1EA3m \u01A1n Qu\u00FD kh\u00E1ch \u0111\u00E3 s\u1EED d\u1EE5ng d\u1ECBch v\u1EE5 t\u1EA1i BIEN HOA GYM!").Italic();
                    });
                });
            }).GeneratePdf(ms);
        } catch (Exception ex) {
            Console.WriteLine($"PDF Error: {ex.Message}");
            // Tr\u1EA3 v\u1EC1 stream r\u1ED7ng n\u1EBFu l\u1ED7i
        }

        return ms.ToArray();
    }
}
