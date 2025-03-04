using MySqlConnector;
using smart_doorbell_api.Factories.Interfaces;
using System.Data;

namespace smart_doorbell_api.Factories
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public MySqlConnectionFactory(string connectionString)
        {
            ArgumentNullException.ThrowIfNull(connectionString);

            _connectionString = connectionString;
        }
        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
