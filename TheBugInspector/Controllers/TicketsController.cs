using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using TheBugInspector.Client.Models;
using TheBugInspector.Client.Services.Interfaces;
using TheBugInspector.Data;
using TheBugInspector.Helpers;
using TheBugInspector.Models;

namespace TheBugInspector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketDTOService _ticketService;
        private readonly IProjectDTOService _projectService;
        private readonly UserManager<ApplicationUser> _userManager;
        private int? _companyId => User.FindFirst("CompanyId") != null ? int.Parse(User.FindFirst("CompanyId")!.Value) : null;

        private string UserId => _userManager.GetUserId(User)!;
        public TicketsController(ITicketDTOService ticketService, IProjectDTOService projectService, UserManager<ApplicationUser> userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetAllTicketsAsync()
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user is not null && user.CompanyId == companyId)
            {

                IEnumerable<TicketDTO> tickets = [];
                try
                {
                    tickets = await _ticketService.GetAllTicketsAsync(companyId);
                    return Ok(tickets);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpGet("archived")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetAllArchivedTicketsAsync()
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user is not null && user.CompanyId == companyId)
            {

                IEnumerable<TicketDTO> tickets = [];
                try
                {
                    tickets = await _ticketService.GetAllArchivedTicketsAsync(companyId);
                    return Ok(tickets);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpGet("personal")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetUserTicketsAsync()
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user is not null && user.CompanyId == companyId)
            {

                IEnumerable<TicketDTO> tickets = [];
                try
                {
                    tickets = await _ticketService.GetUserTicketsAsync(companyId, UserId);
                    return Ok(tickets);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpGet("personal/archived")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetUserArchivedTicketsAsync()
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user is not null && user.CompanyId == companyId)
            {

                try
                {
                    IEnumerable<TicketDTO> tickets = [];
                    tickets = await _ticketService.GetArchivedUserTicketsAsync(companyId, UserId);
                    return Ok(tickets);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpGet("{ticketId:int}/comments")]
        public async Task<ActionResult<IEnumerable<TicketCommentDTO>>> GetTicketCommentsAsync([FromRoute] int ticketId)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user is not null && user.CompanyId == companyId)
            {

                IEnumerable<TicketCommentDTO> comments = [];
                try
                {
                    comments = await _ticketService.GetTicketCommentsAsync(ticketId, companyId);
                    return Ok(comments);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [HttpGet("{ticketId:int}/comment")]
        public async Task<ActionResult<TicketCommentDTO>> GetTicketCommentByIdAsync([FromRoute] int ticketId)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user is not null && user.CompanyId == companyId)
            {

                TicketCommentDTO? comment = new();
                try
                {
                    comment = await _ticketService.GetTicketCommentByIdAsync(ticketId, companyId);
                    return Ok(comment);
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
        public async Task<ActionResult<TicketDTO>> GetTicketById([FromRoute] int id)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user is not null && user.CompanyId == companyId)
            {

                try
                {
                    TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(id, companyId);
                    return Ok(ticket);
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
        public async Task<ActionResult<TicketDTO>> CreateTicket([FromBody] TicketDTO ticketDTO)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user is not null && user.CompanyId == companyId)
            {
                ProjectDTO? project = await _projectService.GetProjectByIdAsync(ticketDTO.ProjectId, companyId);

                if (project is not null && project.CompanyMembers.Any(t => t.UserId == UserId) || User.IsInRole("Admin"))
                {

                    try
                    {
                        TicketDTO createdTicket = await _ticketService.AddTicketAsync(ticketDTO, companyId);

                        return Ok(createdTicket);
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

        [HttpPost("{ticketId:int}/comments")]
        public async Task<IActionResult> CreateComment([FromRoute] int ticketId,
                                                        [FromBody] TicketCommentDTO commentDTO)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user is not null && user.CompanyId == companyId)
            {

                TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(ticketId, companyId);
                if (ticket is not null)
                {
                    UserDTO? manager = await _projectService.GetProjectManagerAsync(ticket.ProjectId, companyId);


                    if (manager is not null && manager.UserId == UserId ||
                       ticket.SubmitterUserId == UserId ||
                       ticket.DeveloperUserId == UserId ||
                       User.IsInRole("Admin"))
                    {

                        try
                        {
                            await _ticketService.AddCommentAsync(commentDTO, companyId);

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
            return BadRequest();
        }

        // POST: api/Tickets/5/attachments
        // NOTE: the parameters are decorated with [FromForm] because they will be sent
        // encoded as multipart/form-data and NOT the typical JSON
        [HttpPost("{id}/attachments")]
        public async Task<ActionResult<TicketAttachmentDTO>> PostTicketAttachment(int id,
                                                                                    [FromForm] TicketAttachmentDTO attachment,
                                                                                    [FromForm] IFormFile? file)
        {
            if (attachment.TicketId != id || file is null)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);
            var ticket = await _ticketService.GetTicketByIdAsync(id, user!.CompanyId);
            if (ticket is not null)
            {
                ProjectDTO? project = await _projectService.GetProjectByIdAsync(ticket.ProjectId, user.CompanyId);
                if (project is not null)
                {
                    UserDTO? manager = await _projectService.GetProjectManagerAsync(project.Id, user.CompanyId);


                    if (user.CompanyId == _companyId)
                    {

                        if (ticket is null)
                        {
                            return NotFound();
                        }

                        if (User.IsInRole("Admin") ||
                           manager is not null && manager.UserId == UserId ||
                           ticket.SubmitterUserId == UserId ||
                           ticket.DeveloperUserId == UserId)
                        {

                            attachment.UserId = user!.Id;
                            attachment.Created = DateTimeOffset.Now;

                            if (string.IsNullOrWhiteSpace(attachment.FileName))
                            {
                                attachment.FileName = file.FileName;
                            }

                            // ImageHelper was renamed to UploadHelper!
                            FileUpload upload = await FileHelper.GetFileUploadAsync(file);

                            try
                            {
                                var newAttachment = await _ticketService.AddTicketAttachment(attachment, upload.Data!, upload.Type!, user!.CompanyId);
                                return Ok(newAttachment);
                            }
                            catch
                            {
                                return Problem();
                            }
                        }
                        return BadRequest();
                    }
                    return BadRequest();
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("{ticketId:int}")]
        public async Task<IActionResult> UpdateTicket([FromRoute] int ticketId,
                                                      [FromBody] TicketDTO ticket)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user is not null && user.CompanyId == companyId)
            {


                ProjectDTO? project = await _projectService.GetProjectByIdAsync(ticket.ProjectId, user.CompanyId);
                if (project is not null)
                {

                    UserDTO? manager = await _projectService.GetProjectManagerAsync(project.Id, user.CompanyId);

                    if (User.IsInRole("Admin") ||
                       manager is not null && manager.UserId == UserId ||
                       ticket.SubmitterUserId == UserId ||
                       ticket.DeveloperUserId == UserId)
                    {


                        try
                        {
                            await _ticketService.UpdateTicketAsync(ticket, companyId, UserId);
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
            return BadRequest();
        }

        [HttpPut("{commentId:int}/comments")]
        public async Task<IActionResult> UpdateComment([FromRoute] int commentId,
                                                       [FromBody] TicketCommentDTO ticketCommentDTO)
        {

            int companyId = _companyId ?? 0;
            string userId = _userManager.GetUserId(User)!;
            var user = await _userManager.GetUserAsync(User);

            if (user is not null && user.CompanyId == companyId)
            {

                TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(ticketCommentDTO.TicketId, companyId);
                if (ticket != null)
                {

                    UserDTO? manager = await _projectService.GetProjectManagerAsync(ticket.ProjectId, companyId);


                    bool InAdminRole = User.IsInRole("Admin");
                    bool InPMRole = manager is not null && manager.UserId == userId;
                    bool IsSubmitter = UserId == ticketCommentDTO.UserId;

                    if (InAdminRole || InPMRole || IsSubmitter)
                    {

                        try
                        {
                            await _ticketService.UpdateCommentAsync(ticketCommentDTO, companyId);
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            return BadRequest(ex.Message);
                        }
                    }
                }
            }

            return BadRequest();

        }

        [HttpPut("{ticketId:int}/archived")]
        public async Task<IActionResult> RestoreTicket([FromRoute] int ticketId)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user is not null && user.CompanyId == companyId)
            {
                TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(ticketId, companyId);
                if (ticket is not null)
                {
                    UserDTO? manager = await _projectService.GetProjectManagerAsync(ticket.ProjectId, companyId);


                    if (User.IsInRole("Admin") || manager is not null && manager.UserId == UserId)
                        try
                        {
                            await _ticketService.RestoreTicketAsync(ticketId, companyId);
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

        [HttpPut("{ticketId:int}/active")]
        public async Task<IActionResult> ArchiveTicket([FromRoute] int ticketId)
        {
            int companyId = _companyId ?? 0;
            var user = await _userManager.GetUserAsync(User);

            if (user is not null && user.CompanyId == companyId)
            {
                TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(ticketId, companyId);
                if (ticket is not null)
                {

                    UserDTO? manager = await _projectService.GetProjectManagerAsync(ticket.ProjectId, companyId);
                    if (User.IsInRole("Admin") || manager is not null && manager.UserId == UserId)
                    {

                        try
                        {
                            await _ticketService.ArchiveTicketAsync(ticketId, companyId);
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
            return BadRequest();
        }

        [HttpDelete("{ticketId:int}/{commentId:int}/comments")]
        public async Task<IActionResult> DeleteComment([FromRoute] int ticketId,
                                                       [FromRoute] int commentId)
        {
            int companyId = _companyId ?? 0;
            string userId = _userManager.GetUserId(User)!;
            var user = await _userManager.GetUserAsync(User);

            if (user is not null && user.CompanyId == companyId)
            {

                TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(ticketId, companyId);
                if (ticket is not null)
                {

                    UserDTO? manager = await _projectService.GetProjectManagerAsync(ticket.ProjectId, companyId);
                    TicketCommentDTO? comment = await _ticketService.GetTicketCommentByIdAsync(ticketId, companyId);

                    if (comment is not null)
                    {
                        if (userId == comment.UserId || User.IsInRole("Admin") || manager is not null && manager.UserId == UserId)
                        {
                            try
                            {
                                await _ticketService.DeleteCommentAsync(commentId, ticketId);
                                return NoContent();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                                return BadRequest(ex.Message);
                            }
                        }
                    }

                    return BadRequest();
                }
                return BadRequest();
            }

            return BadRequest();

        }

        // DELETE: api/Tickets/attachments/1
        [HttpDelete("attachments/{attachmentId}")]
        public async Task<IActionResult> DeleteTicketAttachment(int attachmentId)
        {
            var user = await _userManager.GetUserAsync(User);
            int companyId = _companyId ?? 0;

            if (user is not null && user.CompanyId == companyId)
            {
                TicketAttachmentDTO? attachment = await _ticketService.GetTicketAttachmentById(attachmentId, companyId);
                if (attachment is not null)
                {

                    TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(attachment.TicketId, companyId);
                    if (ticket is not null)
                    {

                        UserDTO? manager = await _projectService.GetProjectManagerAsync(ticket.ProjectId, companyId);

                        if (User.IsInRole("Admin") ||
                            manager is not null && manager.UserId == UserId ||
                            attachment.UserId == UserId)
                        {
                            await _ticketService.DeleteTicketAttachment(attachmentId, user!.CompanyId);

                            return NoContent();

                        }
                        return BadRequest();
                    }
                    return BadRequest();
                }
                return BadRequest();
            }

            return BadRequest();
        }
    }
}
