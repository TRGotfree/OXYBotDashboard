using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Services
{
    public interface ILogger
    {
        void LogError(string errText);

        void LogInfo(string infoText);
    }
}
