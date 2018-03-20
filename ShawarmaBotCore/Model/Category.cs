using System.Collections.Generic;

namespace ShawarmaBotCore.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Parent { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}