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
    public class DistrictDBController
    {

        private readonly string connectionString;
        private readonly ILogger logger;
        private readonly int CommandTimeout;

        public DistrictDBController(IGetConnectionString getConnectionString, ILogger _logger, IConfiguration configuration)
        {
            connectionString = getConnectionString.GetConnString();
            logger = _logger;
            CommandTimeout = configuration.GetValue<int>("CommandTimeOut");
        }

        public IEnumerable<District> GetDistricts()
        {
            List<District> result = new List<District>(0);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(SqlScripts.GetDistricts, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = CommandTimeout;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            District dist;
                            while (reader.Read())
                            {
                                dist = new District();

                                dist.Id = reader.GetInt32(0);
                                dist.Command = reader.GetString(1);
                                dist.Name = reader.GetString(2);

                                result.Add(dist);
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
