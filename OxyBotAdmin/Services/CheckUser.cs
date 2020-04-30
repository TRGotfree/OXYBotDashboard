using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NLog;
using OxyBotAdmin.Models;

namespace OxyBotAdmin.Services
{
    public class CheckUser : ICheckUser
    {
        private readonly IDBController dBController;

        public CheckUser(BaseService baseService)
        {
            dBController = baseService.RepositoryProvider;
        }

        public bool CheckUserLoginPass(BotAdmin botAdmin)
        {
            if (string.IsNullOrWhiteSpace(botAdmin.Password) ||
                string.IsNullOrWhiteSpace(botAdmin.Login))
                return false;

            return dBController.GetLoginDbController().CheckAdmin(botAdmin);
        }

        public BotAdmin GetBotAdmin(string login, string hashedPass)
        {
            return dBController.GetLoginDbController().GetBotAdmin(login, hashedPass);
        }
    }
}
