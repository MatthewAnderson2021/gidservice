using GudelIdService.Domain.Models;
using GudelIdService.Implementation.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Linq;
using GService.Common.Implementation;
using GService.Common.Auth;
namespace GudelIdService.Implementation.Controllers
{
    [ApiController]
    [Route("v1/statistics")]
    public class StatisticsController : ControllerBase
    {

        private readonly ILogger<StatisticsController> _logger;
        private readonly AppDbContext _dbContext;
        public StatisticsController(
            ILogger<StatisticsController> logger,
            AppDbContext dbContext
            )
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        [HttpGet("count/pools")]
        [Right(StaticRights.GET_GID_STATS)]
        public IActionResult GetPoolCount(string username)
        {
            return Ok(_dbContext.Pool.Count(x => string.IsNullOrEmpty(username) || x.CreatedBy == username));
        }

        [HttpGet("count/ids")]
        [Right(StaticRights.GET_GID_STATS)]
        public IActionResult GetIDCount(string byState, string username)
        {
            ArrayList counts = new ArrayList();

            if (string.IsNullOrEmpty(byState))
            {
                var count = _dbContext.GudelId.Count(x => string.IsNullOrEmpty(username) || x.CreatedBy == username);
                counts.Add(new { stateId = "", count });
                return Ok(counts);
            }

            if (string.IsNullOrEmpty(username))
            {
                return Ok(_dbContext.GudelId.GroupBy(x => x.StateId).Select(g => new { stateId = g.Key, count = g.Count() }).ToList());
            }

            counts.Add(_dbContext.GudelId.Where(x => x.CreatedBy == username)
                .Where(x => x.StateId == GudelIdStates.CreatedId)
                .GroupBy(x => x.StateId)
                .Select(g => new { stateId = g.Key, count = g.Count() }).FirstOrDefault());
            counts.Add(_dbContext.GudelId.Where(x => x.ReservedBy == username)
                .Where(x => x.StateId == GudelIdStates.ReservedId)
                .GroupBy(x => x.StateId)
                .Select(g => new { stateId = g.Key, count = g.Count() }).FirstOrDefault());
            counts.Add(_dbContext.GudelId.Where(x => x.ProducedBy == username)
                .Where(x => x.StateId == GudelIdStates.ProducedId)
                .GroupBy(x => x.StateId)
                .Select(g => new { stateId = g.Key, count = g.Count() }).FirstOrDefault());
            counts.Add(_dbContext.GudelId.Where(x => x.AssignedBy == username)
                .Where(x => x.StateId == GudelIdStates.AssignedId)
                .GroupBy(x => x.StateId)
                .Select(g => new { stateId = g.Key, count = g.Count() }).FirstOrDefault());
            counts.Add(_dbContext.GudelId.Where(x => x.VoidedBy == username)
                .Where(x => x.StateId == GudelIdStates.VoidedId)
                .GroupBy(x => x.StateId)
                .Select(g => new { stateId = g.Key, count = g.Count() }).FirstOrDefault());


            return Ok(counts);
        }
    }
}
