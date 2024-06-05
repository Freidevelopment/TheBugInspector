using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheBugInspector.Data;
using TheBugInspector.Models;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class ProjectRepository(IDbContextFactory<ApplicationDbContext> contextFactory,
                                   IServiceProvider serviceProvider) : IProjectRepository
    {

        public async Task AddMemberToProjectAsync(int projectId, string userId, string managerId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


            ApplicationUser? manager = await userManager.FindByIdAsync(managerId);
            if (manager is null) return;

            bool isAdmin = await userManager.IsInRoleAsync(manager, nameof(Roles.Admin));

            if(isAdmin == false)
            {
                ApplicationUser? projectManager = await GetProjectManagerAsync(projectId, manager.CompanyId);

                if ((projectManager?.Id != managerId)) return;
            }

            ApplicationUser? userToAdd = await context.Users.FirstOrDefaultAsync(u =>  u.Id == userId && u.CompanyId == manager.CompanyId);
            if (userToAdd is null) return;

            bool userIsProjectManager = await userManager.IsInRoleAsync(userToAdd, nameof(Roles.ProjectManager));
            if (userIsProjectManager == false) return;

            bool userIsAdmin = await userManager.IsInRoleAsync(userToAdd, nameof(Roles.Admin));
            if (userIsAdmin == true) return;

            Project? project = await context.Projects
                                                     .Include(p => p.CompanyMembers)
                                                     .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == manager.CompanyId);

            if (project is null) return;

            if(project.CompanyMembers.Any(m => m.Id == userToAdd.Id) ==  false)
            {
                project.CompanyMembers.Add(userToAdd);
                await context.SaveChangesAsync();
            }
        }

        // this has been tested and works
        public async Task<Project> AddProjectAsync(Project project, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();


            bool shouldCreate = await context.Companies.AnyAsync(c => c.Id == companyId);

            if (shouldCreate)
            {

                project.Created = DateTimeOffset.Now;
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
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldEdit = await context.Projects.AnyAsync(p => p.CompanyId == companyId && p.Id == projectId);


            if (shouldEdit)
            {
                Project? project = await context.Projects.FirstOrDefaultAsync(p => p.CompanyId == companyId && p.Id == projectId);

                if (project is not null)
                {
                    project.IsArchived = true;
                    context.Projects.Update(project);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task AssignProjectManagerAsync(int projectId, string userId, string adminId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? admin = await userManager.FindByIdAsync(adminId);
            if (admin is null) return;

            bool isAdmin = admin is not null && await userManager.IsInRoleAsync(admin, nameof(Roles.Admin));
            bool isPM = admin is not null && await userManager.IsInRoleAsync(admin, nameof(Roles.ProjectManager));

            if(isAdmin == true || (isPM == true && userId == adminId))
            {
                ApplicationUser? projectManager = userId == adminId ? admin : await userManager.FindByIdAsync(userId);

                if(projectManager is not null
                   && projectManager.CompanyId == admin!.CompanyId
                   && await userManager.IsInRoleAsync(projectManager, nameof(Roles.ProjectManager)))
                {
                    await RemoveProjectManagerAsync(projectId, adminId);

                    Project? project = await context.Projects
                                                              .Include(p => p.CompanyMembers)
                                                              .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == admin.CompanyId);

                    if (project is not null) 
                    { 
                        project.CompanyMembers.Add(projectManager);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }

        // this has been tested and works
        public async Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

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
            using ApplicationDbContext context = contextFactory.CreateDbContext();

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
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Project? project = await context.Projects
                                                     .Include(p => p.Tickets)
                                                        .ThenInclude(p => p.SubmitterUser)
                                                     .Include(p => p.CompanyMembers)
                                                     .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);
            return project;
        }

        public async Task<ApplicationUser?> GetProjectManagerAsync(int projectId, int companyId)
        {
            IEnumerable<ApplicationUser> projectMembers = await GetProjectMembersAsync(projectId, companyId);

            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach(ApplicationUser member in projectMembers)
            {
                bool isProjectManager = await userManager.IsInRoleAsync(member, nameof(Roles.ProjectManager));

                if (isProjectManager == true)
                {
                    return member;
                }
            }

            return null;
        }

        public async Task<IEnumerable<ApplicationUser>> GetProjectMembersAsync(int projectId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Project? project = await context.Projects
                                                     .Include(p => p.CompanyMembers)
                                                     .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);

            return project?.CompanyMembers ?? [];
        }

        public async Task RemoveProjectManagerAsync(int projectId, string adminId)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? admin = await userManager.FindByIdAsync(adminId);
            if (admin is null) return;

            ApplicationUser? projectManager = await GetProjectManagerAsync(projectId, admin.CompanyId);

            if (projectManager is null) return;

            if(await userManager.IsInRoleAsync(admin, nameof(Roles.Admin)))
            {
                await RemoveMemberFromProjectAsync(projectId, projectManager.Id, adminId);
            }
        }

        public async Task RemoveMemberFromProjectAsync(int projectId, string userId, string managerId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


            ApplicationUser? manager = await userManager.FindByIdAsync(managerId);
            if (manager is null) return;

            Project? project = await context.Projects
                                                     .Include(p => p.CompanyMembers)
                                                     .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == manager.CompanyId);
            if (project is null) return;

            ApplicationUser? memberToRemove = project.CompanyMembers.FirstOrDefault(p => p.Id == userId);
            if (memberToRemove is null) return;

            project.CompanyMembers.Remove(memberToRemove);
            await context.SaveChangesAsync();
        }

        public async Task RestoreProjectAsync(int projectId, int companyId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

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

            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldEdit = await context.Projects.AnyAsync(p => p.CompanyId == companyId && p.Id == project.Id);

            if (shouldEdit)
            {
                context.Projects.Update(project);
                await context.SaveChangesAsync();
            }
        }
    }
}
