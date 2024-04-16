using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITKANSys_api.Utility.Auth
{
    public class UserHelper : IUserHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsIdentity _claimsIdentity;
        private readonly IConfiguration _config;

        public UserHelper(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            _config = config;
        }

        public string GetCurrentUserId()
        {
            return _claimsIdentity?.FindFirst(ClaimTypes.Actor)?.Value;
        }

        public string GetCurrentUserName()
        {
            return _claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetCurrentUserRoleId()
        {
            return _claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value;
        }

        public async Task<string> SendNotification(string token, string title, string msg, int? clientId)
        {
            string response = "";

            try
            {
                var folderName = Path.Combine("Resources");
                var pathserviceAccountKey = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fullPath = Path.Combine(pathserviceAccountKey, "serviceAccountKey.json");

                GoogleCredential credential;
                using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream);
                }

                try
                {
                    FirebaseApp.Create(new AppOptions
                    {
                        Credential = credential
                    });
                }
                catch { }

                // Construct the notification message
                var message = new FirebaseAdmin.Messaging.Message
                {
                    Token = token,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = msg
                    },
                    Data = new Dictionary<string, string>
                    {
                        { "client_id", clientId?.ToString() }
                    }
                };

                System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                // Send the notification
                response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                // Handle the response
                if (response != null)
                {
                    // Notification sent successfully
                }
                else
                {
                    // Failed to send notification
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // Log or rethrow the exception based on your requirements
                throw ex;
            }

            // Return the response
            return response;
        }
    }
}
