using System.ComponentModel.DataAnnotations;

namespace TheBugInspector.Client.Components.Models
{
    public class InviteDTO
    {
        private DateTimeOffset _InviteDate;
        private DateTimeOffset? _JoinDate;
        public int Id { get; set; }

        [Required]
        public DateTimeOffset InviteDate
        {
            get => _InviteDate.ToLocalTime();
            set => _InviteDate = value.ToUniversalTime();
        }

        public DateTimeOffset? JoinDate
        {
            get => _JoinDate?.ToLocalTime();
            set => _JoinDate = value?.ToUniversalTime();
        }

        [Required]
        public string? InviteeEmail { get; set; }
        [Required]
        public string? InviteeFirstName { get; set; }
        [Required]
        public string? InviteeLastName { get; set; }

        public string? Message { get; set; }

        public bool IsValid { get; set; }

        public int ProjectId { get; set; }
        public virtual ProjectDTO? Project { get; set; }
        [Required]
        public string? InvitorId { get; set; }
        public virtual UserDTO? Invitor { get; set; }

        public string? InviteeId { get; set; }
        public virtual UserDTO? Invitee { get; set; }

    }
}

