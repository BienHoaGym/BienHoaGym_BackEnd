using Gym.Application.DTOs.Billing;
using Gym.Application.Interfaces.Services;
using Gym.Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Gym.Infrastructure.Services;

public class QuestPdfService : IPdfService
{
    public QuestPdfService()
    {
        // QuestPDF License
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateInvoicePdf(InvoiceDto invoice)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Verdana));

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("BIÊN HÒA GYM").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                        col.Item().Text("Hệ thống Quản lý Gym Chuyên nghiệp").FontSize(9).Italic();
                    });

                    row.RelativeItem().AlignRight().Column(col =>
                    {
                        col.Item().Text($"Số HĐ: {invoice.InvoiceNumber}").SemiBold();
                        col.Item().Text($"Ngày: {invoice.CreatedAt.ToLocalTime():dd/MM/yyyy HH:mm}");
                    });
                });

                page.Content().PaddingVertical(10).Column(col =>
                {
                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    
                    col.Item().PaddingTop(5).Row(row => {
                        row.RelativeItem().Column(c => {
                            c.Item().Text(t => {
                                t.Span("Khách hàng: ").SemiBold();
                                t.Span(invoice.MemberName ?? "Khách lẻ");
                            });
                            if (!string.IsNullOrEmpty(invoice.CreatedByUserName))
                            {
                                c.Item().DefaultTextStyle(x => x.FontSize(9)).Text(t => {
                                    t.Span("Nhân viên: ").SemiBold();
                                    t.Span(invoice.CreatedByUserName);
                                });
                            }
                        });
                        row.RelativeItem().AlignRight().Text(t => {
                            t.Span("PTTT: ").SemiBold();
                            t.Span(TranslatePaymentMethod(invoice.PaymentMethod));
                        });
                    });

                    col.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Sản phẩm/Dịch vụ");
                            header.Cell().Element(CellStyle).AlignCenter().Text("SL");
                            header.Cell().Element(CellStyle).AlignRight().Text("Đơn giá");
                            header.Cell().Element(CellStyle).AlignRight().Text("Thành tiền");

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                            }
                        });

                        foreach (var detail in invoice.Details)
                        {
                            table.Cell().Element(CellStyle).Text(detail.ItemName);
                            table.Cell().Element(CellStyle).AlignCenter().Text(detail.Quantity.ToString());
                            table.Cell().Element(CellStyle).AlignRight().Text(FormatMoney(detail.UnitPrice));
                            table.Cell().Element(CellStyle).AlignRight().Text(FormatMoney(detail.Subtotal));

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                            }
                        }
                    });

                    col.Item().AlignRight().PaddingTop(10).Column(c =>
                    {
                        c.Item().Text(t =>
                        {
                            t.Span("Tổng tiền: ").SemiBold();
                            t.Span(FormatMoney(invoice.TotalAmount));
                        });
                        
                        if (invoice.DiscountAmount > 0)
                        {
                            c.Item().Text(t =>
                            {
                                t.Span("Giảm giá: ").SemiBold().FontColor(Colors.Red.Medium);
                                t.Span($"-{FormatMoney(invoice.DiscountAmount)}").FontColor(Colors.Red.Medium);
                            });
                        }

                        c.Item().PaddingTop(5).Text(t =>
                        {
                            t.Span("THANH TOÁN: ").FontSize(14).SemiBold().FontColor(Colors.Blue.Medium);
                            t.Span(FormatMoney(invoice.FinalAmount)).FontSize(14).SemiBold().FontColor(Colors.Blue.Medium);
                        });
                    });
                    
                    if (!string.IsNullOrEmpty(invoice.Note))
                    {
                        col.Item().PaddingTop(10).DefaultTextStyle(x => x.FontSize(9).Italic()).Text(t => {
                            t.Span("Ghi chú: ").SemiBold();
                            t.Span(invoice.Note);
                        });
                    }
                });

                page.Footer().AlignCenter().PaddingTop(20).Column(col => {
                    col.Item().Text("Cảm ơn quý khách đã tin dùng dịch vụ của chúng tôi!").Italic().AlignCenter();
                    col.Item().Text("Hẹn gặp lại!").Italic().AlignCenter();
                });
            });
        }).GeneratePdf();
    }

    private string FormatMoney(decimal amount)
    {
        return amount.ToString("N0") + " ₫";
    }

    private string TranslatePaymentMethod(PaymentMethod method)
    {
        return method switch
        {
            PaymentMethod.Cash => "Tiền mặt",
            PaymentMethod.BankTransfer => "Chuyển khoản",
            PaymentMethod.EWallet => "Ví điện tử",
            PaymentMethod.Card => "Thẻ",
            _ => "Khác"
        };
    }
}
