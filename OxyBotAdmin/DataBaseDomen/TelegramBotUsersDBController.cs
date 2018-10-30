using Microsoft.Extensions.Configuration;
using OxyBotAdmin.Models;
using OxyBotAdmin.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.DataBaseDomen
{
    public class TelegramBotUsersDBController : IDataBaseDomenController
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly int CommandTimeout;

        public TelegramBotUsersDBController(IGetConnectionString getConnectionString, ILogger _logger, IConfiguration configuration)
        {
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

                                tgUsers.Add(tgUser);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                tgUsers = new List<TelegramUserData>(0);
            }
            return tgUsers;
        }
    }
}
