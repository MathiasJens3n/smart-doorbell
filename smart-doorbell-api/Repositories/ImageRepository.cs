using Dapper;
using MySqlConnector;
using smart_doorbell_api.Factories.Interfaces;
using smart_doorbell_api.Models;
using smart_doorbell_api.Repositories.Interfaces;
using System.Data;

namespace smart_doorbell_api.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ILogger<ImageRepository> logger;
        private readonly IDbConnectionFactory dbConnectionFactory;

        public ImageRepository(ILogger<ImageRepository> logger, IDbConnectionFactory dbConnectionFactory)
        {
            this.logger = logger;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> AddAsync(Image image)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new
                {
                    p_data = image.Data,
                    p_user_id = image.User_Id
                };

                int rowsAffected = await connection.ExecuteAsync("AddImage", parameters, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while adding image.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while adding image.");
            }

            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new
                {
                    p_id = id,
                };

                int rowsAffected = await connection.ExecuteAsync("DeleteImage", parameters, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while deleting image.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while deleting image.");
            }

            return false;
        }

        public async Task<IEnumerable<Image>> GetByUserIdAsync(int userId)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new { p_user_id = userId };

                return await connection.QueryAsync<Image>(
                    "GetImages",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while getting iamge by user id.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error whilewhile getting iamge by user id.");
            }

            return Enumerable.Empty<Image>();
        }
    }
}
