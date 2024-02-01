using RDSService.Models;

namespace RDSService.Interfaces
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
        Task<string> GetActiveManagementServer();

        /// <summary>
        /// Gets the sessions from the specified active management server.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation, with an <see cref="PSObject"/> as the result representing the sessions.</returns>
        Task<List<RdsSession>> GetSessions();

        /// <summary>
        /// Disconnects a session from the specified host server and active management server.
        /// </summary>
        /// <param name="sessionId">The ID of the session to disconnect.</param>
        /// <param name="hostServer">The host server from which to disconnect the session.</param>
        /// <returns>A Task representing the asynchronous operation, with a boolean result indicating whether the operation was successful.</returns>
        Task<bool> DisconnectSession(SessionInfo sessionInfo);

        /// <summary>
        /// Logs off a session from the specified host server and active management server.
        /// </summary>
        /// <param name="sessionId">The ID of the session to log off.</param>
        /// <param name="hostServer">The host server from which to log off the session.</param>
        /// <returns>A Task representing the asynchronous operation, with a boolean result indicating whether the operation was successful.</returns>
        Task<bool> LogOffSession(SessionInfo sessionInfo);
    }
}