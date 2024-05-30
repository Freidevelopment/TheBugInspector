using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace TheBugInspector.Client.Components.Models
{
    public class ProjectDTO
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


        public virtual ICollection<UserDTO> CompanyMembers { get; set; } = new HashSet<UserDTO>();

        public virtual ICollection<TicketDTO> Tickets { get; set; } = new HashSet<TicketDTO>();
    }
}
