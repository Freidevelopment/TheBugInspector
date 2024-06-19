using TheBugInspector.Models;

namespace TheBugInspector.Services.Interfaces
{
    public interface IInviteRepository
    {

        /// <summary>
        /// Creates a new invite record in the database
        /// </summary>
        /// <param name="inviteeEmail">The email of the person who should receive this invite</param>
        /// <param name="inviteeFirstName">The first name of the person who is being invited</param>
        /// <param name="inviteeLastName">The last name of the person who is being invited</param>
        /// <param name="message">The message to be sent with the invitation</param>
        /// <param name="projectId">The project this person is being invited to work on</param>
        /// <param name="invitorId">The ID of the user creating the invite</param>
        /// <returns>The created Invite object</returns>
        Task<Invite> CreateInvite(string inviteeEmail, string inviteeFirstName, string inviteeLastName, string message, int projectId, string invitorId);

        /// <summary>
        /// Sends an invite to the invitee via email
        /// </summary>
        /// <param name="inviteId">The ID of the invite to send</param>
        /// <param name="companyId">The ID of the company the invite belongs to</param>
        /// <returns>True if the invite was successfully sent or false if the invite failed to send</returns>
        Task<bool> SendInvite(int inviteId, int companyId);

        /// <summary>
        /// Accepts encrypted values sent with an invite and returns the decrypted invite if the values are valid
        /// </summary>
        /// <param name="protectedToken">The encrypted token invite</param>
        /// <param name="protectedEmail">The encrypted email of the invitee</param>
        /// <param name="protectedCompanyId">The encrypted company ID the invite belongs to</param>
        /// <returns></returns>
        Task<Invite?> GetValidInviteAsync(string protectedToken, string protectedEmail, string protectedCompanyId);

        /// <summary>
        /// Marks an invite as accepted and adds the invitee to the project, then saves an updated Invite object in the database
        /// </summary>
        /// <param name="inviteId">The ID of the invite to mark as accepted</param>
        /// <param name="inviteeId">The User ID of the account that was registered with the invite</param>
        /// <returns></returns>
        Task AcceptInviteAsync(int inviteId, string inviteeId);

    }
}
