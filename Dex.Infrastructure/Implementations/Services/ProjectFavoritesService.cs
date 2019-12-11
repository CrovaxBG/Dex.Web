using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dex.Common.DTO;
using Dex.Common.Extensions;
using Dex.Infrastructure.Contracts.IServices;
using Dex.Infrastructure.Implementations.Controllers;
using Microsoft.Extensions.Configuration;

namespace Dex.Infrastructure.Implementations.Services
{
    public class ProjectFavoritesService : IProjectFavoritesService
    {
        private readonly string _host;
        private const string ControllerAddress = "api/projectfavorites/";

        public ProjectFavoritesService(IConfiguration configuration)
        {
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }

            _host = configuration.GetSection("Data").GetSection("Host").Value;
        }

        public async Task AddFavorite(ProjectFavoritesDTO dto)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_host + ControllerAddress) };

            await client.PostAsJsonAsync(nameof(ProjectFavoritesController.AddFavorite), dto);
        }

        public async Task RemoveFavorite(ProjectFavoritesDTO dto)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_host + ControllerAddress) };

            await client.DeleteAsJsonAsync(nameof(ProjectFavoritesController.RemoveFavorite), dto);
        }

        public async Task<List<ProjectFavoritesDTO>> GetFavoritesByUser(string userId)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_host + ControllerAddress) };
            var response = await client.GetAsync($"{nameof(ProjectFavoritesController.GetFavoritesByUser)}?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<List<ProjectFavoritesDTO>>();
                return message;
            }

            return null;
        }

        public async Task<List<ProjectFavoritesDTO>> GetFavoritesByProject(int projectId)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_host + ControllerAddress) };

            var response = await client.GetAsync($"{nameof(ProjectFavoritesController.GetFavoritesByProject)}?projectId={projectId}");

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<List<ProjectFavoritesDTO>>();
                return message;
            }

            return null;
        }
    }
}