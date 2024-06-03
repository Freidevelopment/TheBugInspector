using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using TheBugInspector.Data;
using TheBugInspector.Services.Interfaces;

namespace TheBugInspector.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public CompanyRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public Task AddUserToRoleAsync(string userId, string roleName, string adminId)
        {
            throw new NotImplementedException();
        }

        public Task<Company?> GetCompanyByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetUserRoleAsync(string userId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCompanyAsync(Company company, string adminId)
        {
            throw new NotImplementedException();
        }
    }
}
