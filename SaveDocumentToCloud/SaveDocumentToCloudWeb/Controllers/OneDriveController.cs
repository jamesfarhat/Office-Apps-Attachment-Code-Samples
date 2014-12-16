using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AttachmentsDemoWeb.Constants;
using System.IO;

namespace AttachmentsDemoWeb.Controllers
{
    public class OneDriveController : ApiController
    {
        

        private const string OAuthUrl = "https://login.windows.net/{0}";
        private static readonly string AuthorizeUrl = string.Format(CultureInfo.InvariantCulture,
            OAuthUrl,
            "common/oauth2/authorize?response_type=code&client_id={0}&resource={1}&redirect_uri={2}");
        private static readonly Uri RedirectUrl = new Uri(            System.Web.HttpContext.Current.Request.Url, "/App/OneDriveRedirect.html");

        // Step 1: to get redirect URL to authenticate from One Drive
        [HttpPost()]
        public string GetAuthorizationUrl(AuthorizationRequest request)
        {
            return String.Format(CultureInfo.InvariantCulture,
                AuthorizeUrl,
                Uri.EscapeDataString(AppConstants.OneDriveClientId),
                Uri.EscapeDataString(AppConstants.oneDriveResourceId),
                Uri.EscapeDataString(RedirectUrl.ToString())
            );
        }

        // Step 2: To get token from code.
        [HttpPost()]
        public string CompleteOAuthFlow(AuthorizationParameters parameters) {
            try
            {
                ClientCredential credential = new ClientCredential(AppConstants.OneDriveClientId, AppConstants.OneDriveClientSecret);
                string authority = string.Format(CultureInfo.InvariantCulture, OAuthUrl, "common");
                AuthenticationContext authContext = new AuthenticationContext(authority);
                AuthenticationResult result = authContext.AcquireTokenByAuthorizationCode(
                    parameters.Code, new Uri(RedirectUrl.GetLeftPart(UriPartial.Path)), credential);

                // Cache the access token and refresh token
                Storage.OneDrive.RefreshToken = result.RefreshToken;
                return "OAuth succeeded";
            }
            catch (Exception ex)// ActiveDirectoryAuthenticationException ex)
            {
                return "OAuth failed. " + ex.ToString();
            }
        }

        /// <summary>
        /// Try to get a new access token for this resource using a refresh token.
        /// If successful, this method will cache the access token for future use.
        /// If this fails, return null, signaling the caller to do the OAuth redirect.
        /// </summary>
        public static string GetAccessTokenFromRefreshToken(string resourceId)
        {
            // Redeem the refresh token for an access token:
            try
            {
                string refreshToken = Storage.OneDrive.RefreshToken;
                ClientCredential credential = new ClientCredential(AppConstants.OneDriveClientId, AppConstants.OneDriveClientSecret);
                string authority = string.Format(CultureInfo.InvariantCulture, OAuthUrl, "common");
                AuthenticationContext authContext = new AuthenticationContext(authority);
                AuthenticationResult result = authContext.AcquireTokenByRefreshToken(
                    refreshToken, credential, resourceId);
                
                return result.AccessToken;
            }
            catch (Exception)//ActiveDirectoryAuthenticationException)
            {
                return null;
            }
        }

        // Final Step for One Drive to save file
        [HttpPost()]
        public string SaveToOneDrive(Attachment attachment)
        {
            try
            {
                
                return SaveAttachment(attachment);
            }
            catch (Exception e)
            {
                return "There was an exception: " + e.Message + "\n\n" + e.StackTrace;
            }
        }

        private string SaveAttachment(Attachment attachment)
        {
            string oneDriveResourceId = AppConstants.oneDriveResourceId;
            string oneDriveApiEndpoint = AppConstants.oneDriveApiEndpoint;

            string accessToken = GetAccessTokenFromRefreshToken(oneDriveResourceId);

            // Prepare the HTTP request using the new "File" APIs
            HttpWebRequest webRequest =
                WebRequest.CreateHttp(oneDriveApiEndpoint + "/files/Add(name='" + attachment.AttachmentName + "', overwrite=true)");
            webRequest.Accept = "application/json;odata=verbose";
            webRequest.Headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
            webRequest.Method = "POST";
            webRequest.ContentLength = attachment.AttachmentBytes.Length;
            webRequest.ContentType = "application/octet-stream";

            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(attachment.AttachmentBytes, 0, attachment.AttachmentBytes.Length);
            requestStream.Close();

            // Make the request to SharePoint and get the response.
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            // If the response is okay, read it
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                return reader.ReadToEnd();
            }

            return "StatusCode was not OK!";
        }

        

        public class AuthorizationRequest
        {
            public string ResourceId { get; set; }
        }

        public class AuthorizationParameters
        {
            public string Code { get; set; }
        }
    }
}