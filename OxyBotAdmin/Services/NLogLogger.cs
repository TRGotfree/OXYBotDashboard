using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Services
{
    public class NLogLogger : ILogger
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public void LogError(string errText)
        {
            logger.Error(errText);
        }

        public void LogInfo(string infoText)
        {
            logger.Info(infoText);
        }
    }
}
