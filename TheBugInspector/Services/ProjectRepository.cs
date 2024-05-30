using Microsoft.EntityFrameworkCore;
using TheBugInspector.Data;
using TheBugInspector.Models;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public ProjectRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public Task<Project> AddProjectAsync(Project project, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task ArchiveProjectAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            IEnumerable<Project> projects = await context.Projects
                                                         .Where(p => p.IsArchived == false && p.CompanyId == companyId)
                                                         .Include(p => p.Tickets)
                                                         .Include(p => p.CompanyMembers)
                                                         .OrderByDescending(p => p.Created)
                                                         .ToListAsync();
            return projects;
        }

        public Task<IEnumerable<Project>> GetArchivedProjectsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<Project?> GetProjectByIdAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task RestoreProjectAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProjectAsync(Project project, int companyId)
        {
            throw new NotImplementedException();
        }
    }
}
