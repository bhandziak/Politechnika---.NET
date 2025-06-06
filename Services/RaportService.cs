using CarWorkshopProjekt.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

namespace CarWorkshopProjekt.Services
{
    public class RaportService : IRaportService
    {
        private readonly AppDbContext _dbContext;

        public RaportService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public byte[] GenerateRepairReportPdf(int month)
        {
            var data = GetDataForMonth(month);
            if (data == null || !data.Any())
                throw new Exception("Brak danych do raportu, nie można wygenerować PDF.");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Header().Text($"Raport napraw za miesiąc {month + 1}");
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Klient").Bold();
                            header.Cell().Text("Pojazd").Bold();
                            header.Cell().Text("Suma kosztów").Bold();
                            header.Cell().Text("Liczba zleceń").Bold();
                        });

                        foreach (var row in data)
                        {
                            table.Cell().Text(row.ClientName);
                            table.Cell().Text(row.Vehicle);
                            table.Cell().Text($"{row.TotalCost} PLN");
                            table.Cell().Text(row.OrdersCount.ToString());
                        }
                    });
                });
            });
            return document.GeneratePdf();
        }
        private List<RaportRow> GetDataForMonth(int month)
        {
            var startDate = new DateTime(DateTime.Now.Year, month + 1, 1);
            var endDate = startDate.AddMonths(1);

            var groupedData = _dbContext.ServiceOrders
            .Include(o => o.Customer)
            .Include(o => o.Vehicle)
            .Include(o => o.ServiceTasks)
                .ThenInclude(t => t.UsedParts)
                    .ThenInclude(up => up.Part)
            .Where(o =>
                o.DateFinished != null &&
                o.DateFinished >= startDate &&
                o.DateFinished < endDate &&
                o.Customer != null &&
                o.Customer.NameCustomer != null &&
                o.Vehicle != null &&
                o.Vehicle.BrandVehicle != null &&
                o.Vehicle.ModelVehicle != null &&
                o.ServiceTasks != null && o.ServiceTasks.Any())
            .AsEnumerable()
            .GroupBy(o => new
            {
                ClientName = o.Customer.NameCustomer,
                Vehicle = $"{o.Vehicle.BrandVehicle} {o.Vehicle.ModelVehicle}"
            })
            .Select(g => new RaportRow
            {
                ClientName = g.Key.ClientName,
                Vehicle = g.Key.Vehicle,
                TotalCost = g.Sum(o => o.ServiceTasks.Sum(t =>
                    t.LaborCost +
                    (t.UsedParts != null
                        ? t.UsedParts.Sum(up => up.Part != null ? up.Part.UnitPrice * up.Quantity : 0)
                        : 0)
                )),
                OrdersCount = g.Count()
            })
            .ToList();

            if (!groupedData.Any())
                throw new Exception("Brak danych do wygenerowania raportu.");

            return groupedData;
        }
    }

    public class RaportRow
    {
        public string ClientName { get; set; }
        public string Vehicle { get; set; }
        public decimal TotalCost { get; set; }
        public int OrdersCount { get; set; }
    }

}
