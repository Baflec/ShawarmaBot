using System.Collections.Generic;

namespace ShawarmaBotCore.Model
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}