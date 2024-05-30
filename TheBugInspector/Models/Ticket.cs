using System.ComponentModel.DataAnnotations;
using TheBugInspector.Data;
using TheBugInspector.Client;
using TheBugInspector.Client.Components.Models;

namespace TheBugInspector.Models
{
    public class Ticket
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "Ticket Description")]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string? Description { get; set; }

        [Required]
        public DateTimeOffset Created
        {
            get => _created.ToLocalTime();
            set => _created = value.ToUniversalTime();
        }

        public DateTimeOffset? JoinDate
        {
            get => _updated?.ToLocalTime();
            set => _updated = value?.ToUniversalTime();
        }

        public bool IsArchived { get; set; }
        public bool IsArchivedByProject { get; set; }

        public ProjectPriority Priority { get; set; }

        public TicketType Type { get; set; }

        public TicketStatus Status { get; set; }

        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }

        [Required]
        public string? SubmitterUserId { get; set; }
        public virtual ApplicationUser? SubmitterUser { get; set; }

        public string? DeveloperUserId { get; set; }
        public virtual ApplicationUser? DeveloperUser { get; set; }

        public virtual ICollection<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();
        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new HashSet<TicketAttachment>();
    }

    public static class TicketExtensions
    {
        public static TicketDTO ToDTO(this Ticket ticket)
        {
            TicketDTO dto = new()
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Created = ticket.Created,
                JoinDate = ticket.JoinDate,
                IsArchived = ticket.IsArchived,
                IsArchivedByProject = ticket.IsArchivedByProject,
                Priority = ticket.Priority,
                Type = ticket.Type,
                Status = ticket.Status,
                ProjectId = ticket.ProjectId,
                SubmitterUserId = ticket.SubmitterUserId,
                DeveloperUserId = ticket.DeveloperUserId,

            };

            if(ticket.Project is not null)
            {
                ProjectDTO projectDTO = ticket.Project.ToDTO();
                dto.Project = projectDTO;
            }

            if(ticket.SubmitterUser is not null)
            {
                UserDTO userDTO = ticket.SubmitterUser.ToDTO();
                dto.SubmitterUser = userDTO;
            }

            if( ticket.DeveloperUser is not null)
            {
                UserDTO userDTO = ticket.DeveloperUser.ToDTO();
                dto.DeveloperUser = userDTO;
            }

            foreach (TicketComment comment in ticket.Comments)
            {
                TicketCommentDTO commentDTO = comment.ToDTO();
                dto.Comments.Add(commentDTO);
            }

            foreach (TicketAttachment attachment in ticket.Attachments)
            {
                TicketAttachmentDTO attachmentDTO = attachment.ToDTO();
                dto.Attachments.Add(attachmentDTO);
            }

            return dto;
        }
    }
}
