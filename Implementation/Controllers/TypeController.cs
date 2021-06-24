using GudelIdService.Domain.Dto;
using GudelIdService.Implementation.Persistence.Context;
using GudelIdService.Implementation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using GService.Common.Implementation;
using GService.Common.Auth;
namespace GudelIdService.Implementation.Controllers
{
    [ApiController]
    [Route("v1/type")]
    public class TypeController : ControllerBase
    {

        private readonly ILogger<TypeController> _logger;
        private readonly AppDbContext _dbContext;
        public TypeController(
            ILogger<TypeController> logger,
            AppDbContext dbContext
            )
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        [HttpGet]
        [Right(StaticRights.GET_GID)]
        public IActionResult GetTypes(string language = ConfigService.LANG_DEFAULT)
        {
            var types = _dbContext.GudelIdTypes.Select(x => new GudelIdTypeDto(x.Id, x.Name[language], x.Description[language])).ToList();
            return Ok(types);
        }
    }
}
