namespace CarWorkshopProjekt.Services
{
    public interface IRaportService
    {
        byte[] GenerateRepairReportPdf(int month);
    }
}
