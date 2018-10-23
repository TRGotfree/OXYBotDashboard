using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OxyBotAdmin.AppData;
using Microsoft.IdentityModel.Tokens;

namespace OxyBotAdmin.Services
{
    public class GetNewJWToken
    {

        public JwtSecurityToken GetToken(ClaimsIdentity claims)
        {
            DateTime nowDateTime = DateTime.UtcNow;
            DateTime expirationDateTime = nowDateTime.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME));

            var newToken = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: nowDateTime,
                claims: claims.Claims,
                expires: expirationDateTime,
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return newToken;
        }

    }
}
