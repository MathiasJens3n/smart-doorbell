using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json.Linq;
using smart_doorbell_api.Models;
using smart_doorbell_api.Repositories.Interfaces;

namespace smart_doorbell_api.Services
{
    /// <summary>
    /// Service responsible for handling push notifications via Firebase Cloud Messaging (FCM).
    /// </summary>
    public class NotificationService
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<NotificationService> logger;

        public NotificationService(IConfiguration configuration, IUserRepository userRepository, ILogger<NotificationService> logger)
        {
            string? relativePath = configuration["Firebase:CredentialsPath"];
            string fullPath = Path.Combine(AppContext.BaseDirectory, relativePath ?? "");


            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("Firebase credentials file not found.", fullPath);
            }




            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(fullPath)
                });
            }

            this.userRepository = userRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Sends a push notification to a specific user using Firebase Cloud Messaging (FCM).
        /// </summary>
        /// <param name="userId">The unique identifier of the recipient user.</param>
        /// <param name="title">The title of the notification.</param>
        /// <param name="body">The body content of the notification.</param>
        /// <returns>Returns true if the notification is sent successfully; otherwise, false.</returns>
        public async Task<bool> SendPushNotificationAsync(int userId, string title, string body)
        {
            string? fcmToken = await GetUserFcmTokenAsync(userId);
            if (string.IsNullOrEmpty(fcmToken))
            {
                logger.LogWarning("No FCM token found for user ID: {userId}. Skipping push notification.", userId);
                return false;
            }

            if (fcmToken == null)
            {
                return false;
            }
            var message = new FirebaseAdmin.Messaging.Message
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                },
                Token = fcmToken
            };
            try
            {
                await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return true;
            }
            catch (FirebaseMessagingException ex)
            {
                logger.LogError(ex, "Error sending push notification.");
                return false;
            }
        }

        /// <summary>
        /// Retrieves the FCM token associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>Returns the FCM token if found; otherwise, null.</returns>
        private async Task<string?> GetUserFcmTokenAsync(int userId)
        {
            return await userRepository.GetFcmTokenByUserIdAsync(userId);
        }
    }
}
