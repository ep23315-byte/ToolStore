namespace ToolStore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ToolId { get; set; }
        public Tool? Tool { get; set; }
    }
}