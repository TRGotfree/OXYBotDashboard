using System;
using System.Collections.Generic;
using System.Linq;
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

        private ILogger logger;
        private BaseService baseService;
        private IStringLocalizer<AppData.SharedResource> sharedLocalizer;

        public DistrictController(BaseService _baseService, IStringLocalizer<AppData.SharedResource> _localizer)
        {
            logger = _baseService.Logger;
            baseService = _baseService;
            sharedLocalizer = _localizer;
        }

        // GET: api/District
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            IActionResult result = StatusCode(400, sharedLocalizer["BadRequest"]);
            try
            {
                var _districts = baseService.RepositoryProvider.GetDistrictDBController().GetDistricts();

                var data = new
                {
                    districts = _districts.Select(d => d.Name).ToArray()
                };

                result = Ok(data);
            }
            catch (Exception ex)
            {
                result = StatusCode(500, sharedLocalizer["InternalServerError"]);
                logger.LogError(ex);
            }
            return result;
        }

    }
}
