
namespace CarWorkshopProjekt.Data
{
    public class Part
    {
        public int PartId { get; set; }
        public string NamePart { get; set; }
        public decimal UnitPrice { get; set; }

        // kolekcja UsedParts powiązanych z tym Part
        public ICollection<UsedPart> UsedParts { get; set; }
    }
}
