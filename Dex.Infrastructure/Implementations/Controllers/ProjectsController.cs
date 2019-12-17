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
    public class ProjectsController : ControllerBase
    {
        private readonly DexContext _context;
        private readonly IMapper _mapper;

        public ProjectsController(DexContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Route(nameof(AddProject))]
        public IActionResult AddProject(ProjectsDTO dto)
        {
            if (dto == null || !ModelState.IsValid) { return BadRequest(); }

            try
            {
                var entity = _mapper.Map<Projects>(dto);
                _context.Projects.Add(entity);
                _context.SaveChanges();

                if (entity.Id > 0)
                {
                    return Ok(entity.Id);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPut]
        [Route(nameof(ModifyProject))]
        public IActionResult ModifyProject(ProjectsDTO dto)
        {
            if (dto == null || !ModelState.IsValid) { return BadRequest(); }

            try
            {
                var entity = _context.Projects.FirstOrDefault(p => p.Id == dto.Id);
                if (entity == null)
                {
                    return NotFound();
                }

                _mapper.Map(dto, entity);
                _context.SaveChanges();

                return Ok(entity.Id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route(nameof(RemoveProject))]
        public IActionResult RemoveProject(int? id)
        {
            if(id == null) { return BadRequest(); }

            var entity = _context.Projects.FirstOrDefault(p => p.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                _context.Projects.Remove(entity);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route(nameof(GetProject))]
        public IActionResult GetProject(int? id)
        {
            if (id == null) { return BadRequest(); }

            try
            {
                var project = _context.Projects.FirstOrDefault(p => p.Id == id);
                if (project == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<ProjectsDTO>(project));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route(nameof(GetProjects))]
        public IActionResult GetProjects()
        {
            try
            {
                return Ok(_context.Projects.Select(p => _mapper.Map<ProjectsDTO>(p)));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}