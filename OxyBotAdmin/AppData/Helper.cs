using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.AppData
{
    public class Helper
    {
        public static bool IsUserLoginPassEmpty(string login, string pass)
        {
            return (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(pass));
        }
    }
}
