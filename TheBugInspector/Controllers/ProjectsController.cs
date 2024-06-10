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

        private string UserId => _userManager.GetUserId(User)!;

        public ProjectsController(IProjectDTOService projectService, UserManager<ApplicationUser> userManager)
        {
            _projectService = projectService;
            _userManager = userManager;
        }



        [HttpGet("active")] // api/projects/active
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAllProjectsAsync()
        {
            // ensuring that the companyId is the correct users company Id
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user.CompanyId == companyId)
            {

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
            return BadRequest();
        }

        [HttpGet("personal")] // api/projects/personal
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetMyProjectsAsync()
        {
            // ensuring that the companyId is the correct users company Id
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user.CompanyId == companyId)
            {

                try
                {
                    IEnumerable<ProjectDTO> projects = await _projectService.GetMyProjectsAsync(companyId, UserId);
                    return Ok(projects);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpGet("archived")] // api/projects/1/archived
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetArchivedProjectsAsync()
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user.CompanyId == companyId)
            {

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
            return BadRequest();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectById([FromRoute] int id)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user.CompanyId == companyId)
            {

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
            return BadRequest();

        }

        [HttpGet("{projectId:int}/manager")]
        public async Task<ActionResult<UserDTO>> GetProjectManager([FromRoute] int projectId)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user.CompanyId == companyId)
            {

                try
                {
                    UserDTO? manager = await _projectService.GetProjectManagerAsync(projectId, companyId);
                    return Ok(manager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest();
                }
            }
            return BadRequest();

        }

        [HttpGet("{projectId:int}/members")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetProjectMembers([FromRoute] int projectId)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user.CompanyId == companyId)
            {

                try
                {
                    IEnumerable<UserDTO> members = await _projectService.GetProjectMembersAsync(projectId, companyId);
                    return Ok(members);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> CreateProject([FromBody] ProjectDTO project)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user.CompanyId == companyId)
            {

                bool InAdminRole = User.IsInRole("Admin");
                bool InPMRole = User.IsInRole("ProjectManager");

                UserDTO? manager = await _projectService.GetProjectManagerAsync(project.Id, companyId);

                if (InAdminRole || InPMRole && manager.UserId == UserId)
                {

                    try
                    {
                        ProjectDTO createdProject = await _projectService.AddProjectAsync(project, companyId, UserId);

                        return Ok(createdProject);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return BadRequest();
                    }
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("{projectId:int}")] // api/projects/5
        public async Task<IActionResult> UpdateProject([FromRoute] int projectId,
                                                       [FromBody] ProjectDTO project)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user.CompanyId == companyId)
            {

                bool InAdminRole = User.IsInRole("Admin");
                bool InPMRole = User.IsInRole("ProjectManager");

                UserDTO? manager = await _projectService.GetProjectManagerAsync(project.Id, companyId);
                if (InAdminRole || InPMRole && manager.UserId == UserId)
                {
                    try
                    {
                        await _projectService.UpdateProjectAsync(project, companyId);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return BadRequest();
                    }

                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("{projectId:int}/archived")] // api/projects/5/archived
        public async Task<IActionResult> RestoreProject([FromRoute] int projectId)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user.CompanyId == companyId)
            {

                bool InAdminRole = User.IsInRole("Admin");
                bool InPMRole = User.IsInRole("ProjectManager");

                UserDTO? manager = await _projectService.GetProjectManagerAsync(projectId, companyId);
                if (!InAdminRole || InPMRole && manager.UserId == UserId)
                {
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
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("{projectId:int}/active")]
        public async Task<IActionResult> ArchiveProject([FromRoute] int projectId)
        {
            int companyId = _companyId ?? 0;

            var user = await _userManager.GetUserAsync(User);

            if (user.CompanyId == companyId)
            {

                bool InAdminRole = User.IsInRole("Admin");
                bool InPMRole = User.IsInRole("ProjectManager");

                UserDTO? manager = await _projectService.GetProjectManagerAsync(projectId, companyId);
                if (InAdminRole || InPMRole && manager.UserId == UserId)
                {
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
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("{projectId:int}/manager")]
        public async Task<IActionResult> RemoveManagerFromProject([FromRoute] int projectId)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user.CompanyId == companyId)
            {

                bool InAdminRole = User.IsInRole("Admin");

                if (InAdminRole)
                {

                    try
                    {
                        await _projectService.RemoveProjectManagerAsync(projectId, UserId);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return Problem();
                    }
                }

                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("{projectId:int}/members")]
        public async Task<IActionResult> RemoveMemberFromProject([FromRoute] int projectId,
                                                                 [FromBody] string userId)
        {
            int companyId = _companyId ?? 0;

            var user = await _userManager.GetUserAsync(User);

            if (user.CompanyId == companyId)
            {

                bool InAdminRole = User.IsInRole("Admin");
                bool InPMRole = User.IsInRole("ProjectManager");

                UserDTO? manager = await _projectService.GetProjectManagerAsync(projectId, companyId);

                if (InAdminRole || InPMRole && manager.UserId == UserId)
                {

                    try
                    {
                        await _projectService.RemoveMemberFromProjectAsync(projectId, userId, UserId);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return Problem();
                    }
                }

                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("{projectId:int}/members/add")]
        public async Task<IActionResult> AssignMemberToProject([FromRoute] int projectId,
                                                                 [FromBody] string userId)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user.CompanyId == companyId)
            {

                bool InAdminRole = User.IsInRole("Admin");
                bool InPMRole = User.IsInRole("ProjectManager");

                UserDTO? manager = await _projectService.GetProjectManagerAsync(projectId, companyId);

                if (InAdminRole || InPMRole && manager.UserId == UserId)
                {

                    try
                    {
                        await _projectService.AddMemberToProjectAsync(projectId, userId, UserId);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return Problem();
                    }
                }

                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("{projectId:int}/manager/add")]
        public async Task<IActionResult> AssignManagerToProject([FromRoute] int projectId,
                                                                 [FromBody] string userId)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user.CompanyId == companyId)
            {

                bool InAdminRole = User.IsInRole("Admin");

                if (InAdminRole)
                {

                    try
                    {
                        await _projectService.AssignProjectManagerAsync(projectId, userId, UserId);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return Problem();
                    }
                }

                return BadRequest();
            }
            return BadRequest();
        }
    }
}
