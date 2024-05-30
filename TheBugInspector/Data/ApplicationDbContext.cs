using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheBugInspector.Models;

namespace TheBugInspector.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public virtual DbSet<FileUpload> Images { get; set; }

        public virtual DbSet<TaskerItem> TaskerItems { get; set; }
    }
}
