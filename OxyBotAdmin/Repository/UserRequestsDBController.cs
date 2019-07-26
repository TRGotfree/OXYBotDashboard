using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OxyBotAdmin.Models;
using OxyBotAdmin.Services;

namespace OxyBotAdmin.Repository
{
    public class UserRequestsDBController
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly int CommandTimeout;

        public UserRequestsDBController(IGetConnectionString getConnectionString, ILogger _logger, IConfiguration configuration)
        {
            if (getConnectionString == null)
                throw new ArgumentNullException(nameof(getConnectionString));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            connectionString = getConnectionString.GetConnString();

            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));

            CommandTimeout = configuration.GetValue<int>("CommandTimeOut");
        }

        public IEnumerable<UserRequest> GetRequests(int beginPage, int endPage)
        {
            List<UserRequest> result = new List<UserRequest>(0);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.GetRequests, connection))
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

                                userRequest.RequestId = reader.GetInt64(1);
                                userRequest.RequestText = reader.GetString(2).Length > 50 ? reader.GetString(2).Substring(0, 50) : reader.GetString(2);
                                userRequest.ChatId = reader.GetInt64(3);
                                userRequest.UserName = reader.GetString(4);
                                userRequest.UserFirstName = reader.GetString(5);
                                userRequest.UserLastName = reader.GetString(6);
                                userRequest.RequestDateTime = reader.GetDateTime(7).ToString("dd.MM.yyyy HH:mm");
                                userRequest.TotalCount = reader.GetInt32(8);
                                userRequest.TodayRequestCount = reader.GetInt32(9);

                                userRequest.UserFirstAndLastName = $"{userRequest.UserFirstName} {userRequest.UserLastName}";

                                result.Add(userRequest);
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
            return result;
        }

    }
}
