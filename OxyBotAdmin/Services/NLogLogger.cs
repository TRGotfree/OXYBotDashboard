using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Services
{
    public class NLogLogger : ILogger
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void LogError(Exception ex)
        {
            logger.Error($"Текст ошибки: {ex.Message} StackTrace: {ex.StackTrace}");
        }

        public void LogInfo(string infoText)
        {
            logger.Info(infoText);
        }
    }
}
