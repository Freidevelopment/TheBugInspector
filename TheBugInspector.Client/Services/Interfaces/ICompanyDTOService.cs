﻿using TheBugInspector.Client.Models;

namespace TheBugInspector.Client.Services.Interfaces
{
    public interface ICompanyDTOService
    {
        #region Create
        Task AddUserToRoleAsync(string userId, string roleName, string adminId);
        #endregion

        #region Retrieve
        Task<CompanyDTO?> GetCompanyByIdAsync(int id);

        Task<string?> GetUserRoleAsync(string userId, int companyId);

        Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName, int companyId);
        #endregion

        #region Update
        Task UpdateCompanyAsync(CompanyDTO company, string adminId);
        #endregion

        #region Delete
        #endregion
    }
}
