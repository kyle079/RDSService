using System.Management.Automation;
using RDSServiceLibrary.Models;

namespace RDSServiceLibrary.Interfaces
{
    /// <summary>
    /// Interface for RDS Session Service.
    /// </summary>
    public interface IRdsSessionService
    {
        /// <summary>
        /// Gets the active management server.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation, with a string result representing the active management server.</returns>
        Task<string> GetActiveManagementServer(string? connectionBroker = null);

        /// <summary>
        /// Gets the sessions from the specified active management server.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation, with an <see cref="PSObject"/> as the result representing the sessions.</returns>
        Task<List<RdsSession>> GetSessions(string? connectionBroker = null);

        /// <summary>
        /// Disconnects a session from the specified host server and active management server.
        /// </summary>
        /// <param name="sessionInfo">The <see cref="SessionInfo"/> object containing the user session to disconnect.</param>
        /// <param name="connectionBroker">The connection broker to perform the initial query. Defaults to local host if no value provided.</param>
        /// <returns>A Task representing the asynchronous operation, with a boolean result indicating whether the operation was successful.</returns>
        Task<bool> DisconnectSession(SessionInfo sessionInfo, string? connectionBroker = null);

        /// <summary>
        /// Logs off a session from the specified host server and active management server.
        /// </summary>
        /// <param name="sessionInfo">The <see cref="SessionInfo"/> object containing the user session to logoff.</param>
        /// <param name="connectionBroker">The connection broker to perform the initial query. Defaults to local host if no value provided.</param>
        /// <returns>A Task representing the asynchronous operation, with a boolean result indicating whether the operation was successful.</returns>
        Task<bool> LogOffSession(SessionInfo sessionInfo, string? connectionBroker = null);
    }
}