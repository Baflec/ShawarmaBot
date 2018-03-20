namespace ShawarmaBotCore.Model
{
    public class User
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public Order Order { get; set; } = new Order();
    }
}