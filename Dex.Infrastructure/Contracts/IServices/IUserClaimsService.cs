using System.Collections.Generic;
using System.Threading.Tasks;
using Dex.Common.DTO;

namespace Dex.Infrastructure.Contracts.IServices
{
    public interface IUserClaimsService
    {
        Task AddUserClaim(AspNetUserClaimsDTO dto);
        Task RemoveUserClaimById(int id);
        Task RemoveUserClaim(AspNetUserClaimsDTO dto);
        Task<AspNetUserClaimsDTO> GetUserClaim(int id);
        Task<List<AspNetUserClaimsDTO>> GetUserClaimsByUserId(string userId);
    }
}