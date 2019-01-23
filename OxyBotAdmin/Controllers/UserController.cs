using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OxyBotAdmin.Services;

namespace OxyBotAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ILogger logger;
        private IDBController dBController;
        private ITelegramBot bot;

        public UserController(BaseService baseService, ITelegramBot telegramBot)
        {
            logger = baseService.Logger;
            dBController = baseService.DBController;
            bot = telegramBot;
        }

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
                    var tgUsers = dBController.GetTGUsersConroller().GetTelegramBotUsers(beginPage, endPage);
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

        //// GET: api/User/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/User
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //[Authorize]
        [HttpPut("{chatId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> Put([FromBody]string message, long chatId)
        {
            IActionResult result = StatusCode(400);
            var t = Request;
            try
            {
                if (chatId > 0 && !string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                {
                    await bot.SendMessage(chatId, message);
                    result = Ok();
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

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
