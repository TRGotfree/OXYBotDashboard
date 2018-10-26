using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Services
{
    public class BaseService
    {
        public IConfiguration Configuration { get; private set; }
        public ILogger Logger { get; private set; }
        public IGetConnectionString GetConnectionString { get; private set; }
        public IDBController DBController { get; private set; }
         

        public BaseService(IConfiguration configuration, ILogger logger, IGetConnectionString getConnectionString, IDBController dBController)
        {
            this.Configuration = configuration;
            this.Logger = logger;
            this.GetConnectionString = getConnectionString;
            this.DBController = dBController;
        }

    }
}
