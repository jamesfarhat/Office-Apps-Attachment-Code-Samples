using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace AttachmentsDemoWeb.Constants
{
    public class AppUtility
    {
        // copied from Outlook Power Hour code.
        public static Attachment GetAttachment(string attachmentId, string authToken, string ewsUrl)
        {
            string getAttachmentRequest =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""
                xmlns:t=""http://schemas.microsoft.com/exchange/services/2006/types"">
                <soap:Header>
                <t:RequestServerVersion Version=""Exchange2013"" />
                </soap:Header>
                    <soap:Body>
                    <GetAttachment xmlns=""http://schemas.microsoft.com/exchange/services/2006/messages""
                    xmlns:t=""http://schemas.microsoft.com/exchange/services/2006/types"">
                        <AttachmentShape/>
                        <AttachmentIds>
                        <t:AttachmentId Id=""{0}""/>
                        </AttachmentIds>
                    </GetAttachment>
                    </soap:Body>
                </soap:Envelope>";
            getAttachmentRequest = String.Format(getAttachmentRequest, attachmentId);

            // Prepare a web request object.
            HttpWebRequest webRequest = WebRequest.CreateHttp(ewsUrl);
            webRequest.Headers.Add("Authorization", string.Format("Bearer {0}", authToken));
            webRequest.PreAuthenticate = true;
            webRequest.AllowAutoRedirect = false;
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml; charset=utf-8";

            // Construct the SOAP message for the GetAttchment operation.
            byte[] bodyBytes = System.Text.Encoding.UTF8.GetBytes(getAttachmentRequest);
            webRequest.ContentLength = bodyBytes.Length;

            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(bodyBytes, 0, bodyBytes.Length);
            requestStream.Close();

            // Make the request to the Exchange server and get the response.
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            // If the response is okay, create an XML document from the
            // response and process the request.
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = webResponse.GetResponseStream();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(responseStream);

                string fileName = xmlDocument.GetElementsByTagName("t:Name")[0].InnerText;
                byte[] bytes = Convert.FromBase64String(xmlDocument.GetElementsByTagName("t:Content")[0].InnerText);

                // Close the response stream.
                responseStream.Close();
                webResponse.Close();

                return new Attachment() { AttachmentBytes = bytes, AttachmentName = fileName };
            }

            return null;
        }
    }
}