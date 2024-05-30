using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace TheBugInspector.Client.Components.Models
{
    public class TicketCommentDTO
    {
        private DateTimeOffset _created;
        public int Id { get; set; }
        [Required]
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
