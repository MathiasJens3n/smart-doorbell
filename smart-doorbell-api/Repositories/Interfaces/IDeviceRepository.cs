using smart_doorbell_api.Dto;

namespace smart_doorbell_api.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        Task<bool> AddDevice(DeviceDTO device);
    }
}
