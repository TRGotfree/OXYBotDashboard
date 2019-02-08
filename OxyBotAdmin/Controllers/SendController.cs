﻿using System;
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

namespace OxyBotAdmin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SendController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
         
        private IDBController dBController;
        private ITelegramBot bot;

        public SendController(BaseService baseService, ITelegramBot telegramBot)
        {
            logger = baseService.Logger;
            configuration = baseService.Configuration;
            this.dBController = baseService.DBController;
            bot = telegramBot;
        }

        [Authorize]
        [HttpPost]
        [Route("msg")]
        [Consumes("application/json")]
        public async Task<IActionResult> Msg([FromBody]string message)
        {
            var res = StatusCode(500);
            try
            {
                if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                {
                    var tgUsers = dBController.GetTGUsersConroller().GetTelegramBotUsers();

                    //var tgUsers = new long[] { 59725585 };

                    if (tgUsers != null)
                        await bot.SendMessage2All(tgUsers.Select(chat=>chat.ChatId).ToArray(), message);

                    res = Ok();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                res = StatusCode(500);
                throw ex;
            }
            return res;
        }


        [Authorize]
        [HttpPost]
        [Route("img")]
        [Consumes("multipart/form-data")]        
        public async Task<IActionResult> Img([FromForm] IFormCollection data)
        {
            var res = StatusCode(404);
            try
            {
                if (data != null && data.Files != null && data.Files.Count > 0)
                {
                    var tgUsers = dBController.GetTGUsersConroller().GetTelegramBotUsers();
                    //var tgUsers = new long[] { 59725585 };

                    string caption4Msg = data.ContainsKey("message") ? data["message"].ToString() : string.Empty;

                    if (tgUsers != null && data.Files[0] != null)
                    {
                        var stream = data.Files[0].OpenReadStream();
                        if (stream.Length <= 25000000)
                        {
                            await bot.SendImage2All(tgUsers.Select(chat=>chat.ChatId).ToArray(), stream, caption4Msg);
                            res = Ok();
                        }
                    }
                    else
                    {
                        res = StatusCode(400);
                    }
                }
                else
                {
                    res = StatusCode(400);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                res = StatusCode(500);
                throw ex;
            }
            return res;
        }
        
       

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
