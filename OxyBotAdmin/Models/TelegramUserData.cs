using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Models
{
    public class TelegramUserData : TelegramUser
    {
        public long RowNum { get; set; }

        public string LastVisitDateTime { get; set; }

        public int MsgCount { get; set; }

        public int TotalUserCount { get; set; }
    }
}
