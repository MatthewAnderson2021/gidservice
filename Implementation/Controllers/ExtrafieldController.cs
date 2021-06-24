using AutoMapper;
using GudelIdService.Domain.Dto;
using GudelIdService.Domain.Repositories;
using GudelIdService.Domain.Services;
using GudelIdService.Implementation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using GService.Common.Implementation;
using GService.Common.Auth;
namespace GudelIdService.Implementation.Controllers
{
    [ApiController]
    [Route("v1/extra-fields")]
    public class ExtrafieldController : ControllerBase
    {
        private readonly IExtraFieldService _extraFieldService;
        private readonly IGudelIdStateService _gudelIdStateService;
        public ExtrafieldController(IExtraFieldService extraFieldService, IGudelIdStateService gudelIdStateService)
        {
            _extraFieldService = extraFieldService;
            _gudelIdStateService = gudelIdStateService;
        }

        [HttpGet("state/{stateId}")]
        [Right(StaticRights.GET_GID_EXTRA_DEFINITION)]
        public async Task<IActionResult> GetByState(int stateId,
            [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            var result = await _extraFieldService.FindDefinitionsByState(stateId, language);
            return Ok(result);
        }

        [HttpGet]
        [Right(StaticRights.GET_GID_EXTRA_DEFINITION)]
        public async Task<IActionResult> FindAll([FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            var result = await _extraFieldService.FindAllDefinitions(language);
            return Ok(result);
        }

        [HttpGet("{key}")]
        [Right(StaticRights.GET_GID_EXTRA_DEFINITION)]
        public async Task<IActionResult> Find(string key, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            var result = await _extraFieldService.FindDefinitionByKey(key, language);
            if(result == null)
            {
                return NotFound(new { message = $"ExtraFieldDefinion {key} could not be found." });
            }
            return Ok(result);
        }

        [HttpPut("{key}")]
        [Right(StaticRights.UPDATE_GID_EXTRA_DEFINITION)]
        public async Task<IActionResult> ModifyExtraFieldDefinition(string key, [FromBody] ExtraFieldDefinitionData fieldDefinitionData, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            foreach (var stateId in fieldDefinitionData.State)
            {
                var resultFind = await _gudelIdStateService.FindById(stateId, language);
                if (resultFind == null)
                    return BadRequest(new { Message = $"GudelIdState {stateId} could not be found." });
            }
            var result = await _extraFieldService.UpdateDefinition(fieldDefinitionData, language);
            return Ok(result);
        }

        [HttpPost()]
        [Right(StaticRights.CREATE_GID_EXTRA_DEFINITION)]
        public async Task<IActionResult> CreateExtraFieldDefinition([FromBody] ExtraFieldDefinitionData fieldDefinitionData,
            [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            foreach (var stateId in fieldDefinitionData.State)
            {
                var resultFind = await _gudelIdStateService.FindById(stateId, language);
                if (resultFind == null)
                    return BadRequest(new { Message = $"GudelIdState {stateId} could not be found." });
            }

            var result = await _extraFieldService.AddDefinition(fieldDefinitionData, language);
            return Created("/", result);
        }

        [HttpDelete("{key}")]
        [Right(StaticRights.DELETE_GID_EXTRA_DEFINITION)]
        public async Task<IActionResult> DeleteExtraFieldDefinition(string key)
        {
            var definitions = await _extraFieldService.FindDefinitionByKey(key, ConfigService.LANG_DEFAULT);

            if (definitions is null)
            {
                return NotFound(new { Message = $"Extrafielddefinition with {key} could not be found." });
            }

            await _extraFieldService.RemoveDefinition(key);

            return Ok();
        }


        [HttpPost("data/{gudelId}/{fieldId}")]
        [Right(StaticRights.UPDATE_GID_EXTRA_DATA)]
        public async Task<IActionResult> UpdateExtraField(string gudelId, int fieldId, [FromBody] KeyValuePair<string,string> value, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            var entry = new Dictionary<string, string>();
            entry.Add(language, value.Value);
            
            var result = await _extraFieldService.UpdateExtraField(gudelId, fieldId, entry);
            if (result == null)
            {
                return BadRequest(new { Message = $"ExtraField with gudelId {gudelId} and fieldId {fieldId} could not be found." });
            }
            return Ok(result);
        }
    }
}