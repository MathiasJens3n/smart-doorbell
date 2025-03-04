using smart_doorbell_api.Dto;
using smart_doorbell_api.Repositories.Interfaces;

namespace smart_doorbell_api.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }
        public async Task<bool> AddDevice(DeviceDTO device)
        {
            return await deviceRepository.AddDevice(device);
        }
    }
}
