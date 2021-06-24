using GService.Common.Implementation;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Repositories;
using GudelIdService.Domain.Services;
using GudelIdService.Implementation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using GService.Common.Auth;
using System;
namespace GudelIdService.Implementation.Controllers
{
    [ApiController]
    [Route("v1/state")]
    public class StateController : ControllerBase
    {
        private readonly ILogger<StateController> _logger;
        private readonly IGudelIdStateService _stateService;
        private readonly IGudelIdService _gudelIdService;
        private readonly IExtraFieldService _extraFieldService;

        public StateController(ILogger<StateController> logger, IGudelIdStateService stateService, IGudelIdService gudelIdService, IExtraFieldService extraFieldService)
        {
            _stateService = stateService;
            _gudelIdService = gudelIdService;
            _extraFieldService = extraFieldService;
            _logger = logger;
        }

        [HttpGet]
        [Right(StaticRights.GET_GID)]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(List<GudelIdStateData>))]
        public async Task<IActionResult> GetStates([FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            return Ok(await _stateService.FindAll(language));
        }

        [HttpGet("{stateId}")]
        [Right(StaticRights.GET_GID)]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(GudelIdStateData))]
        public async Task<IActionResult> GetStateDetails(int stateId, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            var state = await _stateService.FindById(stateId, language);

            if (state == null)
            {
                return NotFound(new { message = $"{stateId} is not a valid state." });
            }

            return Ok(state);
        }

        [HttpPut("batch/{newStateId}")]
        [Right(StaticRights.UPDATE_GID)]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(List<GudelIdBatchResult>))]
        public async Task<IActionResult> ChangeStateBatch(int newStateId, [FromBody] GudelIdBatchRequests request)
        {
            if (request.GudelIds?.Any() != true)
            {
                return new BadRequestObjectResult("GudelIds parameter is missing or empty");
            }

            var batch = new List<GudelIdBatchResult>();
            foreach (var id in request.GudelIds)
            {
                try
                {
                    var result = await ChangeState(id, newStateId, request.ExtraFieldData);
                    batch.Add(new GudelIdBatchResult()
                    {
                        RequestId = id,
                        Result = result,
                        Success = true,
                        Error = null,
                    });
                }
                catch (Exception error)
                {
                    batch.Add(new GudelIdBatchResult()
                    {
                        RequestId = id,
                        Result = null,
                        Success = false,
                        Error = error.Message,
                    });
                }


            }

            return Ok(batch);
        }

        [HttpPut("{gudelIdParam}/{newStateId}")]
        [Right(StaticRights.UPDATE_GID)]
        [Produces(MediaTypeNames.Application.Json, Type = typeof(List<GudelIdBatchResult>))]
        public async Task<IActionResult> ChangeState(string gudelIdParam, int newStateId, [FromBody] Dictionary<string, string> extraFieldData, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            var gudelId = await _gudelIdService.Find(gudelIdParam, ConfigService.LANG_DEFAULT);
            if (gudelId == null) return NotFound();

            var newState = await _stateService.FindById(newStateId, ConfigService.LANG_DEFAULT);
            var currentState = await _stateService.FindById(gudelId.StateId, ConfigService.LANG_DEFAULT);
            if (currentState.AllowedFollowupStateIds?.Any(_ => _ == newStateId) == false)
            {
                return BadRequest(new { message = $"Cannot set state to {newStateId} since current state is {gudelId.StateId}." });
            }

            this.ValidateLifeCycleChangeRights(gudelId, newState, null);

            var requiredFields = newState.ExtraFieldDefinitions.Where(_ => _.IsRequired);
            var missing = requiredFields
                .ToList();

            missing = missing.Where(field => !extraFieldData.ContainsKey(field.Key))
                .ToList();

            if (gudelId.ExtraFields?.Any() == true)
            {
                var existingFields = gudelId.ExtraFields
                    .Select(field => field.ExtraFieldDefinition.Key)
                    .ToList();

                missing = missing.Where(field => !existingFields.Contains(field.Key))
                    .ToList();
            }
            if (missing.Count > 0)
            {
                return new BadRequestObjectResult($"Provide required extraFieldData {string.Join(", ", missing.Select(_ => _.Key).ToList())}.");
            }
            foreach (var entry in extraFieldData)
            {
                var requiredField = newState.ExtraFieldDefinitions.FirstOrDefault(field => field.Key == entry.Key);
                if (requiredField != null)
                {
                    var value = new Dictionary<string, string>();
                    value.Add(language, entry.Value);
                    await _extraFieldService.CreateOrUpdateField(gudelId, requiredField, value, ConfigService.LANG_DEFAULT);
                }
            }

            return Ok(await _gudelIdService.ChangeState(gudelId, newStateId, HttpContext.GetAuthUserIdOrNull()));
        }

        private void ValidateLifeCycleChangeRights(GudelIdData gudelId, GudelIdStateData newState, object User)
        {
            //todo Sebastian
            //    let allowedRoles: ROLES[] = [];
            //    switch (newState.id)
            //    {
            //        default:
            //            break;
            //        case GudelIdStates.reserved:
            //            {
            //                allowedRoles = [
            //                  ROLES.ID_SYSTEM_ADMINISTRATOR,
            //                  ROLES.ID_ASSIGNOR,
            //                  ROLES.ID_PRODUCER,
            //                  ROLES.LOCAL_ID_POOL_SYNCHRONIZER,

            //                ];
            //                break;
            //            }
            //        case GudelIdStates.produced:
            //            {
            //                allowedRoles = [
            //                  ROLES.ID_SYSTEM_ADMINISTRATOR,
            //                  ROLES.ID_PRODUCER,
            //                  ROLES.LOCAL_ID_POOL_SYNCHRONIZER,

            //                ];
            //                break;
            //            }
            //        case GudelIdStates.assigned:
            //            {
            //                allowedRoles = [
            //                  ROLES.ID_SYSTEM_ADMINISTRATOR,
            //                  ROLES.ID_ASSIGNOR,
            //                  ROLES.LOCAL_ID_POOL_SYNCHRONIZER,

            //                ];
            //                break;
            //            }
            //        case GudelIdStates.void: {
            //                allowedRoles = [ROLES.ID_SYSTEM_ADMINISTRATOR, ROLES.SMART_PRODUCTS];
            //                break;
            //            }
            //    }

            //    // if none of the above cases, assume that authorized user can change state
            //    if (allowedRoles.length === 0)
            //    {
            //        return;
            //    }

            //    // for the above cases, the user has to have one of the defined roles
            //    if (
            //      user.groups &&
            //      user.groups.length > 0 &&
            //      allowedRoles.some(alwRole => user.groups.includes(alwRole))
            //    )
            //    {
            //        return;
            //    }

            //    // otherwise, he is not authorized
            //    throw new HttpException('User has no rights to change to this state', 403);
        }
    }
}