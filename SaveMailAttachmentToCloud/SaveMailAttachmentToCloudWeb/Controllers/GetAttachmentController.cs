using AttachmentsDemoWeb.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Xml;

namespace AttachmentsDemoWeb.Controllers
{
    public class GetAttachmentController : ApiController
    {

        // Final Step for One Drive to save file
        [HttpPost()]
        public string SaveAttachment(AttachmentRequest request)
        {
            try
            {
                Attachment attachment = AppUtility.GetAttachment(request.AttachmentId, request.AuthToken, request.EwsUrl);
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
            
            string accessToken = OAuthController.GetAccessTokenFromRefreshToken(oneDriveResourceId);

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

      

       
    }
}