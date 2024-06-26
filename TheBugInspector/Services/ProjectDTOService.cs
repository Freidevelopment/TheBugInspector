﻿using TheBugInspector.Client.Models;
using TheBugInspector.Client.Services.Interfaces;
using TheBugInspector.Data;
using TheBugInspector.Models;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class ProjectDTOService : IProjectDTOService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICompanyRepository _companyRepository;

        public ProjectDTOService(IProjectRepository projectRepository, ICompanyRepository companyRepository)
        {
            _projectRepository = projectRepository;
            _companyRepository = companyRepository;
        }

        public async Task AddMemberToProjectAsync(int projectId, string memberId, string managerId)
        {
            await _projectRepository.AddMemberToProjectAsync(projectId, memberId, managerId);
        }

        public async Task<ProjectDTO> AddProjectAsync(ProjectDTO project, int companyId, string userId)
        {
            Project newProject = new Project()
            { 
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Priority = project.Priority,
                Name = project.Name,
                Description = project.Description,
                CompanyId = companyId
            };

            Project createdProject = await _projectRepository.AddProjectAsync(newProject, companyId, userId);

            return createdProject.ToDTO();

        }

        public async Task ArchiveProjectAsync(int projectId, int companyId)
        {
           Project? project = await _projectRepository.GetProjectByIdAsync(projectId, companyId);
            
            if (project is not null)
            {
                await _projectRepository.ArchiveProjectAsync(projectId, project.CompanyId);
            }
        }

        public async Task AssignProjectManagerAsync(int projectId, string userId, string adminId)
        {
            await _projectRepository.AssignProjectManagerAsync(projectId, userId, adminId);
        }

        public async Task<PagedList<ProjectDTO>> GetAllProjectsAsync(int companyId, int page, int pageSize)
        {
            PagedList<Project> projects = await _projectRepository.GetAllProjectsAsync(companyId, page, pageSize);

            PagedList<ProjectDTO> results = new PagedList<ProjectDTO>()
            {
                Page = projects.Page,
                TotalItems = projects.TotalItems,
                TotalPages = projects.TotalPages,
                Data = projects.Data.Select(x => x.ToDTO()),
            };

            return results;
        }

        public async Task<PagedList<ProjectDTO>> GetMyProjectsAsync(int companyId, string userId, int page, int pageSize)
        {

            PagedList<Project> projects = await _projectRepository.GetMyProjectsAsync(companyId, userId, page, pageSize);

            PagedList<ProjectDTO> results = new PagedList<ProjectDTO>()
            {
                Page=projects.Page,
                TotalItems = projects.TotalItems,
                TotalPages = projects.TotalPages,
                Data = projects.Data.Select(x => x.ToDTO()),
            };


            return results;
        }

        public async Task<PagedList<ProjectDTO>> GetArchivedProjectsAsync(int companyId, int page, int pageSize)
        {
            PagedList<Project> projects = await _projectRepository.GetArchivedProjectsAsync(companyId, page, pageSize);

            PagedList<ProjectDTO> results = new PagedList<ProjectDTO>()
            {
                Page = projects.Page,
                TotalItems = projects.TotalItems,
                TotalPages = projects.TotalPages,
                Data = projects.Data.Select(x => x.ToDTO()),
            };

            return results;
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int projectId, int companyId)
        {
            Project? project = await _projectRepository.GetProjectByIdAsync(projectId, companyId);

            return project == null ? null : project.ToDTO();
        }

        public async Task<UserDTO?> GetProjectManagerAsync(int projectId, int companyId)
        {
            ApplicationUser? projectManager = await _projectRepository.GetProjectManagerAsync(projectId, companyId);
            if(projectManager == null) return null;

            UserDTO userDTO = projectManager.ToDTO();
            userDTO.Role = nameof(Roles.ProjectManager);

            return userDTO;
        }

        public async Task<IEnumerable<UserDTO>> GetProjectMembersAsync(int projectId, int companyId)
        {
            IEnumerable<ApplicationUser> members = await _projectRepository.GetProjectMembersAsync(projectId, companyId);

            List<UserDTO> result = [];

            foreach (ApplicationUser member in members)
            {
                UserDTO userDTO = member.ToDTO();
                userDTO.Role = await _companyRepository.GetUserRoleAsync(member.Id, companyId);
                result.Add(userDTO);
            }

            return result;
        }

        public async Task RemoveProjectManagerAsync(int projectId, string adminId)
        {
            await _projectRepository.RemoveProjectManagerAsync(projectId, adminId);
        }

        public async Task RemoveMemberFromProjectAsync(int projectId, string memberId, string managerId)
        {
            await _projectRepository.RemoveMemberFromProjectAsync(projectId, memberId, managerId);
        }

        public async Task RestoreProjectAsync(int projectId, int companyId)
        {
            Project? project = await _projectRepository.GetProjectByIdAsync(projectId, companyId);

            if (project is not null)
            {
                await _projectRepository.RestoreProjectAsync(projectId, project.CompanyId);
            }
        }

        public async Task UpdateProjectAsync(ProjectDTO project, int companyId)
        {
            Project? projectToUpdate = await _projectRepository.GetProjectByIdAsync(project.Id, companyId);

            if (projectToUpdate is not null)
            {
                projectToUpdate.StartDate = project.StartDate;
                projectToUpdate.EndDate = project.EndDate;
                projectToUpdate.Description = project.Description;

                projectToUpdate.Name = project.Name;

                projectToUpdate.Priority = project.Priority;

                projectToUpdate.Tickets = [];
                projectToUpdate.CompanyMembers = [];

                projectToUpdate.Company = null;

                await _projectRepository.UpdateProjectAsync(projectToUpdate, companyId);
            }
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsCountAsync(int companyId)
        {
            IEnumerable<Project> projects = await _projectRepository.GetAllProjectsCountAsync(companyId);

            IEnumerable<ProjectDTO> results = projects.Select(t => t.ToDTO());


            return results;
        }

        public async Task<IEnumerable<ProjectDTO>> GetMyProjectsCountAsync(int companyId, string userId)
        {
            IEnumerable<Project> projects = await _projectRepository.GetMyProjectsCountAsync(companyId, userId);

            IEnumerable<ProjectDTO> results = projects.Select(t => t.ToDTO());


            return results;
        }

        public async Task<IEnumerable<ProjectDTO>> GetArchivedProjectsCountAsync(int companyId)
        {
            IEnumerable<Project> projects = await _projectRepository.GetArchivedProjectsCountAsync(companyId);

            IEnumerable<ProjectDTO> results = projects.Select(t => t.ToDTO());
            

            return results;
        }
    }
}
