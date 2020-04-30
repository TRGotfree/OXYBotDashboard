using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OxyBotAdmin.Models;
using OxyBotAdmin.Services;
using OxyBotAdmin.AppData;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using Microsoft.Extensions.Localization;

namespace OxyBotAdmin.Controllers
{

    public class LoginController : Controller
    {
        private readonly ICheckUser checkUser;
        private readonly IWorkWithHash workWithHash;
        private readonly ILogger logger;
        private readonly IStringLocalizer<SharedResource> sharedLocalizer;

        public LoginController(BaseService baseService, ICheckUser checkUser, IWorkWithHash workWithHash, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.checkUser = checkUser;
            this.workWithHash = workWithHash;
            logger = baseService.Logger;
            this.sharedLocalizer = sharedLocalizer;
        }

        [AllowAnonymous]
        [HttpGet]
        public async void Auth()
        {
            try
            {
                Response.ContentType = "text/html";
                await Response.SendFileAsync(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"));
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                Response.StatusCode = 500;
                await Response.WriteAsync(sharedLocalizer["InternalServerError"]);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Auth([FromBody]BotAdmin botAdmin)
        {
            try
            {
                if (botAdmin == null || Helper.IsUserLoginPassEmpty(botAdmin.Login, botAdmin.Password))
                    return Forbid();

                botAdmin.Password = workWithHash.CalculateHash(botAdmin.Password);

                var checkResult = checkUser.CheckUserLoginPass(botAdmin);
                if (!checkResult)
                    return Forbid();

                botAdmin = checkUser.GetBotAdmin(botAdmin.Login, botAdmin.Password);

                var userIdentityClaim = new GetUserIdentity().GetIdentity(botAdmin);
                var jwtoken = new GetNewJWToken().GetToken(userIdentityClaim);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtoken)
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }
    }
}