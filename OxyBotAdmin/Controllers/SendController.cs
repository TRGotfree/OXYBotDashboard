using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OxyBotAdmin.AppData;
using OxyBotAdmin.Models;
using OxyBotAdmin.Services;
using System.Net;

namespace OxyBotAdmin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SendController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IDBController dBController;
        private readonly ITelegramBot telegramBot;

        public SendController(BaseService baseService, ITelegramBot telegramBot)
        {
            logger = baseService.Logger;
            this.dBController = baseService.RepositoryProvider;
            this.telegramBot = telegramBot;
        }

        [Authorize]
        [HttpPost]
        [Route("msg")]
        [Consumes("application/json")]
        public async Task<IActionResult> Msg([FromBody]string message)
        {
            var res = StatusCode(400);
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                    return BadRequest();

                var tgUsers = dBController.GetTGUsersConroller().GetTelegramBotUsers();

                if (tgUsers != null)
                    await telegramBot.SendMessage2All(tgUsers.Select(chat => chat.ChatId).ToArray(), message);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode(500);
            }
        }


        [Authorize]
        [HttpPost]
        [Route("img")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Img([FromForm] IFormCollection data)
        {
            try
            {
                if (data == null || data.Files == null || data.Files.Count <= 0)
                    return BadRequest();

                var tgUsers = dBController.GetTGUsersConroller().GetTelegramBotUsers();
                if (tgUsers == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError);

                string caption4Msg = data.ContainsKey("message") ? data["message"].ToString() : string.Empty;

                var stream = data.Files[0].OpenReadStream();
                if (stream.Length > 25000000)
                    return BadRequest();

                await telegramBot.SendImage2All(tgUsers.Select(u => u.ChatId).ToArray(), stream, data.Files[0].FileName, caption4Msg);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("file")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> File([FromForm] IFormCollection data)
        {
            try
            {
                if (data == null || data.Files == null || data.Files.Count <= 0)
                    return BadRequest();

                var tgUsers = dBController.GetTGUsersConroller().GetTelegramBotUsers();
                if (tgUsers == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError);

                string caption4Msg = data.ContainsKey("message") ? data["message"].ToString() : string.Empty;

                var stream = data.Files[0].OpenReadStream();
                if (stream.Length > 35000000)
                    return BadRequest();

                await telegramBot.SendFileToAll(tgUsers.Select(u => u.ChatId).ToArray(), stream, data.Files[0].FileName, caption4Msg);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        [Authorize]
        [HttpPost]
        [Route("video")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Video([FromForm] IFormCollection data)
        {
            try
            {
                if (data == null || data.Files == null || data.Files.Count <= 0)
                    return BadRequest();

                var tgUsers = dBController.GetTGUsersConroller().GetTelegramBotUsers();
                if (tgUsers == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError);

                string caption4Msg = data.ContainsKey("message") ? data["message"].ToString() : string.Empty;

                var stream = data.Files[0].OpenReadStream();
                if (stream.Length > 35000000)
                    return BadRequest();

                await telegramBot.SendVideoToAll(tgUsers.Select(u => u.ChatId).ToArray(), stream, data.Files[0].FileName, caption4Msg);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("audio")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Audio([FromForm] IFormCollection data)
        {
            var res = StatusCode(400);
            try
            {
                if (data == null || data.Files == null || data.Files.Count <= 0)
                    return BadRequest();

                var tgUsers = dBController.GetTGUsersConroller().GetTelegramBotUsers();
                if (tgUsers == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError);

                string caption4Msg = data.ContainsKey("message") ? data["message"].ToString() : string.Empty;

                var stream = data.Files[0].OpenReadStream();
                if (stream.Length > 35000000)
                    return BadRequest();

                await telegramBot.SendAudioToAll(tgUsers.Select(u => u.ChatId).ToArray(), stream, data.Files[0].FileName, caption4Msg);
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
