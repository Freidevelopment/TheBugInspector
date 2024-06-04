using System.Net.Http.Json;
using TheBugInspector.Client.Models;
using TheBugInspector.Client.Services.Interfaces;

namespace TheBugInspector.Client.Services
{
    public class WASMCompanyDTOService : ICompanyDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMCompanyDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task AddUserToRoleAsync(string userId, string roleName, string adminId)
        {
            throw new NotImplementedException();
        }

        public async Task<CompanyDTO?> GetCompanyByIdAsync(int id)
        {
            CompanyDTO? company = new();

            try
            {
                company = await _httpClient.GetFromJsonAsync<CompanyDTO>($"api/company");
                return company;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return company;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetCompanyMembersAsync(int companyId)
        {
            IEnumerable <UserDTO> CompanyMembers= [];

            try
            {
                CompanyMembers = await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>($"api/company/{companyId}/members") ?? [];
                return CompanyMembers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return CompanyMembers;
            }
        }

        public async Task<string?> GetUserRoleAsync(string userId, int companyId)
        {
            

            try
            {
                string? role = await _httpClient.GetFromJsonAsync<string>($"api/company/{userId}/role");
                
                return role;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        public Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCompanyAsync(CompanyDTO company, string adminId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserRoleAsync(UserDTO user, string adminId)
        {
            throw new NotImplementedException();
        }
    }
}
