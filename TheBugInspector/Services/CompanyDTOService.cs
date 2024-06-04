using TheBugInspector.Client.Models;
using TheBugInspector.Client.Services.Interfaces;
using TheBugInspector.Data;
using TheBugInspector.Models;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class CompanyDTOService(ICompanyRepository repository) : ICompanyDTOService
    {
        public Task AddUserToRoleAsync(string userId, string roleName, string adminId)
        {
            throw new NotImplementedException();
        }

        public async Task<CompanyDTO?> GetCompanyByIdAsync(int id)
        {
            Company? company = await repository.GetCompanyByIdAsync(id);

            return company?.ToDTO();
        }

        public async Task<IEnumerable<UserDTO>> GetCompanyMembersAsync(int companyId)
        {
            Company? company = await repository.GetCompanyByIdAsync(companyId);

            if (company is null) return [];

            List<UserDTO> members = [];

            foreach(ApplicationUser user in company.CompanyMembers)
            {
                UserDTO member = user.ToDTO();
                member.Role = await repository.GetUserRoleAsync(user.Id, companyId);
                members.Add(member);
            }

            return members;
        }

        public Task<string?> GetUserRoleAsync(string userId, int companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            IEnumerable<ApplicationUser> users = await repository.GetUsersInRoleAsync(roleName, companyId);

            IEnumerable<UserDTO> userDTOs = users.Select(u => u.ToDTO());

            foreach(UserDTO user in userDTOs)
            {
                user.Role = roleName;
            }

            return userDTOs;
        }

        public Task UpdateCompanyAsync(CompanyDTO company, string adminId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserRoleAsync(UserDTO user, string adminId)
        {
            if (string.IsNullOrEmpty(user.Role)) return;

            await repository.AddUserToRoleAsync(user.UserId!, user.Role, adminId);
        }
    }
}
