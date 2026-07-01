using System.ComponentModel.DataAnnotations.Schema;

namespace TestJQuery.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        [ForeignKey("UserId")]
        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }

        public ICollection<OrderedPizza>? OrderedPizzas { get; set; }
    }
}
