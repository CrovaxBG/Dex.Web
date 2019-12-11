using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dex.Common.DTO;

namespace Dex.Infrastructure.Contracts.IServices
{
    public interface ILoggerService
    {
        Task<int> Log(LogDTO logData);
        Task<int> Log(string message);
        Task<LogDTO> GetLog(int id);
        Task<List<LogDTO>> GetLogs();
    }
}
