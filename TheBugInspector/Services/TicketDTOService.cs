using TheBugInspector.Client.Models;
using TheBugInspector.Client.Services.Interfaces;
using TheBugInspector.Data;
using TheBugInspector.Models;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class TicketDTOService(ITicketRepository repository) : ITicketDTOService
    {
        public async Task AddCommentAsync(TicketCommentDTO comment, int companyId)
        {
            TicketComment newComment = new TicketComment()
            {
                Id = comment.Id,
                TicketId = comment.TicketId,
                Content = comment.Content,
                UserId = comment.UserId,
                
            };

            await repository.AddCommentAsync(newComment, companyId);
        }

        public async Task<TicketDTO> AddTicketAsync(TicketDTO ticket, int companyId)
        {
            Ticket newTicket = new Ticket()
            {
                Title = ticket.Title,
                Description = ticket.Description,
                Type = ticket.Type,
                Priority = ticket.Priority,
                Status = TicketStatus.New,
                ProjectId = ticket.ProjectId,
                SubmitterUserId = ticket.SubmitterUserId,
            };

            Ticket createdTicket = await repository.AddTicketAsync(newTicket, companyId);

            return createdTicket.ToDTO();
        }

        public async Task<TicketAttachmentDTO> AddTicketAttachment(TicketAttachmentDTO attachment, byte[] uploadData, string contentType, int companyId)
        {
            FileUpload file = new()
            {
                Type = contentType,
                Data = uploadData,
            };

            TicketAttachment dbAttachment = new()
            {
                TicketId = attachment.TicketId,
                Description = attachment.Description,
                FileName = attachment.FileName,
                Upload = file,
                Created = DateTimeOffset.Now,
                UserId = attachment.UserId
            };

            dbAttachment = await repository.AddTicketAttachment(dbAttachment, companyId);

            return dbAttachment.ToDTO();
        }


        public async Task ArchiveTicketAsync(int ticketId, int companyId)
        {
            Ticket? ticket = await repository.GetTicketByIdAsync(ticketId, companyId);

            if (ticket is not null)
            {
                await repository.ArchiveTicketAsync(ticketId, companyId);
            }
        }

        public async Task DeleteCommentAsync(int commentId, int ticketId)
        {
            await repository.DeleteCommentAsync(commentId, ticketId);
        }

        public async Task DeleteTicketAttachment(int attachmentId, int companyId)
        {
            await repository.DeleteTicketAttachment(attachmentId, companyId);
        }

        public async Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId)
        {
            IEnumerable<Ticket> tickets = await repository.GetAllTicketsAsync(companyId);

            IEnumerable<TicketDTO> results = tickets.Select(t => t.ToDTO());

            return results;
        }

        public async Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId)
        {
            Ticket? ticket = await repository.GetTicketByIdAsync(ticketId, companyId);

            return ticket?.ToDTO();
        }

        public async Task<TicketCommentDTO?> GetTicketCommentByIdAsync(int ticketId, int companyId)
        {
            TicketComment? comment = await repository.GetTicketCommentByIdAsync(ticketId, companyId);

            return comment?.ToDTO();
        }

        public async Task<IEnumerable<TicketCommentDTO>> GetTicketCommentsAsync(int ticketId, int companyId)
        {

            IEnumerable<TicketComment> comments = await repository.GetTicketCommentsAsync(ticketId, companyId);

            IEnumerable<TicketCommentDTO> results = comments.Select(t => t.ToDTO());

            return results;
        }

        public async Task RestoreTicketAsync(int ticketId, int companyId)
        {
            Ticket? ticket = await repository.GetTicketByIdAsync(ticketId, companyId);

            if (ticket is not null)
            {
                await repository.RestoreTicketAsync(ticketId, companyId);
            }
        }

        public async Task UpdateCommentAsync(TicketCommentDTO comment, int companyId)
        {
            TicketComment? commentToUpdate = await repository.GetTicketCommentByIdAsync(comment.TicketId, companyId);

            if (commentToUpdate is not null)
            {
                commentToUpdate.Content = comment.Content;


                await repository.UpdateCommentAsync(commentToUpdate, companyId);
            }
        }

        public async Task UpdateTicketAsync(TicketDTO ticket, int companyId, string userId)
        {
            Ticket? ticketToUpdate = await repository.GetTicketByIdAsync(ticket.Id, companyId);

            if (ticketToUpdate is not null)
            {
                ticketToUpdate.Title = ticket.Title;
                ticketToUpdate.Updated = DateTimeOffset.Now;
                ticketToUpdate.Status = ticket.Status;
                ticketToUpdate.Priority = ticket.Priority;
                ticketToUpdate.Type = ticket.Type;
                ticketToUpdate.Description = ticket.Description;
                ticketToUpdate.DeveloperUserId = ticket.DeveloperUserId;
                ticketToUpdate.DeveloperUser = null;

                await repository.UpdateTicketAsync(ticketToUpdate, companyId, userId);

            }


        }
    }
}
