using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Dex.Common.DTO;
using Dex.Common.Extensions;
using Dex.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dex.Infrastructure.Implementations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserClaimsController : ControllerBase
    {
        private readonly DexContext _context;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly IMapper _mapper;

        public UserClaimsController(DexContext context, UserManager<AspNetUsers> userManager, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper;
        }

        [HttpPost]
        [Route(nameof(AddUserClaim))]
        public async Task<IActionResult> AddUserClaim(AspNetUserClaimsDTO dto)
        {
            if (dto == null || !ModelState.IsValid || string.IsNullOrEmpty(dto.ClaimType) || string.IsNullOrEmpty(dto.ClaimValue))
            {
                return BadRequest();
            }

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == dto.UserId);
                var currentClaims = await _userManager.GetClaimsAsync(user);
                if (currentClaims.Any(c => c.Type == dto.ClaimType && c.Value == dto.ClaimValue))
                {
                    return Conflict();
                }

                var res = await _userManager.AddClaimAsync(user,
                    new Claim(dto.ClaimType.ToLower().ToUpperFirstChar(), dto.ClaimValue.ToLower().ToUpperFirstChar())); // normalize values

                if (res.Succeeded)
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route(nameof(RemoveUserClaimById))]
        public async Task<IActionResult> RemoveUserClaimById(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var entity = _context.UserClaims.FirstOrDefault(c => c.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                if (entity.User == null)
                {
                    return BadRequest();
                }

                var result = await _userManager.RemoveClaimAsync(entity.User, new Claim(entity.ClaimType, entity.ClaimValue));
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route(nameof(RemoveUserClaim))]
        public async Task<IActionResult> RemoveUserClaim(AspNetUserClaimsDTO dto)
        {
            if (dto == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == dto.UserId);
                if (user == null)
                {
                    return BadRequest();
                }

                var result = await _userManager.RemoveClaimAsync(user, new Claim(dto.ClaimType, dto.ClaimValue));
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route(nameof(GetUserClaim))]
        public IActionResult GetUserClaim(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                var claim = _context.UserClaims.FirstOrDefault(c => c.Id == id);
                if (claim == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<AspNetUserClaimsDTO>(claim));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route(nameof(GetUserClaimsByUserId))]
        public IActionResult GetUserClaimsByUserId(string userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            try
            {
                return Ok(_context.UserClaims
                    .Where(c => c.UserId == userId)
                    .Select(c => _mapper.Map<AspNetUserClaimsDTO>(c)));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
