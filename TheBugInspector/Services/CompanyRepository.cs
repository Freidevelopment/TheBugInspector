﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheBugInspector.Data;
using TheBugInspector.Models;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class CompanyRepository(IDbContextFactory<ApplicationDbContext> contextFactory,
                                   IServiceProvider serviceProvider, IProjectRepository projectRepository) : ICompanyRepository
    {

        public async Task AddUserToRoleAsync(string userId, string roleName, string adminId)
        {
            if (userId == adminId) return;

            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? admin = await userManager.FindByIdAsync(adminId);

            if (admin is not null && await userManager.IsInRoleAsync(admin, nameof(Roles.Admin)))
            {
                ApplicationUser? user = await userManager.FindByIdAsync(userId);

                if (user is not null && user.CompanyId == admin.CompanyId)
                {
                    IList<string> currentRoles = await userManager.GetRolesAsync(user);
                    string? currentRole = currentRoles.FirstOrDefault(r => r != nameof(Roles.DemoUser));

                    if (string.Equals(currentRole, roleName, StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }

                    if (!string.IsNullOrEmpty(currentRole))
                    {
                        await userManager.RemoveFromRoleAsync(user, currentRole);

                        
                    }

                    await userManager.AddToRoleAsync(user, roleName);

                    bool isProjectManager = await userManager.IsInRoleAsync(user, nameof(Roles.ProjectManager));

                    if (isProjectManager)
                    {
                        IEnumerable<Project> projects = await projectRepository.GetMyProjectsCountAsync(user.CompanyId, user.Id);

                        foreach (Project project in projects)
                        {
                            await projectRepository.RemoveMemberFromProjectAsync(project.Id, user.Id, adminId);
                        }
                    }
                }
            }
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Company? company = await context.Companies
                                                      .Include(c => c.CompanyMembers)
                                                      .Include(c => c.Projects)
                                                      .Include(c => c.Invites)
                                                      .FirstOrDefaultAsync(c => c.Id == id);

            return company;
        }

        public async Task<string?> GetUserRoleAsync(string userId, int companyId)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? user = await userManager.FindByIdAsync(userId);

            string role = "Unknown";

            if (user?.CompanyId == companyId)
            {
                IList<string> roles = await userManager.GetRolesAsync(user);

                role = roles.FirstOrDefault(r => r != nameof(Roles.DemoUser), role);
            }

            return role;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IList<ApplicationUser> users = await userManager.GetUsersInRoleAsync(roleName);
            IEnumerable<ApplicationUser> companyUsers = users.Where(u => u.CompanyId == companyId);

            return companyUsers;
        }

        public async Task UpdateCompanyAsync(Company company, string adminId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();
          

            using IServiceScope scope = serviceProvider.CreateScope();
            UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? admin = await userManager.FindByIdAsync(adminId);

            if (admin is not null && await userManager.IsInRoleAsync(admin, nameof(Roles.Admin)))
            {
                FileUpload? oldImage = null;

                if(company.Image is not null)
                {
                    context.Images.Add(company.Image);

                    oldImage = await context.Images.FirstOrDefaultAsync(i => i.Id == company.ImageId);

                    company.ImageId = company.Image.Id;
                }

                try
                {
                    context.Companies.Update(company);

                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    
                }

                if (oldImage is not null)
                {
                    context.Images.Remove(oldImage);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
