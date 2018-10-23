using OxyBotAdmin.Models;
using OxyBotAdmin.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace OxyBotAdmin.DataBaseDomen
{
    public class LoginDbController :IDataBaseDomenController
    {
        private readonly string connectionString;
        private  ILogger logger;

        public LoginDbController(IGetConnectionString getConnectiontring, ILogger _logger)
        {
            connectionString = getConnectiontring.GetConnString();
            logger = _logger;
        }

        public BotAdmin GetBotAdmin(string login, string pass)
        {
            BotAdmin botAdmin = new BotAdmin();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.GetBotAdmin, connection)) //TODO Добавить хранимую процедуру
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@login", SqlDbType.NVarChar, 100).Value = login;
                        command.Parameters.Add("@pass", SqlDbType.NVarChar, 64).Value = pass;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                botAdmin.Id = reader.GetInt32(0);
                                botAdmin.Login = reader.GetString(1);
                                botAdmin.Password = reader.GetString(2);
                                botAdmin.State = reader.GetBoolean(3);
                            }
                        }    
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                throw ex;
            }
            return botAdmin;
        }

        public bool CheckAdmin(BotAdmin botAdmin)
        {
            bool checkResult = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.CheckBotAdmin, connection))//TODO Добавить хранимую процедуру
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@login", SqlDbType.NVarChar, 100).Value = botAdmin.Login;
                        command.Parameters.Add("@pass", SqlDbType.NVarChar, 64).Value = botAdmin.Password;
                        checkResult = Convert.ToBoolean(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                throw ex;
            }
            return checkResult;
        }
    }
}
