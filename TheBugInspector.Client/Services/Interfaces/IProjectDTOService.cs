using TheBugInspector.Client.Models;

namespace TheBugInspector.Client.Services.Interfaces
{
    public interface IProjectDTOService
    {
        #region Create
        Task<ProjectDTO> AddProjectAsync(ProjectDTO project, int companyId, string userId);
        #endregion

        #region Retrieve
        Task<PagedList<ProjectDTO>> GetAllProjectsAsync(int companyId, int page, int pageSize);
        Task<PagedList<ProjectDTO>> GetMyProjectsAsync(int companyId, string userId, int page, int pageSize);
        Task<PagedList<ProjectDTO>> GetArchivedProjectsAsync(int companyId, int page, int pageSize);
        Task<IEnumerable<ProjectDTO>> GetAllProjectsCountAsync(int companyId);
        Task<IEnumerable<ProjectDTO>> GetMyProjectsCountAsync(int companyId, string userId);
        Task<IEnumerable<ProjectDTO>> GetArchivedProjectsCountAsync(int companyId);
        Task<ProjectDTO?> GetProjectByIdAsync(int projectId, int companyId);
        #endregion

        #region Update
        Task UpdateProjectAsync(ProjectDTO project, int companyId);

        Task ArchiveProjectAsync(int projectId, int companyId);
        Task RestoreProjectAsync(int projectId, int companyId);
        #endregion

        #region Delete
        #endregion

        #region Project Members
        Task<IEnumerable<UserDTO>> GetProjectMembersAsync(int projectId, int companyId);

        Task<UserDTO?> GetProjectManagerAsync(int projectId, int companyId);

        Task AddMemberToProjectAsync(int projectId, string memberId, string managerId);

        Task RemoveMemberFromProjectAsync(int projectId, string memberId, string managerId);

        Task AssignProjectManagerAsync(int projectId, string memberId, string adminId);

        Task RemoveProjectManagerAsync(int projectId, string adminId);
        #endregion
    }
}
