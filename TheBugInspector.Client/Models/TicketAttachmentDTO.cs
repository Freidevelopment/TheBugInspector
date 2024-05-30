using FreiBlogBuilder.Client.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace TheBugInspector.Client.Models
{
    public class TicketAttachmentDTO
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

        public string? UploadUrl { get; set; } = ImageHelper.DefaultProfilePicture;
        [Required]
        public string? UserId { get; set; }
        public virtual UserDTO? User { get; set; }

        public int TicketId { get; set; }
    }
}
