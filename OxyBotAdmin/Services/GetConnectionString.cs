using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Services
{
    public class GetConnectionString : IGetConnectionString
    {
        private readonly string connectionString;

        public GetConnectionString(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Dev");
        }

        public string GetConnString()
        {
            return connectionString;
        }
    }
}
