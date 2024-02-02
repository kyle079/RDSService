using Microsoft.AspNetCore.Mvc;
using RDSServiceLibrary.Interfaces;
using RDSServiceLibrary.Models;
using Serilog;

namespace RDSService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RdsController : ControllerBase
    {
        private readonly IRdsSessionService _rdsSessionService;

        public RdsController(IRdsSessionService rdsSessionService)
        {
            _rdsSessionService = rdsSessionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveManagementServer()
        {
            try
            {
                Log.Information("Getting active management server");
                var activeManagementServer = await _rdsSessionService.GetActiveManagementServer();
                Log.Information("Active management server retrieved {ActiveManagementServer}", activeManagementServer);
                return Ok(activeManagementServer);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting active management server");
                return StatusCode(500, "Internal Server Error - Unable to get active management server");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSessions()
        {
            try
            {
                Log.Information("Getting RDS sessions");
                var sessions = await _rdsSessionService.GetSessions();
                Log.Information("RDS sessions retrieved {SessionCount}", sessions.Count);
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting RDS sessions");
                return StatusCode(500, "Internal Server Error - Unable to get RDS sessions");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DisconnectSession([FromBody] SessionInfo sessionInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionInfo.UnifiedSessionId))
                {
                    return BadRequest("Session ID is required");
                }

                if (string.IsNullOrEmpty(sessionInfo.HostServer))
                {
                    return BadRequest("Host Server is required");
                }
                Log.Information("Disconnecting session {UnifiedSessionId} on {HostServer}", sessionInfo.UnifiedSessionId,
                    sessionInfo.HostServer);
                await _rdsSessionService.DisconnectSession(sessionInfo);
                Log.Information("Session disconnected {UnifiedSessionId} on {HostServer}", sessionInfo.UnifiedSessionId,
                    sessionInfo.HostServer);
                return Ok("Session disconnected");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error - Unable to disconnect session");
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOffSession([FromBody] SessionInfo sessionInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionInfo.UnifiedSessionId))
                {
                    return BadRequest("Session ID is required");
                }

                if (string.IsNullOrEmpty(sessionInfo.HostServer))
                {
                    return BadRequest("Host Server is required");
                }

                Log.Information("Logging off session {UnifiedSessionId} on {HostServer}", sessionInfo.UnifiedSessionId,
                    sessionInfo.HostServer);
                await _rdsSessionService.LogOffSession(sessionInfo);
                Log.Information("Session logged off {UnifiedSessionId} on {HostServer}", sessionInfo.UnifiedSessionId,
                    sessionInfo.HostServer);
                return Ok("Session logged off");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error - Unable to log off session");
            }
        }
    }
}