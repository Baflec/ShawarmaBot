namespace ShawarmaBotCore.Model
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public int Count { get; set; }
    }
}