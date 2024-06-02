using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectDTOService _projectService;
        private readonly UserManager<ApplicationUser> _userManager;
        private int? _companyId => User.FindFirst("CompanyId") != null ? int.Parse(User.FindFirst("CompanyId")!.Value) : null;

        public ProjectsController(IProjectDTOService projectService, UserManager<ApplicationUser> userManager)
        {
            _projectService = projectService;
            _userManager = userManager;
        }
        


        [HttpGet("{companyId:int}/active")] // api/projects/1/active
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAllProjectsAsync([FromRoute] int companyId)
        {
            // ensuring that the companyId is the correct users company Id
             companyId = _companyId ?? 0;

            try
            {
                IEnumerable<ProjectDTO> projects = await _projectService.GetAllProjectsAsync(companyId);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{companyId}/archived")] // api/projects/1/archived
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetArchivedProjectsAsync([FromRoute] int companyId)
        {
            companyId = _companyId ?? 0;

            try
            {
                IEnumerable<ProjectDTO> projects = await _projectService.GetArchivedProjectsAsync(companyId);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectById([FromRoute] int id)
        { 
            int companyId = _companyId ?? 0;
            try
            {
                ProjectDTO? project = await _projectService.GetProjectByIdAsync(id, companyId);
                return Ok(project);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> CreateProject([FromBody] ProjectDTO project)
        {
            int companyId = _companyId ?? 0;
            try
            {
                ProjectDTO createdProject = await _projectService.AddProjectAsync(project, companyId);

                return Ok(createdProject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }

        [HttpPut("{projectId:int}")] // api/projects/5
        public async Task<IActionResult> UpdateProject([FromRoute] int projectId,
                                                       [FromBody] ProjectDTO project)
        {
            int companyId = _companyId ?? 0;

            try
            {
                await _projectService.UpdateProjectAsync(project, companyId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        [HttpPut("{projectId:int}/archived")] // api/projects/5/archived
        public async Task<IActionResult> RestoreProject([FromRoute] int projectId)
        {
            int companyId = _companyId ?? 0;

            try
            {
                await _projectService.RestoreProjectAsync(projectId, companyId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        [HttpPut("{projectId:int}/active")]
        public async Task<IActionResult> ArchiveProject([FromRoute] int projectId)
        {
            int companyId = _companyId ?? 0;

            try
            {
                await _projectService.ArchiveProjectAsync(projectId, companyId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }
    }
}
