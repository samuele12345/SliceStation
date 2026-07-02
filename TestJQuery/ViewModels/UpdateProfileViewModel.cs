using System.ComponentModel.DataAnnotations;

namespace TestJQuery.ViewModels
{
    public class UpdateProfileViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
    }
}
