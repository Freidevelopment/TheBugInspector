using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FreiBlogBuilder.Client.Helpers;

namespace TheBugInspector.Client.Components.Models
{
    public class CompanyDTO
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Company Description")]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string? Description { get; set; }

        public string ImageUrl { get; set; } = ImageHelper.DefaultProfilePicture;

        public virtual ICollection<ProjectDTO> Projects { get; set; } = new HashSet<ProjectDTO>();

        public virtual ICollection<UserDTO> CompanyMembers { get; set; } = new HashSet<UserDTO>();

        public virtual ICollection<InviteDTO> Invites { get; set; } = new HashSet<InviteDTO>();
    }
}
