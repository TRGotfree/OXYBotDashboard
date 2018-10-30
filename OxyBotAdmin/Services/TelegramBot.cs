using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace OxyBotAdmin.Services
{
    public class TelegramBot : ITelegramBot
    {

        private ILogger logger;
        private TelegramBotClient telegramBot;
         

        public TelegramBot(ILogger _logger, IConfiguration configuration)
        {
            logger = _logger;
            telegramBot = new TelegramBotClient(configuration["BotToken"]);
        }

        public async Task<bool> SendMessage(long userChatId, string message)
        {
            var result = false;
            if (userChatId > 0 && !string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
            {
                try
                {
                    await telegramBot.SendTextMessageAsync(userChatId, message, Telegram.Bot.Types.Enums.ParseMode.Html);
                    result = true;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex);
                    result = false;
                }
            }
            return result;
        }

        public async Task SendMessage2All(long[] usersChatId, string message)
        {
            if (usersChatId != null && !string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
            {
                for (int i = 0; i < usersChatId.Length; i++)
                {
                    try
                    {
                        await telegramBot.SendTextMessageAsync(usersChatId[i], message, Telegram.Bot.Types.Enums.ParseMode.Html);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex);
                    }
                }
            }
        }

        public async Task SendImage2All(long[] usersChatId, Stream stream, string msg)
        {
            if (usersChatId != null && stream != null)
            {
                for (int i = 0; i < usersChatId.Length; i++)
                {
                    try
                    {
                        await telegramBot.SendPhotoAsync(usersChatId[i], stream, msg, Telegram.Bot.Types.Enums.ParseMode.Html);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex);
                    }
                }
            }
        }
    }
}
