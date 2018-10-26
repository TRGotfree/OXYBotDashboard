using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Services
{
    public interface ILogger
    {
        void LogError(Exception ex);

        void LogInfo(string infoText);
    }
}
