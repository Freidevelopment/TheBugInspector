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
        private readonly ICompanyDTOService _compnayService;
        private readonly UserManager<ApplicationUser> _userManager;

        private int CompanyId => int.Parse(User.FindFirst("CompanyId")!.Value);

        private string UserId => _userManager.GetUserId(User)!;

        public CompanyController(ICompanyDTOService companyService, UserManager<ApplicationUser> userManager)
        {
            _compnayService = companyService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<CompanyDTO>> GetCompany()
        {
            CompanyDTO? company = await _compnayService.GetCompanyByIdAsync(CompanyId);

            if (company is null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpGet("{companyId:int}/members")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetCompanyMembers([FromRoute] int companyId)
        {

            try
            {
                IEnumerable<UserDTO> members = await _compnayService.GetCompanyMembersAsync(CompanyId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId:string}/role")]
        public async Task<ActionResult<string?>> GetUserRole([FromRoute] string userId)
        {
            try
            {
                string? role = await _compnayService.GetUserRoleAsync(userId, CompanyId);
                return Ok(role);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
