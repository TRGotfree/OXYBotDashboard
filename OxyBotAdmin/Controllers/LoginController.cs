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

namespace OxyBotAdmin.Controllers
{

    public class LoginController : Controller
    {
        private readonly ICheckUser checkUser;
        private readonly IWorkWithHash workWithHash;
        private readonly ILogger logger;
 

        public LoginController(BaseService baseService, ICheckUser _checkUser, IWorkWithHash _workWithHash)
        {
            checkUser = _checkUser;
            workWithHash = _workWithHash;
            logger = baseService.Logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Auth()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Auth([FromBody]BotAdmin botAdmin)
        {
            IActionResult res = Forbid();
            try
            {
                if (botAdmin != null && !Helper.IsUserLoginPassEmpty(botAdmin.Login, botAdmin.Password))
                {
                    botAdmin.Password = workWithHash.CalculateHash(botAdmin.Password);

                    var checkResult = checkUser.CheckUserLoginPass(botAdmin);
                    if (checkResult)
                    {
                        botAdmin = checkUser.GetBotAdmin(botAdmin.Login, botAdmin.Password);

                        var userIdentityClaim = new GetUserIdentity().GetIdentity(botAdmin);
                        var jwtoken = new GetNewJWToken().GetToken(userIdentityClaim);

                        var responseData = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(jwtoken)
                            //,
                            //username = userIdentityClaim.Name
                        };
                        res = Ok(responseData);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                res = StatusCode(500);
            }

            return res;
        }



    }
}