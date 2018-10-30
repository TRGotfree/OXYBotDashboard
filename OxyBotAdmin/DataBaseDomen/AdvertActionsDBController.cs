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
    public class AdvertActionsDBController : IDataBaseDomenController
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly int CommandTimeout;

        public AdvertActionsDBController(IGetConnectionString getConnectionString, ILogger _logger, IConfiguration configuration)
        {
            connectionString = getConnectionString.GetConnString();
            logger = _logger;
            CommandTimeout = configuration.GetValue<int>("CommandTimeOut");
        }

        public IEnumerable<AdvertAction> GetActions(int beginPage, int endPage)
        {
            List<AdvertAction> result = new List<AdvertAction>(0);
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
                            AdvertAction action;
                            while (reader.Read())
                            {
                                action = new AdvertAction();

                                action.ActionId = (uint)reader.GetInt32(1);
                                action.AdvertisingText = reader.GetString(2);
                                action.NameOfAction = reader.GetString(3);
                                action.DateBegin = reader.GetDateTime(4).Date;
                                action.DateEnd = reader.GetDateTime(5).Date;
                                action.CommandText = reader.GetString(6);
                                action.State = reader.GetBoolean(7);
                                action.TotalCountOfAdvertActions = reader.GetInt32(8);

                                action.SetFormattedDateBegin(action.DateBegin);
                                action.SetFormattedDateEnd(action.DateEnd);
                                action.SetAdvertisingTextShort(action.AdvertisingText);
                                result.Add(action);
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

        public void UpdateAction(AdvertAction advertAction)
        {
            try
            {
                if (advertAction == null)
                    throw new ArgumentException(nameof(advertAction));

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(SqlScripts.UpdateAdvertisingAction, connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandTimeout = CommandTimeout;
                                command.Transaction = transaction;

                                command.Parameters.Add("@advertId", SqlDbType.Int).Value = advertAction.ActionId;
                                command.Parameters.Add("@advertText", SqlDbType.NVarChar, 2000).Value = advertAction.AdvertisingText;
                                command.Parameters.Add("@nameOfAction", SqlDbType.NVarChar, 50).Value = advertAction.NameOfAction;
                                command.Parameters.Add("@dateBegin", SqlDbType.SmallDateTime).Value = advertAction.DateBegin;
                                command.Parameters.Add("@dateEnd", SqlDbType.SmallDateTime).Value = advertAction.DateEnd;
                                command.Parameters.Add("@commandText", SqlDbType.NVarChar, 30).Value = advertAction.CommandText;
                                command.Parameters.Add("@advertState", SqlDbType.Bit).Value = advertAction.State;

                                command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            logger.LogError(ex);
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw ex;
            }
        }


        public void InsertAction(AdvertAction advertAction)
        {
            try
            {
                if (advertAction == null)
                    throw new ArgumentException(nameof(advertAction));

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(SqlScripts.InsertAdvertisingAction, connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandTimeout = CommandTimeout;
                                command.Transaction = transaction;

                                command.Parameters.Add("@NameOfAction", SqlDbType.VarChar, 50).Value = advertAction.NameOfAction;
                                command.Parameters.Add("@DateBegin", SqlDbType.SmallDateTime).Value = advertAction.DateBegin;
                                command.Parameters.Add("@DateEnd", SqlDbType.SmallDateTime).Value = advertAction.DateEnd;
                                command.Parameters.Add("@TextAbout", SqlDbType.VarChar, 2000).Value = advertAction.AdvertisingText;
                                command.Parameters.Add("@CommandText", SqlDbType.VarChar, 30).Value = advertAction.CommandText;
                                
                                command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            logger.LogError(ex);
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw ex;
            }
        }
    }
}
