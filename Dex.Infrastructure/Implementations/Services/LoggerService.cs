using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dex.Common.DTO;
using Dex.Common.Extensions;
using Dex.Infrastructure.Contracts.IServices;
using Microsoft.Extensions.Configuration;

namespace Dex.Infrastructure.Implementations.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly string _host;

        public LoggerService(IConfiguration configuration)
        {
            if(configuration == null) { throw new ArgumentNullException(nameof(configuration)); }

            _host = configuration.GetSection("Data").GetSection("Host").Value;
        }

        public async Task<int> Log(LogDTO logData)
        {
            using var client = new HttpClient {BaseAddress = new Uri(_host + "api/logger/")};

            var response = await client.PostAsJsonAsync("AddLog", logData);

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

        public Task<int> Log(string message)
        {
            return Log(new LogDTO {Message = message});
        }

        public async Task<LogDTO> GetLog(int id)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_host + "api/logger/") };

            var response = await client.GetAsync($"GetLog?logId={id}");

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<LogDTO>();
                return message;
            }

            return null;
        }

        public async Task<List<LogDTO>> GetLogs()
        {
            using var client = new HttpClient { BaseAddress = new Uri(_host + "api/logger/") };

            var response = await client.GetAsync($"GetLogs");

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<List<LogDTO>>();
                return message;
            }

            return null;
        }
    }
}
