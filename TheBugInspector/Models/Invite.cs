﻿using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using TheBugInspector.Client.Models;
using TheBugInspector.Data;

namespace TheBugInspector.Models
{
    public class Invite
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

        public Guid CompanyToken { get; set; }

        [Required]
        public string? InviteeEmail { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string? InviteeFirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} at most {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string? InviteeLastName { get; set; }

        public string? Message { get; set; }

        public bool IsValid { get; set; }

        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        [Required]
        public string? InvitorId { get; set; }
        public virtual ApplicationUser? Invitor { get; set; }

        public string? InviteeId { get; set; }
        public virtual ApplicationUser? Invitee { get; set; }

    }

    public static class InviteExtensions
    {
        public static InviteDTO ToDTO(this Invite invite)
        {
            InviteDTO dto = new()
            {
                Id = invite.Id,
                InviteDate = invite.InviteDate,
                JoinDate = invite.JoinDate,
                InviteeEmail = invite.InviteeEmail,
                InviteeFirstName = invite.InviteeFirstName,
                InviteeLastName = invite.InviteeLastName,
                Message = invite.Message,
                IsValid = invite.IsValid,
                ProjectId = invite.ProjectId,
                InviteeId = invite.InviteeId,
                InvitorId = invite.InvitorId,
            };

            if(invite.Project is not null)
            {
                ProjectDTO projectDTO = invite.Project.ToDTO();
                dto.Project = projectDTO;
            }
            
            if(invite.Invitor is not null)
            {
                invite.Invitor.Projects = [];

                UserDTO userDTO = invite.Invitor.ToDTO();
                dto.Invitor = userDTO;
            }

            if (invite.Invitee is not null)
            {
                invite.Invitee.Projects = [];

                UserDTO userDTO = invite.Invitee.ToDTO();
                dto.Invitor = userDTO;
            }

            return dto;
        }
    }
}
