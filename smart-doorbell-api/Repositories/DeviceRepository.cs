using Dapper;
using MySqlConnector;
using smart_doorbell_api.Factories.Interfaces;
using smart_doorbell_api.Models;
using smart_doorbell_api.Repositories.Interfaces;
using System.Data;

namespace smart_doorbell_api.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ILogger<DeviceRepository> logger;
        private readonly IDbConnectionFactory dbConnectionFactory;

        public DeviceRepository(ILogger<DeviceRepository> logger, IDbConnectionFactory dbConnectionFactory)
        {
            this.logger = logger;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Device>> GetByUserIdAsync(int userId)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new { p_user_id = userId };

                return await connection.QueryAsync<Device>(
                    "GetDevices",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while getting devices by registration code.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while getting devices by registration code.");
            }

            return Enumerable.Empty<Device>();
        }

        public async Task<int?> AddDevice(Device device)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("p_registration_code", device.RegistrationCode, DbType.String);
                parameters.Add("p_user_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("AddDevice", parameters, commandType: CommandType.StoredProcedure);

                // Retrieve the user_id from the output parameter
                int? userId = parameters.Get<int?>("p_user_id");

                return userId;
            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while adding device.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while adding device.");
            }

            return null;
        }
    }
}
