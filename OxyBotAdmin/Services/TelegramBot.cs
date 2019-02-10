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

        public async void SendMessage(long userChatId, string message)
        {
            if (userChatId > 0 && !string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
            {
                try
                {
                    await telegramBot.SendTextMessageAsync(userChatId, message, Telegram.Bot.Types.Enums.ParseMode.Html);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex);
                    throw ex;
                }
            }
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

        public async Task SendImage2All(long[] usersChatId, Stream stream, string fileName, string msg)
        {
            string sendedImageFileId = string.Empty;
            if (usersChatId != null && stream != null)
            {
                for (int i = 0; i < usersChatId.Length; i++)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(sendedImageFileId))
                        {
                            var inputOnlineFile = new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream, fileName);

                            var sendedImage = await telegramBot.SendPhotoAsync(59725585, inputOnlineFile, msg, Telegram.Bot.Types.Enums.ParseMode.Html);

                            if (sendedImage != null && sendedImage.Photo != null && sendedImage.Photo.Length > 0)
                            {
                                var maxSized = sendedImage.Photo.OrderBy(p => p.FileSize).FirstOrDefault();
                                sendedImageFileId = maxSized.FileId;
                            }
                        }
                        else
                        {
                            var inputOnlinePhoto = new Telegram.Bot.Types.InputFiles.InputOnlineFile(sendedImageFileId);
                            await telegramBot.SendPhotoAsync(usersChatId[i], sendedImageFileId, msg, Telegram.Bot.Types.Enums.ParseMode.Html);
                        }
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
