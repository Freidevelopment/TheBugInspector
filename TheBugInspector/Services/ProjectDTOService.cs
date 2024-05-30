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

        public Task<ProjectDTO> AddProjectAsync(ProjectDTO project, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task ArchiveProjectAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId)
        {
            IEnumerable<Project> projects = await _repository.GetAllProjectsAsync(companyId);

            IEnumerable<ProjectDTO> result = projects.Select(p => p.ToDTO());

            return result;
        }

        public Task<IEnumerable<ProjectDTO>> GetArchivedProjectsAsync(int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDTO?> GetProjectByIdAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task RestoreProjectAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProjectAsync(ProjectDTO project, int companyId)
        {
            throw new NotImplementedException();
        }
    }
}
