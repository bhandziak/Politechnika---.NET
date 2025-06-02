namespace CarWorkshopProjekt.DTOs
{
    public class PartDTO
    {
        public Guid PartId { get; set; }
        public string NamePart { get; set; }
        public string TypePart { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
