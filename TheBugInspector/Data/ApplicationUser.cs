using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TheBugInspector.Client.Components.Models;
using TheBugInspector.Client;
using TheBugInspector.Helpers;
using TheBugInspector.Models;

namespace TheBugInspector.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [NotMapped]
        public string? FullName => $"{FirstName} {LastName}";

        public Guid? ProfilePictureId { get; set; }

        public virtual FileUpload? ProfilePicture { get; set; }

        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();
    }

    public static class ApplicationUserExtensions
    {
        public static UserDTO ToDTO(this ApplicationUser user)
        {
            UserDTO dto = new()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePictureUrl = user.ProfilePictureId.HasValue ? $"api/uploads{user.ProfilePictureId}" : UploadHelper.DefaultProfilePicture,
                Email = user.Email,

            };

            return dto;
        }
    }

}
