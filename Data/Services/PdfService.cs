using CleaningApp.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CleaningApp.Data.Services
{
    public class PdfService
    {
        public byte[] GenerateOrderPdf(Order order)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text($"Zlecenie #{order.Id}").SemiBold().FontSize(20).FontColor(Colors.Indigo.Medium);
                                col.Item().Text($"Data: {order.OrderDate:dd.MM.yyyy}");
                                col.Item().Text(order.Status == OrderStatus.Zakończone ? "STATUS: WYKONANO" : "STATUS: W TOKU")
                                   .FontColor(order.Status == OrderStatus.Zakończone ? Colors.Green.Medium : Colors.Orange.Medium).Bold();
                            });

                            row.ConstantItem(100).AlignRight().Text("CleaningApp")
                               .FontSize(16).Bold().FontColor(Colors.Grey.Medium);
                        });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Nabywca (Klient):").FontSize(10).FontColor(Colors.Grey.Medium);
                                c.Item().Text(order.Client?.FullName ?? "Brak danych").Bold();
                                c.Item().Text(order.Client?.Email ?? "");
                                c.Item().Text(order.Address ?? "");
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Wykonawca (Pracownik):").FontSize(10).FontColor(Colors.Grey.Medium);
                                c.Item().Text(order.Worker?.FullName ?? "Nieprzypisany").Bold();
                                c.Item().Text("CleaningApp");
                            });
                        });

                        col.Item().PaddingVertical(20).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Nazwa Usługi").Bold();
                                header.Cell().AlignRight().Text("Cena").Bold();
                            });

                            table.Cell().PaddingVertical(5).Text(order.Service?.Name ?? "Usługa niestandardowa");
                            table.Cell().PaddingVertical(5).AlignRight().Text($"{order.Service?.DefaultPrice:C2}");

                            table.Cell().ColumnSpan(2).PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            table.Cell().Text("RAZEM DO ZAPŁATY:").Bold().FontSize(14);
                            table.Cell().AlignRight().Text($"{order.Service?.DefaultPrice:C2}")
                                .Bold().FontSize(14).FontColor(Colors.Indigo.Medium);
                        });

                        if (!string.IsNullOrEmpty(order.ClientNote))
                        {
                            col.Item().PaddingTop(20).Background(Colors.Grey.Lighten4).Padding(10).Column(c =>
                            {
                                c.Item().Text("Uwagi do zlecenia:").FontSize(10).FontColor(Colors.Grey.Darken1);
                                c.Item().Text(order.ClientNote).Italic();
                            });
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Dokument wygenerowany elektronicznie. ");
                            x.Span($"CleaningApp {DateTime.Now.Year}");
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}