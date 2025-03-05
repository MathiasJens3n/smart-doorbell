using Microsoft.AspNetCore.Mvc;
using smart_doorbell_api.Dto;
using smart_doorbell_api.Services;

namespace smart_doorbell_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceService deviceService;
        public DeviceController(DeviceService deviceService)
        {
            this.deviceService = deviceService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDevice([FromBody]DeviceDTO device)
        {
            if (device == null)
            {
                return BadRequest("Device data is required.");
            }

            var result = await deviceService.AddDevice(device);

            if (result)
            {
                return Created();
            }

            return StatusCode(500, "Failed to add device.");
        }
    }
}
