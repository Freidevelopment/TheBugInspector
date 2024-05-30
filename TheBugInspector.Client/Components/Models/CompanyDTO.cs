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
        public string? Description { get; set; }

        public string ImageUrl { get; set; } = ImageHelper.DefaultProfilePicture;

        public virtual ICollection<ProjectDTO> Projects { get; set; } = new HashSet<ProjectDTO>();

        public virtual ICollection<UserDTO> CompanyMembers { get; set; } = new HashSet<UserDTO>();

        public virtual ICollection<InviteDTO> Invites { get; set; } = new HashSet<InviteDTO>();
    }
}
