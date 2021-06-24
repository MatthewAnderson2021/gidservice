using GService.Common.Implementation;
using GService.Common.Models;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Models;
using GudelIdService.Domain.Repositories;
using GudelIdService.Domain.Services;
using GudelIdService.Implementation.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GService.Common.Auth;
namespace GudelIdService.Implementation.Controllers
{
    [ApiController]
    [Route("v1/ids")]
    public class GudelIdController : ControllerBase
    {
        private readonly IGudelIdService _gudelIdService;
        private readonly IPoolService _poolService;

        public GudelIdController(IGudelIdService gudelIdService, IPoolService poolService)
        {
            _gudelIdService = gudelIdService;
            _poolService = poolService;
        }

        [HttpGet("query")]
        [Right(StaticRights.GET_GID)]
        public async Task<IActionResult> QueryGudelIds(
            string gudelId,
            int? state,
            int? type,
            int? pool,
            string createdBy,
            string reservedBy,
            string producedBy,
            string assignedBy,
            string voidedBy,
            DateTime? createdOn,
            DateTime? reservedOn,
            DateTime? producedOn,
            DateTime? assignedOn,
            DateTime? voidedOn,
            int pageSize = 100,
            int page = 0)
        {

            if (pageSize < 1) return BadRequest("Page size has to be positive.");
            if (pageSize > 20000) return BadRequest("Page size is limited to 20,000 IDs.");
            if (page < 0) return BadRequest("Page number must not be negative.");
            

            Expression<Func<GudelId, bool>> func = (x) => (
                                                            (gudelId == null || x.Id.Contains(gudelId))
                                                             && (state == null || x.StateId == state)
                                                             && (type == null || x.TypeId == type)
                                                             && (pool == null || x.PoolId == pool)
                                                             && (createdBy == null || x.CreatedBy == createdBy)
                                                             && (reservedBy == null || x.ReservedBy == reservedBy)
                                                             && (producedBy == null || x.ProducedBy == producedBy)
                                                             && (assignedBy == null || x.AssignedBy == assignedBy)
                                                             && (voidedBy == null || x.VoidedBy == voidedBy)
                                                             && (createdOn == null || x.CreationDate == createdOn)
                                                             && (reservedOn == null || x.ReservationDate == reservedOn)
                                                             && (producedOn == null || x.ProductionDate == producedOn)
                                                             && (assignedOn == null || x.AssignmentDate == assignedOn)
                                                             && (voidedOn == null || x.VoidDate == voidedOn)
                                                             );

            var total = await _gudelIdService.FindAllCount(func);
            double tP = (total / pageSize);
            var totalPages = Math.Ceiling(tP);
            return Ok(new DataResponse(await _gudelIdService.FindAll(func, pageSize, page),new { total, totalPages, page, pageSize}));

        }

        [HttpGet("{gudelId}")]
        [Right(StaticRights.GET_GID)]
        //todo
        //@Roles(ROLES.ID_SYSTEM_ADMINISTRATOR, ROLES.ID_ASSIGNOR, ROLES.ID_PRODUCER, ROLES.SMART_PRODUCTS)
        public async Task<IActionResult> GetGudelId(string gudelId, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            if (String.IsNullOrEmpty(gudelId) || gudelId.Length != 12) 
                return BadRequest(new { message = $"Format of requested Gudel ID \"{gudelId}\" is invalid." });
            var result = await _gudelIdService.Find(gudelId, language);
            if (result == null)
                return NotFound(new { Message = $"Gudel with ID {gudelId} could not be found." });
            return Ok(result);
        }

        [HttpPost("create")]
        [Right(StaticRights.CREATE_GID)]
        //todo
        //@Roles(ROLES.ID_SYSTEM_ADMINISTRATOR, ROLES.ID_CREATOR)
        public async Task<IActionResult> CreateGudelIds([FromBody] GudelIdRequest req, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {

            if(req.Amount <= 0)
            {
                req.Amount = 1;
            }
            if(req.TypeId <= 0)
            {
                req.TypeId = 1;
            }
            if (req.Amount > 10000)
            {
                return BadRequest("Amount per request is limited to 10,000 IDs.");
            }

            var newId = await _gudelIdService.CreateGudelIds(req, HttpContext.GetAuthUserIdOrNull(), language);
            return Created("/",newId);
        }

        [HttpPost("create/{gudelId}")]
        [Right(StaticRights.CREATE_GID)]
        //todo
        //@Roles(ROLES.ID_SYSTEM_ADMINISTRATOR, ROLES.ID_CREATOR)
        public async Task<IActionResult> CreateGudelId(string gudelId, [FromBody] GudelIdRequest req, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            var checkId = await _gudelIdService.Find(gudelId, language);
            if (checkId != null) 
                return BadRequest(new { message = "GudelId already exists" });
            if (string.IsNullOrEmpty(gudelId) || gudelId.Length != 12)
                return BadRequest(new { message = $"Format of requested Gudel ID \"{gudelId}\" is invalid." });
            var newId = await _gudelIdService.CreateGudelId(gudelId, req.poolId, req.TypeId, HttpContext.GetAuthUserIdOrNull());

            return Created("/", new[] { newId });
        }

        [HttpPost("assign/batch")]
        [Right(StaticRights.UPDATE_GID)]
        public async Task<IActionResult> AssignToPool([FromBody] List<PoolAssignRequest> req)
        {
            foreach (var item in req)
            {
                await AssignToPool(item.GudelId, item.TargetPoolId);
            }

            return StatusCode(201);
        }

        [HttpPost("assign/{gudelId}/{poolId}")]
        [Right(StaticRights.UPDATE_GID)]
        public async Task<IActionResult> AssignToPool(string gudelId, int poolId, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            if (String.IsNullOrEmpty(gudelId) || gudelId.Length != 12) return BadRequest(new { message = $"Format of requested Gudel ID \"{gudelId}\" is invalid." });
            var checkId = await _gudelIdService.Find(gudelId, language);
            if (checkId == null) return NotFound();

            var pool = await _poolService.FindById(poolId, null);
            if (pool == null)
            {
                return NotFound(new { Message = $"Pool with ID {poolId} could not be found." });
            }

            await _gudelIdService.UpdatePoolId(gudelId, poolId);
            var result = await _gudelIdService.Find(gudelId, language);
            return Created("/",result);
        }
    }
}