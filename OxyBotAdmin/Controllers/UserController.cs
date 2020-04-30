using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly ILogger logger;
        private readonly IDBController dBController;
        private readonly ITelegramBot bot;

        public UserController(BaseService baseService, ITelegramBot telegramBot)
        {
            logger = baseService.Logger;
            dBController = baseService.RepositoryProvider;
            bot = telegramBot;
        }

        [Authorize]
        [HttpGet]
        [Route("all")]
        public IActionResult Get([FromQuery]int beginPage, [FromQuery]int endPage)
        {
            try
            {
                if (beginPage <= 0 && endPage <= 0)
                    return BadRequest();

                var tgUsers = dBController.GetTGUsersConroller().GetTelegramBotUsers(beginPage, endPage);
                if (tgUsers == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError);

                int totalUsersCount = tgUsers.FirstOrDefault() == null ? 0 : tgUsers.FirstOrDefault().TotalUserCount;

                return Ok(new { botUsers = tgUsers, botUsersCount = totalUsersCount });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var tgUsers = dBController.GetTGUsersConroller().GetTelegramBotUsers(1, 15);
                if (tgUsers == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError);

                int totalUsersCount = tgUsers.FirstOrDefault() == null ? 0 : tgUsers.FirstOrDefault().TotalUserCount;

                return Ok(new { botUsers = tgUsers, botUsersCount = totalUsersCount });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize]
        [HttpPut("{chatId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> Put([FromBody]string message, long chatId)
        {
            try
            {
                if (chatId <= 0 || !string.IsNullOrWhiteSpace(message))
                    return BadRequest();

                await bot.SendMessage(chatId, message);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
