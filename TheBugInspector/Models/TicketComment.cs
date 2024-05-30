using System.ComponentModel.DataAnnotations;
using TheBugInspector.Client.Components.Models;
using TheBugInspector.Data;
using TheBugInspector.Helpers;

namespace TheBugInspector.Models
{
    public class TicketComment
    {
        private DateTimeOffset _created;
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ticket Comment Content")]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string? Content { get; set; }

        [Required]
        public DateTimeOffset Created
        {
            get => _created.ToLocalTime();
            set => _created = value.ToUniversalTime();
        }

        [Required]
        public int TicketId { get; set; }
        public virtual Ticket? Ticket { get; set; }

        [Required]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
    public static class TicketCommentExtensions
    {
        public static TicketCommentDTO ToDTO(this TicketComment ticket)
        {
            TicketCommentDTO dto = new()
            {
               Id = ticket.Id,
               Content = ticket.Content,
               Created = ticket.Created,
               TicketId = ticket.TicketId,
               UserId = ticket.UserId
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
