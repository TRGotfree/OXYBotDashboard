using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OxyBotAdmin.Models;
using OxyBotAdmin.Services;

namespace OxyBotAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisingController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly BaseService baseService;
        private readonly IStringLocalizer<AppData.SharedResource> sharedLocalizer;

        public AdvertisingController(BaseService baseService, IStringLocalizer<AppData.SharedResource> localizer)
        {
            this.baseService = baseService;
            logger = baseService.Logger;
            sharedLocalizer = localizer;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery] int beginPage, [FromQuery] int endPage)
        {
            try
            {
                if (beginPage <= 0 || endPage <= 0)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                var actionsItems = baseService.RepositoryProvider.GetAdvertActionsDBController().GetActions(beginPage, endPage);

                int totalCountOfAdvert = actionsItems.FirstOrDefault() == null ? 0 : actionsItems.FirstOrDefault().TotalCountOfAdvertActions;

                return Ok(new
                {
                    actions = actionsItems,
                    totalCount = totalCountOfAdvert
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode(500, sharedLocalizer["InternalServerError"]);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody]AdvertAction advertAction)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                if (!IsAdvertDateTimesValid(advertAction.FormattedDateBegin, advertAction.FormattedDateEnd, out (DateTime, DateTime) parserResult))
                    return StatusCode((int)HttpStatusCode.NotAcceptable, sharedLocalizer["NotAcceptableDateTime"]);

                advertAction.DateBegin = parserResult.Item1;
                advertAction.DateEnd = parserResult.Item2;
                baseService.RepositoryProvider.GetAdvertActionsDBController().InsertAction(advertAction);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody]AdvertAction advertAction)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                if (!IsAdvertDateTimesValid(advertAction.FormattedDateBegin, advertAction.FormattedDateEnd, out (DateTime, DateTime) parserResult))
                    return StatusCode((int)HttpStatusCode.NotAcceptable, sharedLocalizer["NotAcceptableDateTime"]);

                baseService.RepositoryProvider.GetAdvertActionsDBController().UpdateAction(advertAction);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }


        private bool IsAdvertDateTimesValid(string beginDateString, string endDateString, out (DateTime, DateTime) parseResult)
        {
            try
            {
                DateTime beginDate = DateTime.TryParse(beginDateString, out beginDate) ? beginDate : DateTime.MinValue;
                DateTime endDate = DateTime.TryParse(endDateString, out endDate) ? endDate : DateTime.MinValue;

                if (beginDate == DateTime.MinValue || endDate == DateTime.MinValue)
                {
                    parseResult = (beginDate, endDate);
                    return false;
                }

                parseResult = (beginDate, endDate);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
