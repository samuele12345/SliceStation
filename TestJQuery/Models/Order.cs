using System.ComponentModel.DataAnnotations.Schema;

namespace TestJQuery.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        [ForeignKey("UserId")]
        public int? UserId { get; set; }

        public ICollection<Pizza>? Pizzas { get; set; }
    }
}
