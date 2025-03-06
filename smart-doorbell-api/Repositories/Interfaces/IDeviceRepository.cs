using smart_doorbell_api.Models;

namespace smart_doorbell_api.Repositories.Interfaces
{
    /// <summary>
    /// Interface for device repository operations.
    /// Provides methods for retrieving and adding devices.
    /// </summary>

    public interface IDeviceRepository
    {
        /// <summary>
        /// Retrieves a list of devices associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>Returns a collection of devices belonging to the user.</returns>
        Task<IEnumerable<Device>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Adds a new device to the repository.
        /// </summary>
        /// <param name="device">The device entity to be added.</param>
        /// <returns>Returns true if the device was added successfully; otherwise, returns false.</returns>
        Task<bool> AddDevice(Device device);
    }
}
