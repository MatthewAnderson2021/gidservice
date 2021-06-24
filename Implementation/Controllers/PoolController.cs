using GService.Common.Implementation;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Repositories;
using GudelIdService.Implementation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GService.Common.Auth;
namespace GudelIdService.Implementation.Controllers
{
    [ApiController]
    [Route("v1/pools")]
    public class PoolController : ControllerBase
    {
        private readonly ILogger<PoolController> _logger;
        private readonly IPoolService _poolService;

        public PoolController(
            ILogger<PoolController> logger,
            IPoolService poolService
            )
        {
            _logger = logger;
            _poolService = poolService;
        }

        [HttpGet]
        [Right(StaticRights.GET_GID_POOL)]
        public async Task<IActionResult> FindAll([FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            return Ok(await _poolService.FindAll(language));
        }

        [HttpGet("{poolId}")]
        [Right(StaticRights.GET_GID_POOL)]
        public async Task<IActionResult> FindById(int poolId, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            var pool = await _poolService.FindById(poolId, language);

            if (pool == null)
            {
                return new NotFoundResult();
            }

            return Ok(pool);
        }

        [HttpPut("{poolId}")]
        [Right(StaticRights.UPDATE_GID)]
        public async Task<IActionResult> ModifyPool(int poolId, [FromBody] PoolData pool, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            var currentPool = await _poolService.FindById(poolId, language);
            if (currentPool == null)
            {
                return NotFound(new { Message = $"Pool with ID {poolId} could not be found." });
            }
            var update = await _poolService.Update(pool, language);

            return Ok(update);
        }

        [HttpDelete("{poolId}")]
        [Right(StaticRights.DELETE_GID_POOL)]
        public async Task<IActionResult> DeletePool(int poolId) 
        {
            var pool = await _poolService.FindById(poolId, null);
            if (pool == null)
            {
                return NotFound(new { Message = $"Pool with ID {poolId} could not be found." });
            }

            await _poolService.Remove(poolId);
            return Ok();
        }

        [HttpPost]
        [Right(StaticRights.CREATE_GID_POOL)]
        public async Task<IActionResult> CreatePool([FromBody] PoolData pool, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            pool.CreatedBy = HttpContext.GetAuthUserIdOrNull();
            var result = await _poolService.AddAsync(pool, language);
            return Created("/", result);
        }
    }
}