using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Net;
using System.Net.Sockets;

namespace AppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InfoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetInfo()
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var appServerIp = GetLocalIPAddress();
            var dbIp = await GetDatabaseIP();

            return Ok(new
            {
                ClientIP = clientIp,
                AppServerIP = appServerIp,
                DatabaseIP = dbIp
            });
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Local IP Address Not Found!";
        }

        private async Task<string> GetDatabaseIP()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection") ?? "";
            if (string.IsNullOrEmpty(connectionString)) return "Connection String Not Configured";

            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                using var cmd = new NpgsqlCommand("SELECT inet_server_addr()", conn);
                var result = await cmd.ExecuteScalarAsync();
                return result?.ToString() ?? "Unknown";
            }
            catch (Exception ex)
            {
                // Fallback: If DB is not reachable, try to extract IP from connection string
                try 
                {
                    var builder = new NpgsqlConnectionStringBuilder(connectionString);
                    return builder.Host ?? "Unknown (DB Error: " + ex.Message + ")";
                }
                catch
                {
                    return "Error: " + ex.Message;
                }
            }
        }
    }
}
