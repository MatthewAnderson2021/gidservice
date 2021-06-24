using GudelIdService.Domain.Services;
using GudelIdService.Implementation.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GService.Common.Implementation;
using GService.Common.Auth;
namespace GudelIdService.Implementation.Controllers
{
    [ApiController]
    [Route("v1/permission")]
    public class PermissionController : ControllerBase
    {

        public PermissionController(IPermissionKeyService permissionKeyService)
        {
            _permissionKeyService = permissionKeyService;
        }

        private IPermissionKeyService _permissionKeyService { get; set; }

        /// <summary>
        ///     Creates for the given gudelId and for every permission type a permissionKey
        /// </summary>
        /// <param name="gudelId"></param>
        /// <returns></returns>
        [HttpPost("{gudelId}")]
        [Right(StaticRights.UPDATE_GID)]
        public async Task<IActionResult> GenerateKeys(string gudelId)
        {
            var result = await _permissionKeyService.CreatePermissionKeys(gudelId);

            if(result is null)
            {
                return NotFound();
            }

            // the method should generate one key per permissionType for the given gudelId
            // if keys already exist, they are overwritten!
            // the method should return the original keys (before they are hashed) so that the user has a one time chance to note the original keys

            // please implement the needed logic for generating the keys in the service, as is will also be called within other services where GudelIds are created

            return Ok(result);
        }

        /// <summary>
        ///     Gets all key hints for the given gudelId and every permission type
        /// </summary>
        /// <param name="gudelId"> the gudelId to get the hints for </param>
        /// <returns></returns>
        [HttpGet("{gudelId}")]
        [Right(StaticRights.UPDATE_GID)]
        public async Task<IActionResult> GetKeyHints(string gudelId)
        {
            var result = await _permissionKeyService.GetKeyHintsForGudelIdAsync(gudelId);
            if(result.Any(t => t.Item2 == null))
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        ///     Validates if the given key is a valid key of the given gudelId
        /// </summary>
        /// <param name="gudelId"> the gudelId  </param>
        /// <param name="key"> the key/password </param>
        /// <returns></returns>
        [HttpGet("{gudelId}/{key}")]
        public async Task<IActionResult> ValidateKey(string gudelId, string key)
        {
            // this method should check if the given key is actually a valid key of the given gudelId
            // should return the validity and (if valid) also the type
            // e.g. { "isValid": true, "type": "RESELLER" }
            var result = await _permissionKeyService.CheckIfGivenKeyIsAValidKeyOfTheGudelIDAsync(gudelId, key);

            return Ok(result);
        }
    }
}
