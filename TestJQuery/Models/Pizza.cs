using System.ComponentModel.DataAnnotations.Schema;

namespace TestJQuery.Models
{
    public class Pizza 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public ICollection<OrderedPizza>? OrderedPizzas { get; set; }

    }
}
