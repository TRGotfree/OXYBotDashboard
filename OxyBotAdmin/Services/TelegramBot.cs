using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Net;

namespace OxyBotAdmin.Services
{
    public class TelegramBot : ITelegramBot
    {

        private readonly ILogger logger;
        private readonly TelegramBotClient telegramBot;
        private readonly BaseService baseService;

        public TelegramBot(ILogger logger, IConfiguration configuration, BaseService baseService)
        {
            this.logger = logger;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            telegramBot = new TelegramBotClient(configuration["BotToken"]);
            this.baseService = baseService;
        }

        public async Task SendMessage(long userChatId, string message)
        {
            await telegramBot.SendTextMessageAsync(userChatId, message, Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public async Task SendMessage2All(long[] usersChatId, string message)
        {
            if (usersChatId == null)
                throw new ArgumentNullException(nameof(usersChatId));

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            for (int i = 0; i < usersChatId.Length; i++)
            {
                try
                {
                    await telegramBot.SendTextMessageAsync(usersChatId[i], message, Telegram.Bot.Types.Enums.ParseMode.Html);
                    await UpdateUserState(usersChatId[i], true);
                }
                catch (Exception ex)
                {
                    await UpdateUserState(usersChatId[i], false);
                    logger.LogError(ex);
                }
            }
        }

        public async Task SendImage2All(long[] usersChatId, Stream stream, string fileName, string msg)
        {
            if (usersChatId == null)
                throw new ArgumentNullException(nameof(usersChatId));

            if (stream == null)
                throw new ArgumentNullException(nameof(usersChatId));

            string sendedImageFileId = string.Empty;

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

                        await UpdateUserState(usersChatId[i], true);
                    }
                    else
                    {
                        var inputOnlinePhoto = new Telegram.Bot.Types.InputFiles.InputOnlineFile(sendedImageFileId);
                        await telegramBot.SendPhotoAsync(usersChatId[i], sendedImageFileId, msg, Telegram.Bot.Types.Enums.ParseMode.Html);
                        await UpdateUserState(usersChatId[i], true);
                    }
                }
                catch (Exception ex)
                {
                    await UpdateUserState(usersChatId[i], false);
                    logger.LogError(ex);
                }
            }
        }

        public async Task SendFileToAll(long[] usersChatId, Stream stream, string fileName, string msg)
        {
            string sendedFileId = string.Empty;
            if (usersChatId == null)
                throw new ArgumentNullException(nameof(usersChatId));

            if (stream == null)
                throw new ArgumentNullException(nameof(usersChatId));

            for (int i = 0; i < usersChatId.Length; i++)
            {
                try
                {
                    if (string.IsNullOrEmpty(sendedFileId))
                    {
                        var inputOnlineFile = new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream, fileName);

                        var sendedFile = await telegramBot.SendDocumentAsync(59725585, inputOnlineFile, msg, Telegram.Bot.Types.Enums.ParseMode.Html);

                        if (sendedFile != null && sendedFile.Document != null && sendedFile.Document.FileId.Length > 0)
                            sendedFileId = sendedFile.Document.FileId;

                        await UpdateUserState(usersChatId[i], true);
                    }
                    else
                    {
                        var inputOnlineFile = new Telegram.Bot.Types.InputFiles.InputOnlineFile(sendedFileId);
                        await telegramBot.SendDocumentAsync(usersChatId[i], inputOnlineFile, msg, Telegram.Bot.Types.Enums.ParseMode.Html);
                        await UpdateUserState(usersChatId[i], true);

                    }
                }
                catch (Exception ex)
                {
                    await UpdateUserState(usersChatId[i], false);
                    logger.LogError(ex);
                }
            }

        }

        public async Task SendVideoToAll(long[] usersChatId, Stream stream, string fileName, string msg)
        {
            string sendedFileId = string.Empty;
            if (usersChatId == null)
                throw new ArgumentNullException(nameof(usersChatId));

            if (stream == null)
                throw new ArgumentNullException(nameof(usersChatId));

            for (int i = 0; i < usersChatId.Length; i++)
            {
                try
                {
                    if (string.IsNullOrEmpty(sendedFileId))
                    {
                        var inputOnlineFile = new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream, fileName);

                        var sendedFile = await telegramBot.SendVideoAsync(59725585, inputOnlineFile, 0, 0, 0, msg, Telegram.Bot.Types.Enums.ParseMode.Html);

                        if (sendedFile != null && sendedFile.Document != null && sendedFile.Document.FileId.Length > 0)
                            sendedFileId = sendedFile.Document.FileId;

                        await UpdateUserState(usersChatId[i], true);
                    }
                    else
                    {
                        var inputOnlineFile = new Telegram.Bot.Types.InputFiles.InputOnlineFile(sendedFileId);
                        await telegramBot.SendVideoAsync(usersChatId[i], inputOnlineFile, 0, 0, 0, msg, Telegram.Bot.Types.Enums.ParseMode.Html);

                        await UpdateUserState(usersChatId[i], true);
                    }
                }
                catch (Exception ex)
                {
                    await UpdateUserState(usersChatId[i], false);
                    logger.LogError(ex);
                }
            }
        }

        public async Task SendAudioToAll(long[] usersChatId, Stream stream, string fileName, string msg)
        {
            string sendedFileId = string.Empty;

            if (usersChatId == null)
                throw new ArgumentNullException(nameof(usersChatId));

            if (stream == null)
                throw new ArgumentNullException(nameof(usersChatId));

            for (int i = 0; i < usersChatId.Length; i++)
            {
                try
                {
                    if (string.IsNullOrEmpty(sendedFileId))
                    {
                        var inputOnlineFile = new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream, fileName);

                        var sendedFile = await telegramBot.SendAudioAsync(59725585, inputOnlineFile, msg, Telegram.Bot.Types.Enums.ParseMode.Html);

                        if (sendedFile != null && sendedFile.Document != null && sendedFile.Document.FileId.Length > 0)
                            sendedFileId = sendedFile.Document.FileId;

                        await UpdateUserState(usersChatId[i], true);
                    }
                    else
                    {
                        var inputOnlineFile = new Telegram.Bot.Types.InputFiles.InputOnlineFile(sendedFileId);
                        await telegramBot.SendAudioAsync(usersChatId[i], inputOnlineFile, msg, Telegram.Bot.Types.Enums.ParseMode.Html);

                        await UpdateUserState(usersChatId[i], true);
                    }
                }
                catch (Exception ex)
                {
                    await UpdateUserState(usersChatId[i], false);
                    logger.LogError(ex);
                }
            }
        }


        private async Task UpdateUserState(long userId, bool isActive)
        {
            await baseService.RepositoryProvider.GetTGUsersConroller().UpdateUserStat(userId, isActive);
        }
    }
}
