using System.ComponentModel.DataAnnotations.Schema;

namespace TestJQuery.Models
{
    public class Pizza 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public double? Price { get; set; }

        [ForeignKey("OrderId")]
        public int OrderId { get; set; }

        public Order? Order { get; set; }

    }
}
