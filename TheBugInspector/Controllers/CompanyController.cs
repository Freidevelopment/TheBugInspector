using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheBugInspector.Client.Models;
using TheBugInspector.Client.Services.Interfaces;
using TheBugInspector.Data;

namespace TheBugInspector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyDTOService _companyService;
        private readonly UserManager<ApplicationUser> _userManager;

        private int CompanyId => int.Parse(User.FindFirst("CompanyId")!.Value);

        private string UserId => _userManager.GetUserId(User)!;

        public CompanyController(ICompanyDTOService companyService, UserManager<ApplicationUser> userManager)
        {
            _companyService = companyService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<CompanyDTO>> GetCompany()
        {
            CompanyDTO? company = await _companyService.GetCompanyByIdAsync(CompanyId);

            if (company is null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpGet("members")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetCompanyMembers()
        {

            try
            {
                IEnumerable<UserDTO> members = await _companyService.GetCompanyMembersAsync(CompanyId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}/role")]
        public async Task<ActionResult<string?>> GetUserRole([FromRoute] string userId)
        {
            try
            {
                string? role = await _companyService.GetUserRoleAsync(userId, CompanyId);
                if (string.IsNullOrEmpty(role))
                {
                    return NotFound();
                }

                return Ok(role);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{roleName}/roles")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersInRole([FromRoute] string roleName)
        {
            try
            {
                IEnumerable<UserDTO> members = await _companyService.GetUsersInRoleAsync(roleName, CompanyId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("roles")]
        public async Task<IActionResult> UpdateUserRole([FromBody]  UserDTO userDTO)
        {
            bool InAdminRole = User.IsInRole("Admin");
            var user = await _userManager.GetUserAsync(User);

            if (InAdminRole && user.CompanyId == CompanyId)
            {

            try
            {
                await _companyService.UpdateUserRoleAsync(userDTO, UserId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
            }

            return BadRequest();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyDTO companyDTO)
        {
            bool InAdminRole = User.IsInRole("Admin");

            if (InAdminRole && companyDTO.Id == CompanyId)
            {

                try
                {
                    await _companyService.UpdateCompanyAsync(companyDTO, UserId);
                    return Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest();
        }
    }
}
