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
    public class DrugStoreDBController : IDataBaseDomenController
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly int CommandTimeout;

        public DrugStoreDBController(IGetConnectionString getConnectionString, ILogger _logger, IConfiguration configuration)
        {
            connectionString = getConnectionString.GetConnString();
            logger = _logger;
            CommandTimeout = configuration.GetValue<int>("CommandTimeOut");
        }

        public IEnumerable<DrugStore> GetDrugStores(int beginPage, int endPage)
        {
            List<DrugStore> listResult = new List<DrugStore>(0);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.GetDrugStores, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = CommandTimeout;
                        command.Parameters.Add("@previousPage", SqlDbType.Int).Value = beginPage;
                        command.Parameters.Add("@nextPage", SqlDbType.Int).Value = endPage;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DrugStore ds;
                            while (reader.Read())
                            {
                                ds = new DrugStore();

                                ds.Id = (uint)reader.GetInt32(1);
                                ds.DrugStoreId = (uint)reader.GetInt32(2);
                                ds.DrugStoreName = reader.GetString(3);
                                ds.Address = reader.GetString(4);
                                ds.Status = reader.GetBoolean(5);
                                ds.Phone = reader.GetString(6);
                                ds.WorkTime = reader.GetString(7);
                                ds.Orientir = reader.GetString(8);
                                ds.District = reader.GetString(9);
                                ds.ShortName = reader.GetString(10);
                                ds.DrugStoreTotalCount = reader.GetInt32(11);

                                listResult.Add(ds);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message} {ex.StackTrace}");
                throw ex;
            }

            return listResult;
        }

        public void InsertDrugStore(DrugStore drugStore)
        {
            try
            {
                if (drugStore == null) 
                    throw new ArgumentNullException(nameof(drugStore));

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(SqlScripts.InsertDrugStore, connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandTimeout = CommandTimeout;
                                command.Transaction = transaction;

                                command.Parameters.Add("@name", SqlDbType.NVarChar, 75).Value = drugStore.DrugStoreName;
                                command.Parameters.Add("@address", SqlDbType.NVarChar, 100).Value = drugStore.Address;
                                command.Parameters.Add("@status", SqlDbType.Bit).Value = drugStore.Status;
                                command.Parameters.Add("@phone", SqlDbType.NVarChar, 50).Value = drugStore.Phone;
                                command.Parameters.Add("@workTime", SqlDbType.NVarChar, 100).Value = drugStore.WorkTime;
                                command.Parameters.Add("@orientir", SqlDbType.NVarChar, 100).Value = drugStore.Orientir;
                                command.Parameters.Add("@district", SqlDbType.NVarChar, 50).Value = drugStore.District;
                                command.Parameters.Add("@shortName", SqlDbType.NVarChar, 15).Value = drugStore.ShortName;

                                command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            logger.LogError(ex.StackTrace);
                            throw ex;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public void UpdateDrugStore(DrugStore drugStore)
        {
            try
            {
                if (drugStore == null)
                    throw new ArgumentNullException(nameof(drugStore));

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(SqlScripts.UpdateDrugStore, connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandTimeout = CommandTimeout;
                                command.Transaction = transaction;

                                command.Parameters.Add("@id", SqlDbType.Int).Value = drugStore.Id;
                                command.Parameters.Add("@drugStoreId", SqlDbType.Int).Value = drugStore.DrugStoreId;
                                command.Parameters.Add("@name", SqlDbType.NVarChar, 75).Value = drugStore.DrugStoreName;
                                command.Parameters.Add("@address", SqlDbType.NVarChar, 100).Value = drugStore.Address;
                                command.Parameters.Add("@status", SqlDbType.Bit).Value = drugStore.Status;
                                command.Parameters.Add("@phone", SqlDbType.NVarChar, 50).Value = drugStore.Phone;
                                command.Parameters.Add("@workTime", SqlDbType.NVarChar, 100).Value = drugStore.WorkTime;
                                command.Parameters.Add("@orientir", SqlDbType.NVarChar, 100).Value = drugStore.Orientir;
                                command.Parameters.Add("@district", SqlDbType.NVarChar, 50).Value = drugStore.District;
                                command.Parameters.Add("@shortName", SqlDbType.NVarChar, 15).Value = drugStore.ShortName;

                                command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            logger.LogError(ex.StackTrace);
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

    }
}
