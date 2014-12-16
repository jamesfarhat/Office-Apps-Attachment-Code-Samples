
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

namespace AttachmentsDemoWeb.Controllers
{
    public class DropboxController : ApiController
    {
        private static readonly Uri RedirectUrl = new Uri(System.Web.HttpContext.Current.Request.Url, "/App/DropboxRedirect.html");

        // Step 1: to get redirect URL to authenticate from dropbox.
        [HttpPost()]
        public string GetAuthorizationUrl()
        {
            DropNetClient _client = new DropNetClient(AppConstants.DropboxClientId, AppConstants.DropboxClientSecret);
            
             var token = _client.GetToken();
             Storage.Dropbox.Token = token;
            var url = _client.BuildAuthorizeUrl(RedirectUrl + "?dropboxcallback=1");
            

            return url;
        }

        // Last Step: to save attachment to dropbox.
        [HttpPost()]
        public string SaveAttachment(Attachment attachment)
        {

            try
            {
                DropNetClient _client = new DropNetClient(AppConstants.DropboxClientId, AppConstants.DropboxClientSecret);


                _client.UserLogin = Storage.Dropbox.Token;

                DropNet.Models.UserLogin login = _client.GetAccessToken();



               

                _client.UploadFile("/", attachment.AttachmentName, attachment.AttachmentBytes);
                return "Uploaded Sucessfully.";
            }
            catch (Exception s)
            {
                return s.Message;
            }
               
            //return "";
        }
    }
}