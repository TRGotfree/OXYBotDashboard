using OxyBotAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Services
{
    public interface ICheckUser
    {
        bool CheckUserLoginPass(Models.BotAdmin botAdmin);

        BotAdmin GetBotAdmin(string login, string hashedPass);
    }
}
