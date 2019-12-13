using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dex.Common.DTO;
using Dex.Common.Extensions;
using Dex.Infrastructure.Contracts.IServices;
using Dex.Infrastructure.Implementations.Controllers;

namespace Dex.Infrastructure.Implementations.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly HttpClient _client;

        public LoggerService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Task<int> Log(string message)
        {
            return Log(new LogDTO { Message = message });
        }

        public async Task<int> Log(LogDTO logData)
        {
            var response = await _client.PostAsJsonAsync(nameof(LoggerController.AddLog), logData);

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                if (int.TryParse(message, out var id))
                {
                    return id;
                }
            }

            return -1;
        }

        public async Task<LogDTO> GetLog(int id)
        {
            var response = await _client.GetAsync($"{nameof(LoggerController.GetLog)}?logId={id}");

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<LogDTO>();
                return message;
            }

            return null;
        }

        public async Task<List<LogDTO>> GetLogs()
        {
            var response = await _client.GetAsync(nameof(LoggerController.GetLogs));

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<List<LogDTO>>();
                return message;
            }

            return new List<LogDTO>();
        }
    }
}
