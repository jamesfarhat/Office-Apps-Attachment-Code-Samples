
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AttachmentsDemoWeb.Constants;
using DropNet;
using Acts.REST.GoogleDrive;

namespace AttachmentsDemoWeb.Controllers
{
    public class GoogleDriveController : ApiController
    {
        private static readonly Uri googledriveRedirectUrl = new Uri(System.Web.HttpContext.Current.Request.Url, "/App/GoogleDriveRedirect.html");
       

        // Step 1: to get redirect URL to authenticate from Google Drive
        [HttpPost()]
        public string GetAuthorizationUrl()
        {
            DriveRestClient client = new DriveRestClient(AppConstants.GoogleDriveClientId, AppConstants.DropboxClientSecret, googledriveRedirectUrl.ToString());

            var url = client.GetPermissionURL();
            

            return url;
        }

        // Step 2: To get token from code.
        [HttpPost()]
        public string CompleteOAuthFlow(AuthorizationParameters parameters)
        {
            try
            {
                DriveRestClient restClient = new DriveRestClient(AppConstants.GoogleDriveClientId, AppConstants.GoogleDriveClientSecret, googledriveRedirectUrl.ToString());

                DriveToken token = restClient.GetTokenfromCode(parameters.Code);

                Storage.GoogleDrive.Token = token;
                return "OAuth succeeded";
            }
            catch (Exception ex)// ActiveDirectoryAuthenticationException ex)
            {
                return "OAuth failed. " + ex.ToString();
            }
        }

        // Last Step: to save attachment to google drive.
        [HttpPost()]
        public string SaveAttachment(AttachmentsDemoWeb.Constants.Attachment attachment)
        {

            try
            {
                DriveRestClient restClient = new DriveRestClient(AppConstants.GoogleDriveClientId, AppConstants.DropboxClientSecret, googledriveRedirectUrl.ToString());


                restClient.Token = Storage.GoogleDrive.Token;

                
                

                string uploadText = restClient.UploadFile(attachment.AttachmentName,attachment.AttachmentBytes);
                return "Uploaded Sucessfully.";
            }
            catch (Exception s)
            {
                return s.Message;
            }
               
            //return "";
        }


      


       

        public class AuthorizationParameters
        {
            public string Code { get; set; }
        }
    }
}