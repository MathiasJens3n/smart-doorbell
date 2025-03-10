﻿using Dapper;
using MySqlConnector;
using smart_doorbell_api.Factories.Interfaces;
using smart_doorbell_api.Models;
using smart_doorbell_api.Repositories.Interfaces;
using System.Data;

namespace smart_doorbell_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<DeviceRepository> logger;
        private readonly IDbConnectionFactory dbConnectionFactory;

        public UserRepository(ILogger<DeviceRepository> logger, IDbConnectionFactory dbConnectionFactory) 
        {
            this.logger = logger;
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> AddAsync(User user)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new
                {
                    p_username = user.Username,
                    p_password = user.Password,
                    p_registration_code = user.Registration_Code
                };

                int rowsAffected = await connection.ExecuteAsync("AddUser", parameters, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while adding user.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while adding user.");
            }

            return false;
        }

        public async Task<bool> AddFcmTokenAsync(int userId, string fcmToken)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new
                {
                    token = fcmToken,
                    user_id = userId
                };

                int rowsAffected = await connection.ExecuteAsync("UPDATE user SET fcm_token = @token WHERE id = @user_id;", parameters);
                return rowsAffected > 0;
            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while adding user.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while adding user.");
            }

            return false;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new { p_id = id};

                return await connection.QueryFirstOrDefaultAsync<User>(
                    "GetUserById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while getting user by id.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while getting user by id.");
            }

            return null;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new { p_username = username };

                return await connection.QueryFirstOrDefaultAsync<User>(
                    "GetUserByUsernameuser",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while getting user by username.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while getting user by username.");
            }

            return null;
        }

        public async Task<string?> GetFcmTokenByUserIdAsync(int userId)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new { user_id = userId };

                return await connection.QueryFirstOrDefaultAsync<string>(
                    "SELECT fcm_token FROM user WHERE id = @user_id",
                    parameters
                );
            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while getting token by user id");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database error while getting token by user id");
            }

            return null;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            try
            {
                using var connection = dbConnectionFactory.CreateConnection();

                var parameters = new { 
                    p_id = user.Id,
                    p_username = user.Username,
                    p_password = user.Password,
                };

                int rowsAffected = await connection.ExecuteAsync("UpdateUser", parameters, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;

            }
            catch (MySqlException mysqlEx)
            {
                logger.LogError(mysqlEx, "Database error while updating user.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while updationg user.");
            }

            return false;
        }
    }
}
