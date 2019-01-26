using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OxyBotAdmin.Models;
using OxyBotAdmin.Services;

namespace OxyBotAdmin.DataBaseDomen
{
    public class UserReuestsController
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly int CommandTimeout;

        public UserReuestsController(IGetConnectionString getConnectionString, ILogger _logger, IConfiguration configuration)
        {
            connectionString = getConnectionString.GetConnString();
            logger = _logger;
            CommandTimeout = configuration.GetValue<int>("CommandTimeOut");
        }

        public IEnumerable<UserRequest> GetActions(int beginPage, int endPage)
        {
            List<UserRequest> result = new List<UserRequest>(0);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.GetAdvertisingActions, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = CommandTimeout;
                        command.Parameters.Add("@previousPage", SqlDbType.Int).Value = beginPage;
                        command.Parameters.Add("@nextPage", SqlDbType.Int).Value = endPage;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            UserRequest userRequest;
                            while (reader.Read())
                            {
                                userRequest = new UserRequest();

                                userRequest.RequestId = reader.GetInt64(0);
                                userRequest.RequestText = reader.GetString(1);
                                userRequest.ChatId = reader.GetInt64(2);
                                userRequest.UserName = reader.GetString(3);
                                userRequest.UserFirstName = reader.GetString(4);
                                userRequest.UserLastName = reader.GetString(5);
                                userRequest.MessageDateTime = reader.GetDateTime(6);
                                
                                result.Add(userRequest);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw ex;
            }
            return result;
        }

    }
}
