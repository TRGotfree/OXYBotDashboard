using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OxyBotAdmin.Models;
using OxyBotAdmin.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ILogger = OxyBotAdmin.Services.ILogger;

namespace OxyBotAdmin.Repository
{
    public class DiscountDBController
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly int CommandTimeout;

        public DiscountDBController(IGetConnectionString getConnectionString, ILogger _logger, IConfiguration configuration)
        {
            if (getConnectionString == null)
                throw new ArgumentNullException(nameof(getConnectionString));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            logger = _logger ?? throw new ArgumentNullException(nameof(_logger));

            connectionString = getConnectionString.GetConnString();
            CommandTimeout = configuration.GetValue<int>("CommandTimeOut");
        }

        public IEnumerable<DiscountCard> GetDiscountCardsData(int beginPage, int endPage)
        {
            List<DiscountCard> result = new List<DiscountCard>(0);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.GetDiscountsCardsData, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = CommandTimeout;
                        command.Parameters.Add("@previousPage", SqlDbType.Int).Value = beginPage;
                        command.Parameters.Add("@nextPage", SqlDbType.Int).Value = endPage;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DiscountCard discount;
                            while (reader.Read())
                            {
                                discount = new DiscountCard();

                                discount.CardId = (uint)reader.GetInt32(2);
                                discount.UserFIO = reader.GetString(3);
                                discount.BirthDate = reader.GetDateTime(4).ToString("dd.MM.yyyy");
                                discount.Phone = reader.GetString(5);
                                discount.Email = reader.GetString(6);
                                discount.Sex = reader.GetString(7);
                                discount.IsUserWantsToGetUpdates = reader.GetBoolean(8);
                                discount.IsRegistered = reader.GetBoolean(9);
                                discount.ChatId = (ulong)reader.GetInt64(10);
                                discount.TotalCountOfCardsData = reader.GetInt32(11);
                                discount.DateTimeEntered = reader.GetDateTime(12).ToString("dd.MM.yyyy");

                                result.Add(discount);
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

        public async Task InsertOrUpdateDiscountCardData(DiscountCard cardData)
        {
            try
            {
                if (cardData == null)
                    throw new ArgumentException(nameof(cardData));

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(SqlScripts.InsertOrUpdateDiscountCardData, connection, transaction))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                command.Parameters.Add("@cardId", SqlDbType.Int).Value = cardData.CardId;
                                command.Parameters.Add("@chatId", SqlDbType.BigInt).Value = cardData.ChatId;
                                command.Parameters.Add("@userFIO", SqlDbType.NVarChar, 200).Value = cardData.UserFIO;
                                command.Parameters.Add("@birthDate", SqlDbType.Date).Value = cardData.BirthDate;
                                command.Parameters.Add("@phone", SqlDbType.NVarChar, 50).Value = cardData.Phone;

                                var emailParam = command.Parameters.Add("@email", SqlDbType.NVarChar, 50);
                                if (string.IsNullOrEmpty(cardData.Email) || string.IsNullOrWhiteSpace(cardData.Email) || cardData.Email == "-")
                                    emailParam.Value = DBNull.Value;
                                else
                                    emailParam.Value = cardData.Email;

                                command.Parameters.Add("@sex", SqlDbType.NVarChar, 6).Value = cardData.Sex;
                                command.Parameters.Add("@isUserWantToGetNews", SqlDbType.Bit).Value = cardData.IsUserWantsToGetUpdates;
                                command.Parameters.Add("@isRegistered", SqlDbType.Bit).Value = cardData.IsRegistered;

                                await command.ExecuteNonQueryAsync();
                                transaction.Commit(); 
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw;
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
                            throw;
                        }
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
