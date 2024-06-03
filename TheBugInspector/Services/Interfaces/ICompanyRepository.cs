using Bogus.DataSets;
using TheBugInspector.Data;

namespace TheBugInspector.Services.Interfaces
{
    public interface ICompanyRepository
    {
        #region Create
        Task AddUserToRoleAsync(string userId, string roleName, string adminId);
        #endregion

        #region Retrieve
        Task<Company?> GetCompanyByIdAsync(int id);

        Task<string?> GetUserRoleAsync(string userId, int companyId);

        Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName, int companyId);
        #endregion

        #region Update
        Task UpdateCompanyAsync(Company company, string adminId);
        #endregion

        #region Delete
        #endregion
    }
}
