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
        [Route("all")]
        public IActionResult All([FromQuery]int beginPage, [FromQuery]int endPage)
        {
            IActionResult result = StatusCode(400);
            try
            {
                if (beginPage > 0 && endPage > 0)
                {
                    var tgUsers = baseService.DBController.GetTGUsersConroller().GetTelegramBotUsers(beginPage, endPage);
                    if (tgUsers != null)
                    {
                        int totalUsersCount = tgUsers.FirstOrDefault() == null ? 0 : tgUsers.FirstOrDefault().TotalUserCount;
                        var data = new
                        {
                            botUsers = tgUsers,
                            botUsersCount = totalUsersCount
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
                throw ex;
            }

            return result;
        }

        // GET: api/Request/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Request
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Request/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
