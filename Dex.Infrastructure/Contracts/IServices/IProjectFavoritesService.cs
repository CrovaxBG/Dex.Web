using System.Collections.Generic;
using System.Threading.Tasks;
using Dex.Common.DTO;

namespace Dex.Infrastructure.Contracts.IServices
{
    public interface IProjectFavoritesService
    {
        Task AddFavorite(ProjectFavoritesDTO dto);
        Task RemoveFavorite(ProjectFavoritesDTO dto);
        Task<List<ProjectFavoritesDTO>> GetFavoritesByUser(string userId);
        Task<List<ProjectFavoritesDTO>> GetFavoritesByProject(int projectId);
    }
}