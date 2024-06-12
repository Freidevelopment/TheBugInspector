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
                CompanyMembers = await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>($"api/company/members") ?? [];
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
                ;
                HttpResponseMessage response = await _httpClient.GetAsync($"api/company/{userId}/role"); 
                response.EnsureSuccessStatusCode();

                string role = await response.Content.ReadAsStringAsync();
                return role;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            IEnumerable<UserDTO> UsersInRole = [];

            try
            {
                UsersInRole = await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>($"api/company/{roleName}/roles") ?? [];
                return UsersInRole;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return UsersInRole;
            }
        }

        public async Task UpdateCompanyAsync(CompanyDTO company, string adminId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/company/edit", company);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserRoleAsync(UserDTO user, string adminId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/company/roles", user);
            response.EnsureSuccessStatusCode();
        }
    }
}
