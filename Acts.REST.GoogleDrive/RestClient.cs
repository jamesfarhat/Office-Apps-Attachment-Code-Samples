using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Web.Script.Serialization;

namespace Acts.REST.GoogleDrive
{
    public class DriveRestClient
    {
        public string ClientID { get; private set; }
        public string ClientSecret { get; private set; }
        public string RedirectURL { get; private set; }

        public DriveToken Token { get; set; }

        private RestClient restClient = null;
        private RestRequest restRequest = null;
        public DriveRestClient(string clientID, string clientSecret, string redirectURL)
        {
            ClientID = clientID;
            ClientSecret = clientSecret;
            RedirectURL = redirectURL;
        }

        // This is utility function used to set Request Header and Request Parameters.
        private void WarmupRequest(RestSharp.Method currentMethod,string host,string requestURL
            , Dictionary<string,string> parameters, Dictionary<string,string> header)
        {
            restClient = new RestClient(host);

            restRequest = new RestRequest(requestURL, currentMethod);
            if(parameters!=null)
            foreach (string paramKey in parameters.Keys)
            {
                restRequest.AddParameter(paramKey,parameters[paramKey]);
            }

            if(header!= null)
            foreach (string headerKey in header.Keys)
            {
                restRequest.AddParameter(headerKey, parameters[headerKey]);
            }
        }

        // Step 1st: This method gets the redirect URL for Google Drive to authenticate app.
        public string GetPermissionURL()
        {
            StringBuilder returnURL = new StringBuilder();
            returnURL.AppendFormat(
                "{0}{1}?{2}={3}&response_type=code&{4}={5}&scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fdrive&approval_prompt=force&access_type=offline",
                DriveRestConstants.AccountHostURL,DriveRestConstants.GetCodeRequestURL,
            DriveRestConstants.RedirectURIKey,RedirectURL,DriveRestConstants.ClientIDKey,ClientID);
            

            

            return returnURL.ToString();
        }

        // Step 2nd: This is second step to get the Token when control is handed over from dropbox to our app.
        public DriveToken GetTokenfromCode(string code)
        {
            Dictionary<string,string> parameters = new Dictionary<string,string>();

             parameters.Add(DriveRestConstants.CodeKey, code);

                parameters.Add(DriveRestConstants.RedirectURIKey, RedirectURL);
                parameters.Add(DriveRestConstants.ClientIDKey, ClientID);
                parameters.Add(DriveRestConstants.ClientSecretKey, ClientSecret);
                parameters.Add(DriveRestConstants.GrantTypeKey, DriveRestConstants.GrantTypeAuthorizationCode);

            WarmupRequest(Method.POST,DriveRestConstants.AccountHostURL,DriveRestConstants.GetTokenRequestURL
                ,parameters,null);

            IRestResponse response = restClient.Execute(restRequest);
            var content = response.Content; // raw content as string

            //JavaScriptSerializer ser = new JavaScriptSerializer();
            DriveToken token = GetTokenFromContent(content); //(DriveToken)ser.DeserializeObject(content);
            Token = token;
            return token;
        }

        // Step 3: Optional: This step is used to get refresh token if token expires.
        public DriveToken GetRefreshToken()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add(DriveRestConstants.ClientSecretKey, ClientSecret);
            parameters.Add(DriveRestConstants.GrantTypeKey, DriveRestConstants.RefreshTokenKey);
            parameters.Add(DriveRestConstants.RefreshTokenKey, Token.RefreshToken);
            parameters.Add(DriveRestConstants.ClientIDKey, ClientID);

            //parameters.Add(DriveRestConstants.RedirectURIKey, RedirectURL);
            
            
            

            WarmupRequest(Method.POST, DriveRestConstants.AccountHostURL, DriveRestConstants.GetTokenRequestURL
                , parameters, null);

            IRestResponse response = restClient.Execute(restRequest);
            var content = response.Content; // raw content as string

            //JavaScriptSerializer ser = new JavaScriptSerializer();
            DriveToken token = GetTokenFromContent(content);//(DriveToken)ser.DeserializeObject(content);
            Token = token;
            return token;
        }

        // Step 4: this method is usd to upload file.
        public string UploadFile(string fileName, byte[] fileBytes)
        {
            
            
            //parameters.Add(DriveRestConstants.RedirectURIKey, RedirectURL);




            WarmupRequest(Method.POST, DriveRestConstants.FileHostURL, DriveRestConstants.FileRequestURL
                , null, null);
            restRequest.AddHeader("Authorization", "Bearer " + Token.AccessToken);

            restRequest.AddFile(fileName, fileBytes, fileName);

            IRestResponse response = restClient.Execute(restRequest);
            var content = response.Content; // raw content as string

            

            return content;
        }


        // This is utility method used to transform Google Drive token to our own object.
        private DriveToken GetTokenFromContent(string content)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var tokenArray = ser.Deserialize<Dictionary<string, string>>(content);
            DriveToken token = new DriveToken();

            foreach (var tokenKey in tokenArray.Keys)
            {
                switch (tokenKey)
                {
                    case "access_token":
                        token.AccessToken = tokenArray[tokenKey];
                        break;
                    case "token_type":
                        token.TokenType = tokenArray[tokenKey];
                        break;
                    case "expires_in":
                        token.ExpiresIn = tokenArray[tokenKey];
                        break;
                    case "refresh_token":
                        token.RefreshToken = tokenArray[tokenKey];
                        break;
                }
 
            }


            return token;
        }

        
    }
}
