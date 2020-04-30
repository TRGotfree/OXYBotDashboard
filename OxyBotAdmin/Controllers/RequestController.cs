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
    public class RequestController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly BaseService baseService;
        private readonly IStringLocalizer<AppData.SharedResource> sharedLocalizer;

        public RequestController(BaseService baseService, IStringLocalizer<AppData.SharedResource> localizer)
        {
            logger = baseService.Logger;
            this.baseService = baseService;
            sharedLocalizer = localizer;
        }

        // GET: api/Request
        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery]int beginPage, [FromQuery]int endPage)
        {
            try
            {
                if (beginPage <= 0 || endPage <= 0)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                var userRequests = baseService.RepositoryProvider.GetUserRequestsDBController().GetRequests(beginPage, endPage);
                if (userRequests == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);

                int totalUserRequest = userRequests.FirstOrDefault() == null ? 0 : userRequests.FirstOrDefault().TotalCount;
                int todayRequests = userRequests.FirstOrDefault() == null ? 0 : userRequests.FirstOrDefault().TodayRequestCount;
                var data = new
                {
                    requests = userRequests,
                    requestTotalCount = totalUserRequest,
                    todayRequestsCount = todayRequests
                };
                return Ok(data);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }

    }
}
