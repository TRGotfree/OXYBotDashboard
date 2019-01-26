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
    public class GoodAnnotationsDbController
    {
        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly int CommandTimeout;

        public GoodAnnotationsDbController(IGetConnectionString getConnectionString, ILogger _logger, IConfiguration configuration)
        {
            connectionString = getConnectionString.GetConnString();
            logger = _logger;
            CommandTimeout = configuration.GetValue<int>("CommandTimeOut");
        }

        public IEnumerable<GoodAnnotation> GetAnnotations(int beginPage, int endPage)
        {
            List<GoodAnnotation> result = new List<GoodAnnotation>(0);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.GetAnnotations, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = CommandTimeout;
                        command.Parameters.Add("@previousPage", SqlDbType.Int).Value = beginPage;
                        command.Parameters.Add("@nextPage", SqlDbType.Int).Value = endPage;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            GoodAnnotation annotation;
                            while (reader.Read())
                            {
                                annotation = new GoodAnnotation();

                                annotation.AnnotationId = reader.GetInt32(0);
                                annotation.DrugName = reader.GetString(1).Length > 50 ? reader.GetString(1).Substring(0, 50) : reader.GetString(1);
                                annotation.Producer = reader.GetString(2).Length > 50 ? reader.GetString(2).Substring(0, 50) : reader.GetString(2);
                                annotation.UsingWay = reader.GetString(3).Length > 50 ? reader.GetString(3).Substring(0, 50) : reader.GetString(3);
                                annotation.ForWhatIsUse = reader.GetString(4).Length > 50 ? reader.GetString(4).Substring(0, 50) : reader.GetString(4);
                                annotation.SpecialInstructions = reader.GetString(5).Length > 50 ? reader.GetString(5).Substring(0, 50) : reader.GetString(5);
                                annotation.ContraIndicators = reader.GetString(6).Length > 50 ? reader.GetString(6).Substring(0, 50) : reader.GetString(6);
                                annotation.SideEffects = reader.GetString(7).Length > 50 ? reader.GetString(7).Substring(0, 50) : reader.GetString(7);
                                annotation.IsImageExists = reader.GetInt32(8) > 0;
                                annotation.TotalCountOfAnnotations = reader.GetInt32(9);

                                result.Add(annotation);
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
