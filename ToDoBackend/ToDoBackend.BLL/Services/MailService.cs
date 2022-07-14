using System;
using System.IO;
using System.Net.Mail;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using ToDoBackend.BLL.Interfaces;

namespace ToDoBackend.BLL.Services
{
    public class MailService : IMailService
    {
        public bool SendAsync(string message)
        {
            try
            {
                UserCredential credential;
                string[] scopes = { GmailService.Scope.GmailSend };
                // Load client secrets.
                using (var stream =
                       new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    /* The file token.json stores the user's access and refresh tokens, and is created
                     automatically when the authorization flow completes for the first time. */
                    string credPath = "token.json";
                    
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Gmail API service.
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "ToDoBackend"
                });

                //var toSend = new MailMessage("To Do", "somebody", "Confirm your email address", message);

                var toSend = new Message()
                {
                    Raw = message,
                    
                };

                //Send message
                Message response = service.Users.Messages.Send(toSend, "me").Execute();

                if (response != null) return true;
                
                return false;
            }
            catch
            {
                return false;
            }
        }
        
        private string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}