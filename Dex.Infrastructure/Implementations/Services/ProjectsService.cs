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
    public class ProjectsService : IProjectsService
    {
        private readonly HttpClient _client;

        public ProjectsService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<int> AddProject(ProjectsDTO dto)
        {
            var response = await _client.PostAsJsonAsync(nameof(ProjectsController.AddProject), dto);
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
            var response = await _client.PutAsJsonAsync(nameof(ProjectsController.ModifyProject), dto);
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
            await _client.DeleteAsync($"{nameof(ProjectsController.RemoveProject)}?id={id}");
        }

        public async Task<ProjectsDTO> GetProject(int id)
        {
            var response = await _client.GetAsync($"{nameof(ProjectsController.GetProject)}?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<ProjectsDTO>();
                return message;
            }
            return null; //Should consider NullObject pattern
        }

        public async Task<List<ProjectsDTO>> GetProjects()
        {
            var response = await _client.GetAsync(nameof(ProjectsController.GetProjects));
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<List<ProjectsDTO>>();
                return message;
            }
            return new List<ProjectsDTO>();
        }
    }
}