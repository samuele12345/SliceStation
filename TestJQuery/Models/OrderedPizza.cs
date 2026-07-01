using System.ComponentModel.DataAnnotations.Schema;

namespace TestJQuery.Models
{
    public class OrderedPizza
    {
        public int Id { get; set; }

        [ForeignKey("OrderId")]
        public int OrderId { get; set; }

        public Order? Order { get; set; }

        [ForeignKey("PizzaId")]
        public int PizzaId { get; set; }
        public Pizza? Pizza { get; set; }

        public int Quantity { get; set; }

        public string? PizzaName { get; set; }

        public decimal PriceSnapshot { get; set; }
    }
}
