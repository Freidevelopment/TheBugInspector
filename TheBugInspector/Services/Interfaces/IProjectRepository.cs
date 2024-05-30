using TheBugInspector.Models;

namespace TheBugInspector.Services.Interfaces
{
    public interface IProjectRepository
    {
        #region Create
        Task<Project> AddProjectAsync(Project project, int companyId);
        #endregion

        #region Retrieve
        Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId);
        Task<IEnumerable<Project>> GetArchivedProjectsAsync(int companyId);
        Task<Project?> GetProjectByIdAsync(int projectId, int companyId);
        #endregion

        #region Update
        Task UpdateProjectAsync(Project project, int companyId);

        Task ArchiveProjectAsync(int projectId, int companyId);
        Task RestoreProjectAsync(int projectId, int companyId);
        #endregion

        #region Delete
        #endregion
    }
}
