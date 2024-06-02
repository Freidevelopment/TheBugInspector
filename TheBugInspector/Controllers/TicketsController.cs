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
    public class TicketsController : ControllerBase
    {
        private readonly ITicketDTOService _ticketService;
        private readonly UserManager<ApplicationUser> _userManager;
        private int? _companyId => User.FindFirst("CompanyId") != null ? int.Parse(User.FindFirst("CompanyId")!.Value) : null;

        public TicketsController(ITicketDTOService ticketService, UserManager<ApplicationUser> userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetAllTicketsAsync() 
        { 
            int companyId = _companyId ?? 0;

            try
            {
                IEnumerable<TicketDTO> tickets = await _ticketService.GetAllTicketsAsync(companyId);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TicketDTO>> GetTicketById([FromRoute] int id)
        {
            int companyId = _companyId ?? 0;

            try
            {
                TicketDTO? ticket = await _ticketService.GetTicketByIdAsync(id, companyId);
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        [HttpPost]
        public async Task<ActionResult<TicketDTO>> CreateTicket([FromBody] TicketDTO ticketDTO)
        {
            int companyId = _companyId ?? 0;

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

        [HttpPut("{ticketId:int}")]
        public async Task<IActionResult> UpdateTicket([FromRoute] int ticketId,
                                                      [FromBody] TicketDTO ticket)
        {
            int companyId = _companyId ?? 0;
            string userId = _userManager.GetUserId(User)!;


            try
            {
                await _ticketService.UpdateTicketAsync(ticket, companyId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        [HttpPut("{ticketId:int}/archived")]
        public async Task<IActionResult> RestoreTicket([FromRoute] int ticketId)
        {
            int companyId = _companyId ?? 0;


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

        [HttpPut("{ticketId:int}/active")]
        public async Task<IActionResult> ArchiveTicket([FromRoute] int ticketId)
        {
            int companyId = _companyId ?? 0;


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
    }
}
