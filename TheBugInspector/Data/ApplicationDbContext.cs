using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheBugInspector.Models;

namespace TheBugInspector.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public virtual DbSet<FileUpload> Images { get; set; }

        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<Invite> Invites { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<Ticket> Tickets { get; set; }

        public virtual DbSet<TicketAttachment> TicketAttachments { get; set; }

        public virtual DbSet<TicketComment> TicketComments { get; set; }
    }
}
