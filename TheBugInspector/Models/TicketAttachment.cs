using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using TheBugInspector.Client.Components.Models;
using TheBugInspector.Data;
using TheBugInspector.Helpers;

namespace TheBugInspector.Models
{
    public class TicketAttachment
    {
        private DateTimeOffset _created;
        public int Id { get; set; }
        [Required]
        public string? FileName { get; set; }

        [Required]
        [Display(Name = "Ticket Attachment Description")]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string? Description { get; set; }

        [Required]
        public DateTimeOffset Created
        {
            get => _created.ToLocalTime();
            set => _created = value.ToUniversalTime();
        }

        public Guid? UploadId { get; set; }
        public virtual FileUpload? Upload { get; set; }
        [Required]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket? Ticket { get; set; }
    }

    public static class TicketAttachmentExtensions
    {
        public static TicketAttachmentDTO ToDTO(this TicketAttachment ticket)
        {
            TicketAttachmentDTO dto = new()
            {
                Id = ticket.Id,
                FileName = ticket.FileName,
                Description = ticket.Description,
                Created = ticket.Created,
                TicketId = ticket.TicketId,
                UserId = ticket.UserId,
                UploadUrl = ticket.UploadId.HasValue ? $"api/uploads/{ticket.UploadId}" : UploadHelper.DefaultContactImage,
            };

            if(ticket.User is not null)
            {
                UserDTO userDTO = ticket.User.ToDTO();
                dto.User = userDTO;
            }


            return dto;


        }
    }

}
