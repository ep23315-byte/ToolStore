namespace ToolStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string Status { get; set; } = "Оформлен";

        public int UserId { get; set; }
        public User? User { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}