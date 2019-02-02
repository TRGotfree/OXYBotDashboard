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
        public IActionResult Get([FromQuery]int beginPage, [FromQuery]int endPage)
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

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            IActionResult result = StatusCode(400);
            try
            {
                var tgUsers = dBController.GetTGUsersConroller().GetTelegramBotUsers(1, 15);
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
            catch (Exception ex)
            {
                logger.LogError(ex);
                result = StatusCode(500);
                throw ex;
            }

            return result;
        }

        [Authorize]
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


    }
}
