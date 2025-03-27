using Microsoft.Extensions.Configuration;
using MySqlConnector;
using smart_doorbell_api.Factories.Interfaces;
using System.Data;
using System.Runtime.InteropServices;

namespace smart_doorbell_api.Factories
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public MySqlConnectionFactory(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            // Detect OS and pick the appropriate connection string
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _connectionString = configuration.GetConnectionString("LinuxConnection");
            }
            else
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }
        }
        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
