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
        public  string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? ProfilePictureUrl { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
