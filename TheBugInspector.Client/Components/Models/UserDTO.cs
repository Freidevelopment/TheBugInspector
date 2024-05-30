using System.ComponentModel.DataAnnotations;

namespace TheBugInspector.Client.Components.Models
{
    public class UserDTO
    {
        [Required]
        public  string? UserId { get; set; }
        [Required]
        public  string? Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public  string? FirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Required]
        public string? ProfilePictureUrl { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
