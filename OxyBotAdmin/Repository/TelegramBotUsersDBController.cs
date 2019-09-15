using Microsoft.Extensions.Configuration;
using OxyBotAdmin.Models;
using OxyBotAdmin.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Repository
{
    public class TelegramBotUsersDBController
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly int CommandTimeout;

        public TelegramBotUsersDBController(IGetConnectionString getConnectionString, ILogger _logger, IConfiguration configuration)
        {
            if (getConnectionString == null)
                throw new ArgumentNullException(nameof(getConnectionString));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));

            connectionString = getConnectionString.GetConnString();
            logger = _logger;
            CommandTimeout = configuration.GetValue<int>("CommandTimeOut");
        }

        public IEnumerable<TelegramUser> GetTelegramBotUsers()
        {
            List<TelegramUser> tgUsers = new List<TelegramUser>(0);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.GetAllTelegramUsers, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = CommandTimeout;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            TelegramUser tgUser;
                            while (reader.Read())
                            {
                                tgUser = new TelegramUser();
                                tgUser.Id = reader.GetInt32(0);
                                tgUser.ChatId = reader.GetInt64(1);
                                tgUser.NickName = reader.GetString(2);
                                tgUser.FirstName = reader.GetString(3);
                                tgUser.LastName = reader.GetString(4);
                                tgUser.FirstAndLastName = $"{tgUser.FirstName} {tgUser.LastName}";

                                tgUsers.Add(tgUser);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                tgUsers = new List<TelegramUser>(0);
            }
            return tgUsers;
        }

        public IEnumerable<TelegramUserData> GetTelegramBotUsers(int firstPage, int secondPage)
        {
            List<TelegramUserData> tgUsers = new List<TelegramUserData>(0);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.GetTelegramUsersPageByPage, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = CommandTimeout;
                        command.Parameters.Add("@previousPage", SqlDbType.Int).Value = firstPage;
                        command.Parameters.Add("@nextPage", SqlDbType.Int).Value = secondPage;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            TelegramUserData tgUser;

                            while (reader.Read())
                            {
                                tgUser = new TelegramUserData();
                                tgUser.RowNum = reader.GetInt64(0);
                                tgUser.ChatId = reader.GetInt64(1);
                                tgUser.NickName = reader.GetString(2);
                                tgUser.FirstName = reader.GetString(3);
                                tgUser.LastName = reader.GetString(4);
                                tgUser.LastVisitDateTime = reader.GetDateTime(5).ToString("dd.MM.yyyy HH:mm");
                                tgUser.MsgCount = reader.GetInt32(6);
                                tgUser.TotalUserCount = reader.GetInt32(7);
                                tgUser.FirstAndLastName = $"{tgUser.FirstName} {tgUser.LastName}";

                                tgUsers.Add(tgUser);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw;
            }
            return tgUsers;
        }

        public async Task UpdateUserStat(long userId, bool isUserActive)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(SqlScripts.UpdateUserState, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@userId", SqlDbType.BigInt).Value = userId;
                        command.Parameters.Add("@isActive", SqlDbType.Bit).Value = isUserActive;

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw;
            }
        }
    }
}
