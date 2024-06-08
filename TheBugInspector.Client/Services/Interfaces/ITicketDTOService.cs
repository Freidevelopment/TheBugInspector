using System.Net.Sockets;
using TheBugInspector.Client.Models;

namespace TheBugInspector.Client.Services.Interfaces
{
    public interface ITicketDTOService
    {
        #region Create
        Task<TicketDTO> AddTicketAsync(TicketDTO ticket, int companyId);
        #endregion

        #region Retrieve
        Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId);
        Task<IEnumerable<TicketDTO>> GetUserTicketsAsync(int companyId, string userId);
        Task<IEnumerable<TicketDTO>> GetAllArchivedTicketsAsync(int companyId);
        Task<IEnumerable<TicketDTO>> GetArchivedUserTicketsAsync(int companyId, string userId);
        Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId);
        #endregion

        #region Update
        Task UpdateTicketAsync(TicketDTO ticket, int companyId, string userId);

        Task ArchiveTicketAsync(int ticketId, int companyId);
        Task RestoreTicketAsync(int ticketId, int companyId);
        #endregion

        #region Delete
        #endregion


        #region Ticket Comments
        Task<IEnumerable<TicketCommentDTO>> GetTicketCommentsAsync(int ticketId, int companyId);

        Task<TicketCommentDTO?> GetTicketCommentByIdAsync(int ticketId, int companyId);

        Task AddCommentAsync(TicketCommentDTO comment, int companyId);

        Task DeleteCommentAsync(int commentId, int ticketId);

        Task UpdateCommentAsync(TicketCommentDTO comment, int companyId);
        #endregion

        #region Attachments
        Task<TicketAttachmentDTO> AddTicketAttachment(TicketAttachmentDTO attachment, byte[] uploadData, string contentType, int companyId);
        Task DeleteTicketAttachment(int attachmentId, int companyId);
        #endregion

    }
}
