using FreiBlogBuilder.Client.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace TheBugInspector.Client.Components.Models
{
    public class TicketAttachmentDTO
    {
        private DateTimeOffset _created;
        public int Id { get; set; }
        [Required]
        public string? FileName { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTimeOffset Created
        {
            get => _created.ToLocalTime();
            set => _created = value.ToUniversalTime();
        }

        public string UploadUrl { get; set; } = ImageHelper.DefaultProfilePicture;
        [Required]
        public string? UserId { get; set; }
        public virtual UserDTO? User { get; set; }

        public int TicketId { get; set; }
    }
}
