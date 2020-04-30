using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OxyBotAdmin.Services;

namespace OxyBotAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly BaseService baseService;
        private readonly IStringLocalizer<AppData.SharedResource> sharedLocalizer;

        public DistrictController(BaseService baseService, IStringLocalizer<AppData.SharedResource> localizer)
        {
            logger = baseService.Logger;
            this.baseService = baseService;
            sharedLocalizer = localizer;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var _districts = baseService.RepositoryProvider.GetDistrictDBController().GetDistricts();

                return Ok(new
                {
                    districts = _districts.Select(d => d.Name).ToArray()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }

    }
}
