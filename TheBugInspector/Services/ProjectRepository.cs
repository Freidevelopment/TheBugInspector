﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheBugInspector.Client.Models;
using TheBugInspector.Data;
using TheBugInspector.Helpers.Extensions;
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

            if (isAdmin == false)
            {
                ApplicationUser? projectManager = await GetProjectManagerAsync(projectId, manager.CompanyId);

                if ((projectManager?.Id != managerId)) return;
            }

            ApplicationUser? userToAdd = await context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.CompanyId == manager.CompanyId);
            if (userToAdd is null) return;

            bool userIsProjectManager = await userManager.IsInRoleAsync(userToAdd, nameof(Roles.ProjectManager));
            if (userIsProjectManager) return;

            bool userIsAdmin = await userManager.IsInRoleAsync(userToAdd, nameof(Roles.Admin));
            if (userIsAdmin == true) return;

            Project? project = await context.Projects
                                                     .Include(p => p.CompanyMembers)
                                                     .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == manager.CompanyId);

            if (project is null) return;

            if (project.CompanyMembers.Any(m => m.Id == userToAdd.Id) == false)
            {
                project.CompanyMembers.Add(userToAdd);
                await context.SaveChangesAsync();
            }
        }

        // this has been tested and works
        public async Task<Project> AddProjectAsync(Project project, int companyId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? user = await userManager.FindByIdAsync(userId);

            bool shouldCreate = await context.Companies.AnyAsync(c => c.Id == companyId);

            if (shouldCreate)
            {

                project.Created = DateTimeOffset.Now;
                context.Projects.Add(project);
                await context.SaveChangesAsync();

                bool isPM = user is not null && await userManager.IsInRoleAsync(user, nameof(Roles.ProjectManager));

                if (isPM)
                {
                    int createdProjectId = project.Id;
                    Project? newProject = await context.Projects.FirstOrDefaultAsync(p => p.Id == createdProjectId);
                    if (newProject is not null)
                    {
                        await AssignProjectManagerAsync(createdProjectId, userId, user.Id);
                    }

                }

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
                Project? project = await context.Projects.Include(p => p.Tickets)
                                .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.Id == projectId);

                if (project is not null)
                {
                    project.IsArchived = true;
                    project.Tickets.Where(p => p.ProjectId == project.Id);
                    foreach (Ticket ticket in project.Tickets)
                    {
                        
                        ticket.IsArchivedByProject = true;
                    }
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

            if (isAdmin == true || (isPM == true && userId == adminId))
            {
                ApplicationUser? projectManager = await context.Users.FindAsync(userId);

                if (projectManager is not null
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
        public async Task<PagedList<Project>> GetAllProjectsAsync(int companyId, int page, int pageSize)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            PagedList<Project> projects = await context.Projects
                                                         .Where(p => p.IsArchived == false && p.CompanyId == companyId)
                                                         .Include(p => p.Tickets)
                                                         .Include(p => p.CompanyMembers)
                                                         .OrderByDescending(p => p.Created)
                                                         .ToPagedListAsync(page, pageSize);
            return projects;
        }

        public async Task<PagedList<Project>> GetMyProjectsAsync(int companyId, string userId, int page, int pageSize)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            PagedList<Project> projects = await context.Projects
                                                         .Where(p => p.IsArchived == false && p.CompanyId == companyId && p.CompanyMembers.Any(c => c.Id == userId))
                                                         .Include(p => p.Tickets)
                                                         .Include(p => p.CompanyMembers)
                                                         .OrderByDescending(p => p.Created)
                                                         .ToPagedListAsync(page, pageSize);



            return projects;
        }

        // this has been tested and works
        public async Task<PagedList<Project>> GetArchivedProjectsAsync(int companyId, int page, int pageSize)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            PagedList<Project> projects = await context.Projects
                                                         .Where(p => p.IsArchived == true && p.CompanyId == companyId)
                                                         .Include(p => p.Tickets)
                                                         .Include(p => p.CompanyMembers)
                                                         .OrderByDescending(p => p.Created)
                                                         .ToPagedListAsync(page, pageSize);
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

            foreach (ApplicationUser member in projectMembers)
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

            if (await userManager.IsInRoleAsync(admin, nameof(Roles.Admin)))
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
                Project? project = await context.Projects.Include(p => p.Tickets)
                                                         .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.Id == projectId);

                if (project is not null)
                {
                    project.IsArchived = false;
                    project.Tickets.Where(p => p.ProjectId == project.Id);
                    foreach(Ticket ticket in project.Tickets)
                    {
                        ticket.IsArchivedByProject = false;
                    }
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

        public async Task<IEnumerable<Project>> GetAllProjectsCountAsync(int companyId)
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

        public async Task<IEnumerable<Project>> GetMyProjectsCountAsync(int companyId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Project> projects = await context.Projects
                                                         .Where(p => p.IsArchived == false && p.CompanyId == companyId && p.CompanyMembers.Any(c => c.Id == userId))
                                                         .Include(p => p.Tickets)
                                                         .Include(p => p.CompanyMembers)
                                                         .OrderByDescending(p => p.Created)
                                                         .ToListAsync();



            return projects;
        }

        public async Task<IEnumerable<Project>> GetArchivedProjectsCountAsync(int companyId)
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
    }
}
