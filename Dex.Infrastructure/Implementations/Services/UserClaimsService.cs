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
    public class UserClaimsService : IUserClaimsService
    {
        private readonly HttpClient _client;

        public UserClaimsService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task AddUserClaim(AspNetUserClaimsDTO dto)
        {
            await _client.PostAsJsonAsync(nameof(UserClaimsController.AddUserClaim), dto);
        }

        public async Task RemoveUserClaimById(int id)
        {
            await _client.DeleteAsync($"{nameof(UserClaimsController.RemoveUserClaimById)}?id={id}");
        }

        public async Task RemoveUserClaim(AspNetUserClaimsDTO dto)
        {
            await _client.DeleteAsJsonAsync(nameof(UserClaimsController.RemoveUserClaim), dto);
        }

        public async Task<AspNetUserClaimsDTO> GetUserClaim(int id)
        {
            var response = await _client.GetAsync($"{nameof(UserClaimsController.GetUserClaim)}?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<AspNetUserClaimsDTO>();
                return message;
            }

            return null;
        }

        public async Task<List<AspNetUserClaimsDTO>> GetUserClaimsByUserId(string userId)
        {
            var response = await _client.GetAsync($"{nameof(UserClaimsController.GetUserClaimsByUserId)}?userId={userId}");
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsJsonAsync<List<AspNetUserClaimsDTO>>();
                return message;
            }

            return new List<AspNetUserClaimsDTO>();
        }
    }
}
