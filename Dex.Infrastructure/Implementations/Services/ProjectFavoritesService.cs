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
    public class ProjectFavoritesService : IProjectFavoritesService
    {
        private readonly HttpClient _client;

        public ProjectFavoritesService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task AddFavorite(ProjectFavoritesDTO dto)
        {
            await _client.PostAsJsonAsync(nameof(ProjectFavoritesController.AddFavorite), dto);
        }

        public async Task RemoveFavorite(ProjectFavoritesDTO dto)
        {
            await _client.DeleteAsJsonAsync(nameof(ProjectFavoritesController.RemoveFavorite), dto);
        }

        public async Task<List<ProjectFavoritesDTO>> GetFavoritesByUser(string userId)
        {
            var response =
                await _client.GetAsync($"{nameof(ProjectFavoritesController.GetFavoritesByUser)}?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<List<ProjectFavoritesDTO>>();
                return message;
            }

            return new List<ProjectFavoritesDTO>();
        }

        public async Task<List<ProjectFavoritesDTO>> GetFavoritesByProject(int projectId)
        {
            var response =
                await _client.GetAsync(
                    $"{nameof(ProjectFavoritesController.GetFavoritesByProject)}?projectId={projectId}");
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<List<ProjectFavoritesDTO>>();
                return message;
            }

            return new List<ProjectFavoritesDTO>();
        }
    }
}