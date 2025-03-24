using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_doorbell_api.Dto;
using smart_doorbell_api.Models;
using smart_doorbell_api.Services;
using smart_doorbell_api.Tools;

namespace smart_doorbell_api.Controllers
{
    /// <summary>
    /// Controller for handling image-related operations such as adding, deleting, and retrieving images.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ImageService imageService;
        private readonly NotificationService notificationService;

        public ImageController(ImageService imageService, NotificationService notificationService)
        {
            this.imageService = imageService;
            this.notificationService = notificationService;
        }

        /// <summary>
        /// Adds a new image to the system.
        /// </summary>
        /// <param name="imageDto">The image data transfer object containing image details.</param>
        /// <returns>Returns a Created response if successful, or a Bad Request/Server Error response if an issue occurs.</returns>
        [HttpPost]
        public async Task<IActionResult> AddImage([FromBody] ImageDTO imageDto)
        {
            if (imageDto == null || imageDto.Data == null)
            {
                return BadRequest("Invalid image data.");
            }

            bool result = await imageService.AddImageAsync(imageDto);

            if (!result)
            {
                return StatusCode(500, "Error adding image.");
            }

            // Send a push notification
            await notificationService.SendPushNotificationAsync(imageDto.UserId, "Doorbell Pressed", "Someone is at your door!");

            return Created();
        }

        /// <summary>
        /// Deletes an image by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the image to be deleted.</param>
        /// <returns>Returns No Content if deleted successfully, Not Found if the image does not exist.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteImage(int id)
        {
            bool result = await imageService.DeleteImageAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Retrieves all images associated with the authenticated user.
        /// </summary>
        /// <returns>Returns a list of images if found, Not Found if no images are available, or Unauthorized if the user token is invalid.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetImagesByUserId()
        {
            int userIdFromToken = UserHelper.GetUserIdFromToken(HttpContext);

            if (userIdFromToken == -1)
            {
                return Unauthorized("Invalid or missing user token.");
            }

            IEnumerable<ImageDTO> images = await imageService.GetImagesByUserIdAsync(userIdFromToken);

            if (images == null || !images.Any())
            {
                return NotFound();
            }

            return Ok(images);
        }
    }
}
