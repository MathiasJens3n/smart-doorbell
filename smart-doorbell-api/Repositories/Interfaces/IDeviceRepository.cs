using smart_doorbell_api.Models;

namespace smart_doorbell_api.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        Task<bool> AddDevice(Device device);
    }
}
