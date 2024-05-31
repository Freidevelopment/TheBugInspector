using TheBugInspector.Client.Models;
using TheBugInspector.Client.Services.Interfaces;
using TheBugInspector.Models;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class ProjectDTOService : IProjectDTOService
    {
        private readonly IProjectRepository _repository;

        public ProjectDTOService(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectDTO> AddProjectAsync(ProjectDTO project, int companyId)
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

            Project createdProject = await _repository.AddProjectAsync(newProject, companyId);

            return createdProject.ToDTO();

        }

        public async Task ArchiveProjectAsync(int projectId, int companyId)
        {
           Project? project = await _repository.GetProjectByIdAsync(projectId, companyId);
            
            if (project is not null)
            {
                await _repository.ArchiveProjectAsync(projectId, project.CompanyId);
            }
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId)
        {
            IEnumerable<Project> projects = await _repository.GetAllProjectsAsync(companyId);

            IEnumerable<ProjectDTO> result = projects.Select(p => p.ToDTO());

            return result;
        }

        public async Task<IEnumerable<ProjectDTO>> GetArchivedProjectsAsync(int companyId)
        {
            IEnumerable<Project> projects = await _repository.GetArchivedProjectsAsync(companyId);

            IEnumerable<ProjectDTO> result = projects.Select(p => p.ToDTO());

            return result;
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int projectId, int companyId)
        {
            Project? project = await _repository.GetProjectByIdAsync(projectId, companyId);

            return project == null ? null : project.ToDTO();
        }

        public async Task RestoreProjectAsync(int projectId, int companyId)
        {
            Project? project = await _repository.GetProjectByIdAsync(projectId, companyId);

            if (project is not null)
            {
                await _repository.RestoreProjectAsync(projectId, project.CompanyId);
            }
        }

        public async Task UpdateProjectAsync(ProjectDTO project, int companyId)
        {
            Project? projectToUpdate = await _repository.GetProjectByIdAsync(project.Id, companyId);

            if (projectToUpdate is not null)
            {
                projectToUpdate.StartDate = project.StartDate;
                projectToUpdate.EndDate = project.EndDate;
                projectToUpdate.Description = project.Description;

                projectToUpdate.Name = project.Name;

                projectToUpdate.Priority = project.Priority;

                await _repository.UpdateProjectAsync(projectToUpdate, companyId);
            }
        }
    }
}
