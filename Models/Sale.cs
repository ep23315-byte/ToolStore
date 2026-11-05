namespace ToolStore.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Quantity { get; set; }
        public string Status { get; set; } = "Активна";

        public int ToolId { get; set; }
        public Tool? Tool { get; set; }
    }
}