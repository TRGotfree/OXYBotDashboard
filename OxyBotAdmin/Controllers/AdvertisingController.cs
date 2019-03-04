using System;
using System.Collections.Generic;
using System.Linq;
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
        private ILogger logger;
        private BaseService baseService;
        private IStringLocalizer<AppData.SharedResource> sharedLocalizer;

        public AdvertisingController(BaseService _baseService, IStringLocalizer<AppData.SharedResource> _localizer)
        {
            logger = _baseService.Logger;
            baseService = _baseService;
            sharedLocalizer = _localizer;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery] int beginPage, [FromQuery] int endPage)
        {
            IActionResult result = StatusCode(400, sharedLocalizer["BadRequest"]);
            try
            {
                if (beginPage > 0 && endPage > 0)
                {
                    if (baseService != null)
                    {
                        var actionsItems = baseService.DBController.GetAdvertActionsDBController().GetActions(beginPage, endPage);

                        int totalCountOfAdvert = actionsItems.FirstOrDefault() == null ? 0 : actionsItems.FirstOrDefault().TotalCountOfAdvertActions;

                        var data = new
                        {
                            actions = actionsItems,
                            totalCount = totalCountOfAdvert
                        };

                        result = Ok(data);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                result = StatusCode(500, sharedLocalizer["InternalServerError"]);
                throw ex;
            }
            return result;
        }


        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody]AdvertAction advertAction)
        {
            IActionResult result = StatusCode(400, sharedLocalizer["BadRequest"]);
            try
            {
                if (advertAction != null && advertAction.ActionId == 0)
                {
                    if (ModelState.IsValid)
                    {
                        (DateTime, DateTime) parserResult;
                        if (IsAdvertDateTimesValid(advertAction.FormattedDateBegin, advertAction.FormattedDateEnd, out parserResult))
                        {
                            advertAction.DateBegin = parserResult.Item1;
                            advertAction.DateEnd = parserResult.Item2;
                            baseService.DBController.GetAdvertActionsDBController().InsertAction(advertAction);
                            result = Ok();
                        }
                        else
                        {
                            result = StatusCode(406, sharedLocalizer["NotAcceptableDateTime"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                result = StatusCode(500, sharedLocalizer["InternalServerError"]);
            }

            return result;
        }

    
        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody]AdvertAction advertAction)
        {
            IActionResult result = StatusCode(400, sharedLocalizer["BadRequest"]);
            try
            {
                if (advertAction != null && advertAction.ActionId > 0)
                {
                    if (ModelState.IsValid)
                    {
                        (DateTime, DateTime) parserResult;
                        if (IsAdvertDateTimesValid(advertAction.FormattedDateBegin, advertAction.FormattedDateEnd, out parserResult))
                        {                      
                            baseService.DBController.GetAdvertActionsDBController().UpdateAction(advertAction);
                            result = Ok();
                        }
                        else
                        {
                            result = StatusCode(406, sharedLocalizer["NotAcceptableDateTime"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                result = StatusCode(500, sharedLocalizer["InternalServerError"]);
                throw ex;
            }

            return result;
        }


        private bool IsAdvertDateTimesValid(string beginDateString, string endDateString, out (DateTime, DateTime) parseResult)
        {
            bool result = false;
            (DateTime, DateTime) dateTimeTuple = (DateTime.MinValue, DateTime.MinValue);
            try
            {
                DateTime beginDate = DateTime.TryParse(beginDateString, out beginDate) ? beginDate : DateTime.MinValue;
                DateTime endDate = DateTime.TryParse(endDateString, out endDate) ? endDate : DateTime.MinValue;

                dateTimeTuple = (beginDate, endDate);

                if (beginDate != DateTime.MinValue && endDate != DateTime.MinValue)
                    result = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw ex;
            }
            parseResult = dateTimeTuple;
            return result;
        }
    }
}
