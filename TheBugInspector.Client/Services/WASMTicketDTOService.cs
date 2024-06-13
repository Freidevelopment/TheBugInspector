using System.Net.Http.Json;
using TheBugInspector.Client.Models;
using TheBugInspector.Client.Services.Interfaces;
using static System.Net.WebRequestMethods;

namespace TheBugInspector.Client.Services
{
    public class WASMTicketDTOService : ITicketDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMTicketDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddCommentAsync(TicketCommentDTO comment, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync<TicketCommentDTO>($"api/tickets/{comment.TicketId}/comments", comment);
            response.EnsureSuccessStatusCode();


        }

        public async Task<TicketDTO> AddTicketAsync(TicketDTO ticket, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/tickets", ticket);
            response.EnsureSuccessStatusCode();

            TicketDTO? createdTicket = await response.Content.ReadFromJsonAsync<TicketDTO>();
            return createdTicket!;
        }

        public async Task<TicketAttachmentDTO> AddTicketAttachment(TicketAttachmentDTO attachment, byte[] uploadData, string contentType, int companyId)
        {
            using var formData = new MultipartFormDataContent();
            formData.Headers.ContentDisposition = new("form-data");

            var fileContent = new ByteArrayContent(uploadData);
            fileContent.Headers.ContentType = new(contentType);

            if (string.IsNullOrWhiteSpace(attachment.FileName))
            {
                formData.Add(fileContent, "file");
            }
            else
            {
                formData.Add(fileContent, "file", attachment.FileName);
            }

            formData.Add(new StringContent(attachment.Id.ToString()), nameof(attachment.Id));
            formData.Add(new StringContent(attachment.FileName ?? string.Empty), nameof(attachment.FileName));
            formData.Add(new StringContent(attachment.Description ?? string.Empty), nameof(attachment.Description));
            formData.Add(new StringContent(DateTimeOffset.Now.ToString()), nameof(attachment.Created));
            formData.Add(new StringContent(attachment.UserId ?? string.Empty), nameof(attachment.UserId));
            formData.Add(new StringContent(attachment.TicketId.ToString()), nameof(attachment.TicketId));

            var res = await _httpClient.PostAsync($"api/tickets/{attachment.TicketId}/attachments", formData);
            res.EnsureSuccessStatusCode();

            var addedAttachment = await res.Content.ReadFromJsonAsync<TicketAttachmentDTO>();
            return addedAttachment!;
        }


        public async Task ArchiveTicketAsync(int ticketId, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/tickets/{ticketId}/active", ticketId);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCommentAsync(int commentId, int ticketId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/tickets/{ticketId}/{commentId}/comments");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTicketAttachment(int attachmentId, int companyId)
        {
            var res = await _httpClient.DeleteAsync($"api/tickets/attachments/{attachmentId}");
            res.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId)
        {
            IEnumerable<TicketDTO> tickets = [];

            try
            {
                tickets = await _httpClient.GetFromJsonAsync<IEnumerable<TicketDTO>>($"api/tickets") ?? [];
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return tickets;
            }
        }

        public async Task<IEnumerable<TicketDTO>> GetUserTicketsAsync(int companyId, string userId)
        {
            IEnumerable<TicketDTO> tickets = [];

            try
            {
                tickets = await _httpClient.GetFromJsonAsync<IEnumerable<TicketDTO>>($"api/tickets/personal") ?? [];
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return tickets;
            }
        }


        public async Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId)
        {
            TicketDTO? ticket = new();

            try
            {
                ticket = await _httpClient.GetFromJsonAsync<TicketDTO>($"api/tickets/{ticketId}");
                return ticket;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ticket;
            }
        }

        public async Task<TicketCommentDTO?> GetTicketCommentByIdAsync(int ticketId, int companyId)
        {
            TicketCommentDTO? comment = new();

            try
            {
                comment = await _httpClient.GetFromJsonAsync<TicketCommentDTO>($"api/tickets/{ticketId}/comment");
                return comment;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return comment;
            }
        }

        public async Task<IEnumerable<TicketCommentDTO>> GetTicketCommentsAsync(int ticketId, int companyId)
        {
            IEnumerable<TicketCommentDTO> comments = [];

            try
            {
                comments = await _httpClient.GetFromJsonAsync<IEnumerable<TicketCommentDTO>>($"api/tickets/{ticketId}/comments") ?? [];
                return comments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return comments;
            }
        }


        public async Task RestoreTicketAsync(int ticketId, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/tickets/{ticketId}/archived", ticketId);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCommentAsync(TicketCommentDTO comment, int companyId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/tickets/{comment.Id}/comments", comment);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateTicketAsync(TicketDTO ticket, int companyId, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/tickets/{ticket.Id}", ticket);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<TicketDTO>> GetAllArchivedTicketsAsync(int companyId)
        {
            IEnumerable<TicketDTO> tickets = [];

            try
            {
                tickets = await _httpClient.GetFromJsonAsync<IEnumerable<TicketDTO>>($"api/tickets/archived") ?? [];
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return tickets;
            }
        }

        public async Task<IEnumerable<TicketDTO>> GetArchivedUserTicketsAsync(int companyId, string userId)
        {
            IEnumerable<TicketDTO> tickets = [];

            try
            {
                tickets = await _httpClient.GetFromJsonAsync<IEnumerable<TicketDTO>>($"api/tickets/personal/archived") ?? [];
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return tickets;
            }
        }

        public Task<TicketAttachmentDTO?> GetTicketAttachmentById(int attachmentId, int companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TicketDTO>> GetMostRecentActiveTicketsAsync(int companyId)
        {
            IEnumerable<TicketDTO> tickets = [];

            try
            {
                tickets = await _httpClient.GetFromJsonAsync<IEnumerable<TicketDTO>>($"api/tickets/recent/active") ?? [];
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return tickets;
            }
        }

        public async Task<IEnumerable<TicketDTO>> GetMostRecentArchivedTicketsAsync(int companyId)
        {
            IEnumerable<TicketDTO> tickets = [];

            try
            {
                tickets = await _httpClient.GetFromJsonAsync<IEnumerable<TicketDTO>>($"api/tickets/recent/archived") ?? [];
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return tickets;
            }
        }

        public async Task<IEnumerable<TicketDTO>> GetRecentUserTicketsAsync(int companyId, string userId)
        {
            IEnumerable<TicketDTO> tickets = [];

            try
            {
                tickets = await _httpClient.GetFromJsonAsync<IEnumerable<TicketDTO>>($"api/tickets/personal/recent/active") ?? [];
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return tickets;
            }
        }

        public async Task<IEnumerable<TicketDTO>> GetRecentArchivedUserTicketsAsync(int companyId, string userId)
        {
            IEnumerable<TicketDTO> tickets = [];

            try
            {
                tickets = await _httpClient.GetFromJsonAsync<IEnumerable<TicketDTO>>($"api/tickets/personal/recent/archived") ?? [];
                return tickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return tickets;
            }
        }
    }
}
