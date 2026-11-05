namespace ToolStore.Models
{
    public class Tool
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int QuantityInStock { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }

        public int ApplicationAreaId { get; set; }
        public ApplicationArea? ApplicationArea { get; set; }
    }
}