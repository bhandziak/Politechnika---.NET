namespace CarWorkshopProjekt.DTOs
{
    public class ReturnMechanicsTasks
    {
        public string ServiceTaskId { get; set; }     
        public string Name { get; set; }             
        public decimal LaborCost { get; set; }        

        // Lista części użytych w tym 'task' wraz z ilością i kosztem jednostkowym
        public UsedPartDTO UsedPart { get; set; }

        // Obliczone pole: koszt całkowity = LaborCost + suma (Quantity * UnitPrice) części
        public decimal TotalCost
             => LaborCost + (UsedPart?.Quantity * UsedPart?.Part?.UnitPrice ?? 0);
    }
}
