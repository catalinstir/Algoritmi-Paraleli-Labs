namespace ex06.Models
{
    public class Order
    {
        public string Id { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
