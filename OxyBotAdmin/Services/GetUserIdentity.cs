using NLog;
using OxyBotAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OxyBotAdmin.Services
{


    public class GetUserIdentity
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public ClaimsIdentity GetIdentity(BotAdmin botAdmin)
        {
            ClaimsIdentity claimsIdentity = null;
            try
            {
                if (botAdmin != null)
                {
                    var claims = new List<Claim> {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, botAdmin.Login),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, botAdmin.Role)
                    };

                    claimsIdentity = new ClaimsIdentity(claims, "JWToken", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
            }

            return claimsIdentity;
        }
    }
}
