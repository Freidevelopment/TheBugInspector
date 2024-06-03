using TheBugInspector.Client.Models;
using TheBugInspector.Client.Services.Interfaces;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class CompanyDTOService(ICompanyRepository repository) : ICompanyDTOService
    {
        public Task AddUserToRoleAsync(string userId, string roleName, string adminId)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyDTO?> GetCompanyByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetUserRoleAsync(string userId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCompanyAsync(CompanyDTO company, string adminId)
        {
            throw new NotImplementedException();
        }
    }
}
