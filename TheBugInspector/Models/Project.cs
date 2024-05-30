using System.ComponentModel.DataAnnotations;
using TheBugInspector.Client;
using TheBugInspector.Client.Components.Models;
using TheBugInspector.Data;

namespace TheBugInspector.Models
{
    public class Project
    {
        private DateTimeOffset _created;
        private DateTimeOffset _startDate;
        private DateTimeOffset _endDate;
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Project Description")]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string? Description { get; set; }

        [Required]
        public DateTimeOffset Created
        {
            get => _created.ToLocalTime();
            set => _created = value.ToUniversalTime();
        }

        [Required]
        public DateTimeOffset StartDate
        {
            get => _startDate.ToLocalTime();
            set => _startDate = value.ToUniversalTime();
        }
        [Required]
        public DateTimeOffset EndDate
        {
            get => _endDate.ToLocalTime();
            set => _endDate = value.ToUniversalTime();
        }

        [Required]
        public ProjectPriority Priority { get; set; } 

        public bool IsArchived { get; set; }

        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public virtual ICollection<ApplicationUser> CompanyMembers { get; set; } = new HashSet<ApplicationUser>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }

    public static class ProjectExtensions
    {
        public static ProjectDTO ToDTO(this Project project)
        {
            ProjectDTO dto = new()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Created = project.Created,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Priority = project.Priority,
                IsArchived = project.IsArchived,
                
            };

            foreach (ApplicationUser user in project.CompanyMembers)
            {
                user.Projects.Clear();

                UserDTO userInfo = user.ToDTO();
                dto.CompanyMembers.Add(userInfo);
            }

            foreach (Ticket ticket in project.Tickets)
            {
                TicketDTO ticketDTO = ticket.ToDTO();
                dto.Tickets.Add(ticketDTO);
            }

            return dto;
        }
    }
}
