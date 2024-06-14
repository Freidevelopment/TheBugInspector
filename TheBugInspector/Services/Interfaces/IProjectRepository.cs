using TheBugInspector.Client.Models;
using TheBugInspector.Data;
using TheBugInspector.Models;

namespace TheBugInspector.Services.Interfaces
{
    public interface IProjectRepository
    {
        #region Create
        Task<Project> AddProjectAsync(Project project, int companyId, string userId);
        #endregion

        #region Retrieve
        Task<PagedList<Project>> GetAllProjectsAsync(int companyId, int page, int pageSize);
        Task<PagedList<Project>> GetMyProjectsAsync(int companyId, string userId, int page, int pageSize);
        Task<PagedList<Project>> GetArchivedProjectsAsync(int companyId, int page, int pageSize);
        Task<IEnumerable<Project>> GetAllProjectsCountAsync(int companyId);
        Task<IEnumerable<Project>> GetMyProjectsCountAsync(int companyId, string userId);
        Task<IEnumerable<Project>> GetArchivedProjectsCountAsync(int companyId);
        Task<Project?> GetProjectByIdAsync(int projectId, int companyId);
        #endregion

        #region Update
        Task UpdateProjectAsync(Project project, int companyId);

        Task ArchiveProjectAsync(int projectId, int companyId);
        Task RestoreProjectAsync(int projectId, int companyId);
        #endregion

        #region Delete
        #endregion

        #region Project Members
        Task<IEnumerable<ApplicationUser>> GetProjectMembersAsync(int projectId, int companyId);

        Task<ApplicationUser?> GetProjectManagerAsync(int projectId, int companyId);

        Task AddMemberToProjectAsync(int projectId, string userId, string managerId);

        Task RemoveMemberFromProjectAsync(int projectId, string userId, string managerId);

        Task AssignProjectManagerAsync(int projectId, string userId, string adminId);

        Task RemoveProjectManagerAsync(int projectId, string adminId);
        #endregion
    }
}
