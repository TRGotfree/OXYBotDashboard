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
    public class RequestController : ControllerBase
    {
        private ILogger logger;
        private BaseService baseService;
        private IStringLocalizer<AppData.SharedResource> sharedLocalizer;

        public RequestController(BaseService _baseService, IStringLocalizer<AppData.SharedResource> _localizer)
        {
            logger = _baseService.Logger;
            baseService = _baseService;
            sharedLocalizer = _localizer;
        }

        // GET: api/Request
        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery]int beginPage, [FromQuery]int endPage)
        {
            IActionResult result = StatusCode(400);
            try
            {
                if (beginPage > 0 && endPage > 0)
                {
                    var userRequests = baseService.DBController.GetUserRequestsDBController().GetRequests(beginPage, endPage);
                    if (userRequests != null)
                    {
                        int totalUserRequest = userRequests.FirstOrDefault() == null ? 0 : userRequests.FirstOrDefault().TotalCount;
                        int todayRequests = userRequests.FirstOrDefault() == null ? 0 : userRequests.FirstOrDefault().TodayRequestCount;
                        var data = new
                        {
                            requests = userRequests,
                            requestTotalCount = totalUserRequest,
                            todayRequestsCount = todayRequests
                        };
                        result = Ok(data);
                    }
                    else
                    {
                        result = StatusCode(500);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                result = StatusCode(500);
            }

            return result;
        }

    }
}
