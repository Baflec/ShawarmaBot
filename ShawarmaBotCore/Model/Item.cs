namespace ShawarmaBotCore.Model
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public Category Category { get; set; }
    }
}