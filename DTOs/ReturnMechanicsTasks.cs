namespace CarWorkshopProjekt.DTOs
{
    public class ReturnMechanicsTasks
    {
        public string ServiceTaskId { get; set; }     
        public string Name { get; set; }             
        public decimal LaborCost { get; set; }        

        // Lista części użytych w tym 'task' wraz z ilością i kosztem jednostkowym
        public List<UsedPartDTO> UsedParts { get; set; }

        // Obliczone pole: koszt całkowity = LaborCost + suma (Quantity * UnitPrice) części
        public decimal TotalCost
        {
            get
            {
                decimal partsCost = UsedParts?.Sum(up => up.Quantity * up.Part.UnitPrice) ?? 0;
                return LaborCost + partsCost;
            }
        }
    }
}
