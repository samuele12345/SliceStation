using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace TestJQuery.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public string? Address { get; set; }

        public ICollection<Order>? Orders { get; set; }


        public ICollection<Review> Reviews { get; set; }

    }
}
