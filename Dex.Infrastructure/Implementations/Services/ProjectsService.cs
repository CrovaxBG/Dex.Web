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
    public class ProjectsService : IProjectsService
    {
        private readonly string _host;

        public ProjectsService(IConfiguration configuration)
        {
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }

            _host = configuration.GetSection("Data").GetSection("Host").Value;
        }

        public async Task<int> AddProject(ProjectsDTO dto)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_host + "api/projects/") };

            var response = await client.PostAsJsonAsync("AddProject", dto);

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

        public async Task<int> ModifyProject(ProjectsDTO dto)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_host + "api/projects/") };

            var response = await client.PutAsJsonAsync("ModifyProject", dto);

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

        public async Task RemoveProject(int id)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_host + "api/projects/") };

            await client.DeleteAsync($"RemoveProject?id={id}");
        }

        public async Task<ProjectsDTO> GetProject(int id)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_host + "api/projects/") };

            var response = await client.GetAsync($"GetProject?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<ProjectsDTO>();
                return message;
            }

            return null;
        }

        public async Task<List<ProjectsDTO>> GetProjects()
        {
            using var client = new HttpClient { BaseAddress = new Uri(_host + "api/projects/") };

            var response = await client.GetAsync($"GetProjects");

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<List<ProjectsDTO>>();
                return message;
            }

            return null;
        }
    }
}