using System.ComponentModel.DataAnnotations.Schema;

namespace TestJQuery.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Object { get; set; }
        public string Content { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
