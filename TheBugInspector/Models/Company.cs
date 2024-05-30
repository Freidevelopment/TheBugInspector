using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using TheBugInspector.Client;
using TheBugInspector.Client.Components.Models;
using TheBugInspector.Data;
using TheBugInspector.Helpers;

namespace TheBugInspector.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Company Description")]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string? Description { get; set; }

        public Guid? ImageId { get; set; }

        public virtual FileUpload? Image {  get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();

        public virtual ICollection<ApplicationUser> CompanyMembers { get; set; } = new HashSet<ApplicationUser>();

        public virtual ICollection<Invite> Invites { get; set; } = new HashSet<Invite>();

    }

    public static class CompanyExtensions
    {
        public static CompanyDTO ToDTO(this Company company)
        {
            CompanyDTO dto = new()
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                ImageUrl = company.ImageId.HasValue ? $"api/uploads/{company.ImageId}" : UploadHelper.DefaultContactImage,

            };

            foreach(Project project in company.Projects)
            {
                
                ProjectDTO projectDTO = project.ToDTO();
                dto.Projects.Add(projectDTO);
            }

            foreach(Invite invite in company.Invites)
            {
                
                InviteDTO inviteDTO = invite.ToDTO();
                dto.Invites.Add(inviteDTO);
            }

            foreach (ApplicationUser User in company.CompanyMembers)
            {

                UserDTO userDTO = User.ToDTO();
                dto.CompanyMembers.Add(userDTO);
            }

            return dto;
        }
    }
}
