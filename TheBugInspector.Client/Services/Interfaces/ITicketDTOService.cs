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
        Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId);
        #endregion

        #region Update
        Task UpdateTicketAsync(TicketDTO ticket, int companyId, string userId);

        Task ArchiveTicketAsync(int ticketId, int companyId);
        Task RestoreTicketAsync(int ticketId, int companyId);
        #endregion

        #region Delete
        #endregion
    }
}
