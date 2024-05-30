using TheBugInspector.Client.Models;

namespace TheBugInspector.Client.Services.Interfaces
{
    public interface IProjectDTOService
    {
        #region Create
        Task<ProjectDTO> AddProjectAsync(ProjectDTO project, int companyId);
        #endregion

        #region Retrieve
        Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId);
        Task<IEnumerable<ProjectDTO>> GetArchivedProjectsAsync(int companyId);
        Task<ProjectDTO?> GetProjectByIdAsync(int projectId, int companyId);
        #endregion

        #region Update
        Task UpdateProjectAsync(ProjectDTO project, int companyId);

        Task ArchiveProjectAsync(int projectId, int companyId);
        Task RestoreProjectAsync(int projectId, int companyId);
        #endregion

        #region Delete
        #endregion
    }
}
