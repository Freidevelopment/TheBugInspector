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

        // this has been tested and works
        public async Task<Project> AddProjectAsync(Project project, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            project.Created = DateTimeOffset.Now;

            bool shouldCreate = await context.Projects.AnyAsync(c => c.CompanyId == companyId);

            if (shouldCreate)
            {
                context.Projects.Add(project);
                await context.SaveChangesAsync();

                return project;
            }
            else
            {
                return project;
            }
        }

        public async Task ArchiveProjectAsync(int projectId, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            bool shouldEdit = await context.Projects.AnyAsync(p => p.CompanyId == companyId && p.Id == projectId);


            if (shouldEdit)
            {
                Project? project = await context.Projects.FirstOrDefaultAsync(p  => p.CompanyId == companyId && p.Id == projectId);

                if (project is not null)
                {
                    project.IsArchived = true;
                    context.Projects.Update(project);
                    await context.SaveChangesAsync();
                }
            }
        }

        // this has been tested and works
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

        // this has been tested and works
        public async Task<IEnumerable<Project>> GetArchivedProjectsAsync(int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            IEnumerable<Project> projects = await context.Projects
                                                         .Where(p => p.IsArchived == true && p.CompanyId == companyId)
                                                         .Include(p => p.Tickets)
                                                         .Include(p => p.CompanyMembers)
                                                         .OrderByDescending(p => p.Created)
                                                         .ToListAsync();
            return projects;
        }

        public async Task<Project?> GetProjectByIdAsync(int projectId, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            Project? project = await context.Projects
                                                     .Include(p => p.Tickets)
                                                     .Include(p => p.CompanyMembers)
                                                     .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);
            return project;
        }

        public async Task RestoreProjectAsync(int projectId, int companyId)
        {
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            bool shouldEdit = await context.Projects.AnyAsync(p => p.CompanyId == companyId && p.Id == projectId);


            if (shouldEdit)
            {
                Project? project = await context.Projects.FirstOrDefaultAsync(p => p.CompanyId == companyId && p.Id == projectId);

                if (project is not null)
                {
                    project.IsArchived = false;
                    context.Projects.Update(project);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateProjectAsync(Project project, int companyId)
        {
            
            using ApplicationDbContext context = _dbContextFactory.CreateDbContext();

            bool shouldEdit = await context.Projects.AnyAsync(p => p.CompanyId == companyId && p.Id == project.Id);

            if (shouldEdit)
            {
                context.Projects.Update(project);
                await context.SaveChangesAsync();
            }
        }
    }
}
