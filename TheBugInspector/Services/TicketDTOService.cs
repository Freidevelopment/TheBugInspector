using TheBugInspector.Client.Models;
using TheBugInspector.Client.Services.Interfaces;
using TheBugInspector.Models;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class TicketDTOService(ITicketRepository repository) : ITicketDTOService
    {
        public async Task<TicketDTO> AddTicketAsync(TicketDTO ticket, int companyId)
        {
            Ticket newTicket = new Ticket()
            {
                Title = ticket.Title,
                Description = ticket.Description,
                Type = ticket.Type,
                Priority = ticket.Priority,
                ProjectId = ticket.ProjectId,
                SubmitterUserId = ticket.SubmitterUserId,
            };

            Ticket createdTicket = await repository.AddTicketAsync(newTicket, companyId);

            return createdTicket.ToDTO();
        }

        public async Task ArchiveTicketAsync(int ticketId, int companyId)
        {
            Ticket? ticket = await repository.GetTicketByIdAsync(ticketId, companyId);

            if (ticket is not null)
            {
                await repository.ArchiveTicketAsync(ticketId, companyId);
            }
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

        public async Task RestoreTicketAsync(int ticketId, int companyId)
        {
            Ticket? ticket = await repository.GetTicketByIdAsync(ticketId, companyId);

            if(ticket is not null)
            {
                await repository.RestoreTicketAsync(ticketId, companyId);
            }
        }

        public async Task UpdateTicketAsync(TicketDTO ticket, int companyId, string userId)
        {
            Ticket? ticketToUpdate = await repository.GetTicketByIdAsync(ticket.Id, companyId);

            if(ticketToUpdate is not null)
            {
                ticketToUpdate.Title = ticket.Title;
                ticketToUpdate.Updated = DateTimeOffset.Now;
                ticketToUpdate.Status = ticket.Status;
                ticketToUpdate.Priority = ticket.Priority;
                ticketToUpdate.Type = ticket.Type;
                ticketToUpdate.Description = ticket.Description;
                

                await repository.UpdateTicketAsync(ticketToUpdate, companyId, userId);

            }
            

        }
    }
}
