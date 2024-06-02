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
        Task<Ticket?> GetTicketByIdAsync(int ticketId, int companyId);
        #endregion

        #region Update
        Task UpdateTicketAsync(Ticket ticket, int companyId, string userId);

        Task ArchiveTicketAsync(int ticketId, int companyId);
        Task RestoreTicketAsync(int ticketId, int companyId);
        #endregion

        #region Delete
        #endregion
    }
}
