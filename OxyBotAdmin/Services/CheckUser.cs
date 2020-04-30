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
        private ILogger logger;
        private IDBController dBController;

        public CheckUser(BaseService baseService)
        {
           logger = baseService.Logger;
            dBController = baseService.RepositoryProvider;
        }

        public bool CheckUserLoginPass(BotAdmin botAdmin)
        {
            bool checkResult = false;
            try
            {
                if (!string.IsNullOrEmpty(botAdmin.Password) &&
                    !string.IsNullOrEmpty(botAdmin.Login) &&
                    !string.IsNullOrWhiteSpace(botAdmin.Password) &&
                    !string.IsNullOrWhiteSpace(botAdmin.Login))
                {
                    checkResult = dBController.GetLoginDbController().CheckAdmin(botAdmin);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
            }

            return checkResult;
        }

        public BotAdmin GetBotAdmin(string login, string hashedPass)
        {
            BotAdmin botAdmin = new BotAdmin();
            try
            {
                botAdmin = dBController.GetLoginDbController().GetBotAdmin(login, hashedPass);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
            }
            return botAdmin;
        }
    }
}
