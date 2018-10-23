using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OxyBotAdmin.Models;
using OxyBotAdmin.DataBaseDomen;
using Microsoft.Extensions.Configuration;

namespace OxyBotAdmin.Services
{
    public class DBController : IDBController
    {
        private IGetConnectionString getConnectionString;
        private ILogger logger;
        private IConfiguration configuration;

        public DBController(IGetConnectionString _getConnectionString, ILogger _logger, IConfiguration configuration)
        {
            getConnectionString = _getConnectionString;
            logger = _logger;
            this.configuration = configuration;
        }

        public LoginDbController GetLoginDbController()
        {
            return new LoginDbController(getConnectionString, logger);
        }

        public TelegramBotUsersDBController GetTGUsersConroller()
        {
            return new TelegramBotUsersDBController(getConnectionString, logger, configuration);
        }

        public AdvertActionsDBController GetAdvertActionsDBController()
        {
            return new AdvertActionsDBController(getConnectionString, logger, configuration);
        }

        public DrugStoreDBController GetDrugStoreDBController()
        {
            return new DrugStoreDBController(getConnectionString, logger, configuration);
        }
    }
}
