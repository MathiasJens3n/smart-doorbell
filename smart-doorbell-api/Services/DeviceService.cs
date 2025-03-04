using smart_doorbell_api.Dto;
using smart_doorbell_api.Models;
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
        public async Task<bool> AddDevice(DeviceDTO deviceDto)
        {
            // Convert DTO to Domain Model
            var device = new Device
            {
                Name = deviceDto.Name,
                RegistrationCode = deviceDto.RegistrationCode
            };

            return await deviceRepository.AddDevice(device);
        }
    }
}
