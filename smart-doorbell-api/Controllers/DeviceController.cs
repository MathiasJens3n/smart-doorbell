using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smart_doorbell_api.Dto;
using smart_doorbell_api.Services;
using smart_doorbell_api.Tools;
using System.Security.Claims;

namespace smart_doorbell_api.Controllers
{
    /// <summary>
    /// Controller responsible for managing device-related operations.
    /// Provides endpoints for retrieving and adding devices.
    /// </summary>

    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceService deviceService;
        public DeviceController(DeviceService deviceService)
        {
            this.deviceService = deviceService;
        }

        /// <summary>
        /// Retrieves a list of devices associated with the authenticated user.
        /// Requires a valid authorization token.
        /// </summary>
        /// <returns>Returns a list of devices if successful; otherwise, returns an error response.</returns>

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDevices()
        {
            int userIdFromToken = UserHelper.GetUserIdFromToken(HttpContext);

            if (userIdFromToken == -1)
            {
                return Unauthorized("Invalid or missing user token.");
            }

            var devices = await deviceService.GetDevicesByUserIdAsync(userIdFromToken);

            return Ok(devices);
        }

        /// <summary>
        /// Adds a new device to the system.
        /// </summary>
        /// <param name="device">The device data transfer object containing device details.</param>
        /// <returns>Returns a success response if the device was added successfully; otherwise, returns an error response.</returns>

        [HttpPost]
        public async Task<IActionResult> AddDevice([FromBody] DeviceDTO device)
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
