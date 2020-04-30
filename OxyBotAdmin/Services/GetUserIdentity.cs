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
        public ClaimsIdentity GetIdentity(BotAdmin botAdmin)
        {
            if (botAdmin == null)
                throw new ArgumentNullException(nameof(botAdmin));

            var claims = new List<Claim> {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, botAdmin.Login),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, botAdmin.Role)
                    };

            return new ClaimsIdentity(claims, "JWToken", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
