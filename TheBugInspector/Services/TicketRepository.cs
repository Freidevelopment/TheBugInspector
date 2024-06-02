using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using TheBugInspector.Data;
using TheBugInspector.Models;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class TicketRepository : ITicketRepository
    {

        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public TicketRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }


        public async Task<Ticket> AddTicketAsync(Ticket ticket, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();


            bool shouldCreate = await context.Companies.AnyAsync(c => c.Id == companyId);

            if (shouldCreate)
            {

                ticket.Created = DateTimeOffset.Now;
                context.Tickets.Add(ticket);
                await context.SaveChangesAsync();

                return ticket;
            }

            return ticket;
        }

        public async Task ArchiveTicketAsync(int ticketId, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            bool shouldEdit = await context.Companies.AnyAsync(c => c.Id == companyId);

            bool ticketEdit = await context.Tickets.AnyAsync(t => t.Id == ticketId);

            if (shouldEdit && ticketEdit)
            {
                Ticket? ticket = await context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
                if (ticket is not null)
                {
                    ticket.IsArchived = true;
                    context.Tickets.Update(ticket);
                    await context.SaveChangesAsync();
                }

            }
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync(int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();



            IEnumerable<Ticket> tickets = await context.Tickets
                                                               .Where(t => t.Project!.CompanyId == companyId)
                                                               .Include(t => t.Project)
                                                               .Include(t => t.Attachments)
                                                               .Include(t => t.Comments)
                                                               .Include(t => t.SubmitterUser)
                                                               .OrderByDescending(t => t.Created)
                                                               .ToListAsync();

            return tickets;
        }

        public async Task<Ticket?> GetTicketByIdAsync(int ticketId, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            Ticket? ticket = await context.Tickets
                                                  .Where(t => t.Project!.CompanyId == companyId)
                                                  .Include(t => t.Project)
                                                  .Include(t => t.SubmitterUser)
                                                  .Include(t => t.Attachments)
                                                  .Include(t => t.DeveloperUser)
                                                  .Include(t => t.Comments)
                                                  .FirstOrDefaultAsync(t => t.Id == ticketId);

            return ticket;
        }

        public async Task RestoreTicketAsync(int ticketId, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            bool shouldEdit = await context.Companies.AnyAsync(c => c.Id == companyId);

            bool ticketEdit = await context.Tickets.AnyAsync(t => t.Id == ticketId);

            if (shouldEdit && ticketEdit)
            {
                Ticket? ticket = await context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
                if (ticket is not null)
                {
                    ticket.IsArchived = false;
                    context.Tickets.Update(ticket);
                    await context.SaveChangesAsync();
                }
 
            }
            
        }

        public async Task UpdateTicketAsync(Ticket ticket, int companyId, string userId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            bool shouldEdit = await context.Companies.AnyAsync(c => c.Id == companyId);

            bool ticketEdit = await context.Tickets.AnyAsync(t => t.Id == ticket.Id);

            if (shouldEdit && ticketEdit)
            {
                context.Tickets.Update(ticket);
                await context.SaveChangesAsync();

            }
            
        }
    }
}
