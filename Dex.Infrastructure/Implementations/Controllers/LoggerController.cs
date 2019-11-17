using System;
using System.Linq;
using AutoMapper;
using Dex.Common.DTO;
using Dex.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

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
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetLogs")]
        public IActionResult GetLogs()
        {
            try
            {
                if (_context == null)
                {
                    return NotFound();
                }

                return Ok(_context.Log.Select(_mapper.Map<Log>));
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

                return Ok(_mapper.Map<Log>(log));
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
            if (ModelState.IsValid)
            {
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

                    return NotFound();
                }
                catch (Exception)
                {
                    return BadRequest();
                }

            }

            return BadRequest();
        }
    }
}