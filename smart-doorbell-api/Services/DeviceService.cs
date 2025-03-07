using smart_doorbell_api.Dto;
using smart_doorbell_api.Models;
using smart_doorbell_api.Repositories.Interfaces;

namespace smart_doorbell_api.Services
{
    /// <summary>
    /// Service class responsible for device-related business logic.
    /// Provides methods to retrieve and add devices.
    /// </summary>
    public class DeviceService
    {
        private readonly IDeviceRepository deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        /// <summary>
        /// Retrieves a list of devices associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>Returns a collection of devices belonging to the user.</returns>
        public async Task<IEnumerable<Device>> GetDevicesByUserIdAsync(int userId)
        {
            return await deviceRepository.GetByUserIdAsync(userId);
        }

        /// <summary>
        /// Adds a new device to the system.
        /// Converts the provided DeviceDTO into a domain model before saving.
        /// </summary>
        /// <param name="deviceDto">The device data transfer object containing device details.</param>
        /// <returns>Returns userId if found, otherwise null.</returns>
        public async Task<int?> AddDevice(DeviceDTO deviceDto)
        {
            // Convert DTO to Domain Model
            var device = new Device
            {
                Registration_Code = deviceDto.RegistrationCode
            };

            return await deviceRepository.AddDevice(device);
        }
    }
}
