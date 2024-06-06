using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using TheBugInspector.Data;
using TheBugInspector.Models;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class TicketRepository(IDbContextFactory<ApplicationDbContext> _dbContextFactory,
                                   IServiceProvider serviceProvider) : ITicketRepository
    {


        public async Task AddCommentAsync(TicketComment comment, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            comment.Created = DateTimeOffset.Now;

            context.TicketComments.Add(comment);
            await context.SaveChangesAsync();




        }

        public async Task<Ticket> AddTicketAsync(Ticket ticket, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();


            bool shouldCreate = await context.Tickets.AnyAsync(c => c.Project!.CompanyId == companyId);

            if (shouldCreate)
            {

                ticket.Created = DateTimeOffset.Now;
                context.Tickets.Add(ticket);
                await context.SaveChangesAsync();

                return ticket;
            }

            return ticket;
        }

        public async Task<TicketAttachment> AddTicketAttachment(TicketAttachment attachment, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            // make sure the ticket exists and belongs to this company
            var ticket = await context.Tickets
                .FirstOrDefaultAsync(t => t.Id == attachment.TicketId && t.Project!.CompanyId == companyId);

            // save it if it does
            if (ticket is not null)
            {
                attachment.Created = DateTimeOffset.Now;
                context.TicketAttachments.Add(attachment);
                await context.SaveChangesAsync();

                return attachment;
            }
            else
            {
                throw new ArgumentException("Ticket not found");
            }
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


        public async Task DeleteCommentAsync(int commentId, int ticketId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            TicketComment? comment = context.TicketComments
                                                           .FirstOrDefault(c => c.Id == commentId);

            if (comment is not null)
            {
                context.TicketComments.Remove(comment);
                await context.SaveChangesAsync();
            }

        }

        public async Task DeleteTicketAttachment(int attachmentId, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            var attachment = await context.TicketAttachments
                .Include(a => a.Upload)
                .FirstOrDefaultAsync(a => a.Id == attachmentId && a.Ticket!.Project!.CompanyId == companyId);

            if (attachment is not null)
            {
                context.Remove(attachment);
                context.Remove(attachment.Upload!);
                await context.SaveChangesAsync();
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
                                                               .Include(t => t.DeveloperUser)
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
                                                  .Include(t => t.Comments)
                                                  .Include(t => t.DeveloperUser)
                                                  .FirstOrDefaultAsync(t => t.Id == ticketId);

            return ticket;
        }

        public async Task<TicketComment?> GetTicketCommentByIdAsync(int ticketId, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            TicketComment? comment = await context.TicketComments
                                                                 
                                                                 .Include(c => c.User)
                                                                 .FirstOrDefaultAsync(c => c.TicketId == ticketId);
            return comment;
        }

        public async Task<IEnumerable<TicketComment>> GetTicketCommentsAsync(int ticketId, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            IEnumerable<TicketComment> comments = await context.TicketComments
                                                                        .Where(c => c.TicketId == ticketId && c.Ticket.Project.CompanyId == companyId)
                                                                        .Include(c => c.User)
                                                                        .OrderByDescending(c => c.Created)
                                                                        .ToListAsync();

            return comments;
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

        public async Task UpdateCommentAsync(TicketComment comment, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            bool shouldEdit = await context.Companies.AnyAsync (c => c.Id == companyId);

            bool ticketEdit = await context.Tickets.AnyAsync(t => t.Id == comment.TicketId);

            bool commentEdit = await context.TicketComments.AnyAsync(c => c.Id == comment.Id);

            if (shouldEdit && ticketEdit && commentEdit) 
            { 
                context.TicketComments.Update(comment);
                await context.SaveChangesAsync();
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
