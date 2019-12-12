using System;
using System.Linq;
using AutoMapper;
using Dex.Common.DTO;
using Dex.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using DexContext = Dex.DataAccess.Models.DexContext;

namespace Dex.Infrastructure.Implementations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectFavoritesController : ControllerBase
    {
        private readonly DexContext _context;
        private readonly IMapper _mapper;

        public ProjectFavoritesController(DexContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Route("AddFavorite")]
        public IActionResult AddFavorite(ProjectFavoritesDTO dto)
        {
            if (dto == null || !ModelState.IsValid) { return BadRequest(); }

            try
            {
                var entity = _mapper.Map<ProjectFavorites>(dto);
                _context.ProjectFavorites.Add(entity);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("RemoveFavorite")]
        public IActionResult RemoveFavorite(ProjectFavoritesDTO dto)
        {
            if (dto == null || !ModelState.IsValid) { return BadRequest(); }

            try
            {
                var entity =
                    _context.ProjectFavorites.SingleOrDefault(p =>
                        p.ProjectId == dto.ProjectId && p.UserId == dto.UserId);
                if (entity == null)
                {
                    return NotFound();
                }

                _context.ProjectFavorites.Remove(entity);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetFavoritesByUser")]
        public IActionResult GetFavoritesByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId)) { return BadRequest(); }

            try
            {
                return Ok(_context.ProjectFavorites.Where(p => p.UserId == userId)
                    .Select(p => _mapper.Map<ProjectFavoritesDTO>(p)));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetFavoritesByProject")]
        public IActionResult GetFavoritesByProject(int? projectId)
        {
            if (projectId == null) { return BadRequest(); }

            try
            {
                return Ok(_context.ProjectFavorites.Where(p => p.ProjectId == projectId)
                    .Select(p => _mapper.Map<ProjectFavoritesDTO>(p)));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}