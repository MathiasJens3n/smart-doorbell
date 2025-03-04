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
        public async Task<bool> AddDevice(Device device)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new
                {
                    p_device_name = device.Name,
                    p_registration_code = device.RegistrationCode
                };

                int rowsAffected = await connection.ExecuteAsync("AddDevice", parameters, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while adding device.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while adding device.");
            }

            return false;
        }
    }
}
