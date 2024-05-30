using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace TheBugInspector.Client.Models
{
    public class TicketCommentDTO
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

        [Required]
        public string? UserId { get; set; }
        public virtual UserDTO? User { get; set; }
    }
}
