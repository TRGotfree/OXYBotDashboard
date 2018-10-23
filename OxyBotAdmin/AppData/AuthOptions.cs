using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxyBotAdmin.AppData
{
    public class AuthOptions
    {
        public const string ISSUER = "OxyBotAdminApp";
        public const string AUDIENCE = "OxyBotAdmin";
        public const string KEY = "_*cf247118689211131*_";
        public const int LIFETIME = 60; //время действия токена в минутах

        public static SecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}
