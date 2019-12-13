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
    public class LoggerController : ControllerBase
    {
        private readonly DexContext _context;
        private readonly IMapper _mapper;

        public LoggerController(DexContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [Route("GetLogs")]
        public IActionResult GetLogs()
        {
            try
            {
                return Ok(_context.Log.Select(_mapper.Map<LogDTO>));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetLog")]
        public IActionResult GetLog(int? logId)
        {
            if (logId == null) { return BadRequest(); }

            try
            {
                var log = _context.Log.FirstOrDefault(l => l.Id == logId);

                if (log == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<LogDTO>(log));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddLog")]
        public IActionResult AddLog(LogDTO log)
        {
            if (!ModelState.IsValid) { return BadRequest(); }
            try
            {
                log.Date = DateTime.Now;
                log.Id = 0;

                var entity = _mapper.Map<Log>(log);
                _context.Log.Add(entity);
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
    }
}