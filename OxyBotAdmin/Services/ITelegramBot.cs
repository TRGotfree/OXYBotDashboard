using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Services
{
    public interface ITelegramBot
    {
        Task SendMessage2All(long[] usersChatId, string message);

        void SendMessage(long userChatId, string message);

        Task SendImage2All(long[] usersChatId, Stream stream, string msg);
    }
}
