using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dex.Common.DTO;
using Dex.DataAccess.Models;
using Dex.Infrastructure.Contracts.IServices;
using Microsoft.AspNetCore.Http;
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
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("AddFavorite")]
        public IActionResult AddFavorite(ProjectFavoritesDTO dto)
        {
            if (ModelState.IsValid)
            {
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

            return BadRequest();
        }

        [HttpDelete]
        [Route("RemoveFavorite")]
        public IActionResult RemoveFavorite(ProjectFavoritesDTO dto)
        {
            if (dto == null) { return BadRequest(); }

            try
            {
                var entity = _context.ProjectFavorites.SingleOrDefault(p => p.ProjectId == dto.ProjectId && p.UserId == dto.UserId);
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
            if (_context == null || userId == null)
            {
                return NotFound();
            }

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
            if (_context == null || projectId == null)
            {
                return NotFound();
            }

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