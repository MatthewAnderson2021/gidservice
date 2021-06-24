using GudelIdService.Domain.Services;
using GudelIdService.Implementation.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using GService.Common.Implementation;
using GService.Common.Auth;

namespace GudelIdService.Implementation.Controllers
{
    [ApiController]
    [Route("v1/activity")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }


        [HttpGet("user/{userId}")]
        [Right(StaticRights.GET_GID_ACTIVITY)]
        public async Task<IActionResult> GetByUser(string userId, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            var activities = await _activityService.FindAll((x) => x.CreatedBy == userId);
            var result = new List<object>();

            activities.ForEach(activity =>
            {
                result.Add(_activityService.MapExtraFieldToView(activity, language));
            });
            if(result.Count>0)
                return Ok(result);
            else
                return NotFound(new { Message = $"User with ID {userId} could not be found." });
        }


        [HttpGet("id/{gudelId}")]
        [Right(StaticRights.GET_GID_ACTIVITY)]
        public async Task<IActionResult> GetById(string gudelId, [FromHeader(Name = "accept-language")] string language = ConfigService.LANG_DEFAULT)
        {
            var activities = await _activityService.FindAll((x) => x.GudelId == gudelId);
            var result = new List<object>();

            activities.ForEach(activity =>
            {
                result.Add(_activityService.MapExtraFieldToView(activity, language));
            });

            if (result.Count > 0)
                return Ok(result);
            else
                return NotFound(new { Message = $"Activity with ID {gudelId} could not be found." });
        }
    }
}
