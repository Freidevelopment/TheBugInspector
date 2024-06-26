﻿using TheBugInspector.Client.Models;
using TheBugInspector.Data;
using TheBugInspector.Models;

namespace TheBugInspector.Services.Interfaces
{
    public interface ITicketRepository
    {
        #region Create
        Task<Ticket> AddTicketAsync(Ticket ticket, int companyId);
        #endregion

        #region Retrieve
        Task<IEnumerable<Ticket>> GetAllTicketsAsync(int companyId);
        Task<IEnumerable<Ticket>> GetAllArchivedTicketsAsync(int companyId);
        //Task<PagedList<Ticket>> GetPagedActiveTicketsAsync(int companyId, int page, int pageSize);
        //Task<PagedList<Ticket>> GetPagedArchivedTicketsAsync(int companyId, int page, int pageSize);
        //Task<PagedList<Ticket>> GetPagedUserTicketsAsync(int companyId, string userId, int page, int pageSize);
        //Task<PagedList<Ticket>> GetPagedArchivedUserTicketsAsync(int companyId, string userId, int page, int pageSize);
        Task<IEnumerable<Ticket>> GetMostRecentActiveTicketsAsync(int companyId);
        Task<IEnumerable<Ticket>> GetMostRecentArchivedTicketsAsync(int companyId);
        Task<IEnumerable<Ticket>> GetUserTicketsAsync(int companyId, string userId);
        Task<IEnumerable<Ticket>> GetRecentUserTicketsAsync(int companyId, string userId);
        Task<IEnumerable<Ticket>> GetArchivedUserTicketsAsync(int companyId, string userId);
        Task<IEnumerable<Ticket>> GetRecentArchivedUserTicketsAsync(int companyId, string userId);
        Task<Ticket?> GetTicketByIdAsync(int ticketId, int companyId);
        #endregion

        #region Update
        Task UpdateTicketAsync(Ticket ticket, int companyId, string userId);

        Task ArchiveTicketAsync(int ticketId, int companyId);
        Task RestoreTicketAsync(int ticketId, int companyId);
        #endregion

        #region Delete
        #endregion


        #region Ticket Comments
        Task<IEnumerable<TicketComment>> GetTicketCommentsAsync(int ticketId, int companyId);

        Task<TicketComment?> GetTicketCommentByIdAsync(int ticketId, int companyId);

        Task AddCommentAsync(TicketComment comment, int companyId);

        Task DeleteCommentAsync(int commentId, int ticketId);

        Task UpdateCommentAsync(TicketComment comment, int companyId);
        #endregion

        #region Attachments
        Task<TicketAttachment> AddTicketAttachment(TicketAttachment attachment, int companyId);
        Task DeleteTicketAttachment(int attachmentId, int companyId);
        Task<TicketAttachment?> GetTicketAttachmentById(int attachmentId, int companyId);
        #endregion
        
    }
}
