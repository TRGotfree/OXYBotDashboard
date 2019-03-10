using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OxyBotAdmin.Models;
using Microsoft.Extensions.Configuration;
using OxyBotAdmin.Repository;

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

        public DistrictDBController GetDistrictDBController()
        {
            return new DistrictDBController(getConnectionString, logger, configuration);
        }

        public UserRequestsDBController GetUserRequestsDBController()
        {
            return new UserRequestsDBController(getConnectionString, logger, configuration);
        }

        public GoodAnnotationsDbController GetGoodAnnotations()
        {
            return new GoodAnnotationsDbController(getConnectionString, logger, configuration);
        }

        public DiscountDBController GetDiscountCardsDBController()
        {
            return new DiscountDBController(getConnectionString, logger, configuration);
        }
    }
}
