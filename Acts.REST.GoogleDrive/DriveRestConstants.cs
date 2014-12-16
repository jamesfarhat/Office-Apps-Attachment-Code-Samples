using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acts.REST.GoogleDrive
{
    internal class DriveRestConstants
    {

        public static string ClientIDKey = "client_id";
        public static string CodeKey = "code";
        public static string RedirectURIKey = "redirect_uri";
        public static string ClientSecretKey = "client_secret";
        public static string GrantTypeKey = "grant_type";
        public static string RefreshTokenKey = "refresh_token";


        public static string GrantTypeAuthorizationCode = "authorization_code";



        public static string AccountHostURL = "https://accounts.google.com";
        public static string FileHostURL = "https://www.googleapis.com";
        



        public static string GetCodeRequestURL = "/o/oauth2/auth";
        public static string GetTokenRequestURL = "/o/oauth2/token";
        public static string FileRequestURL = "/upload/drive/v2/files";
        

    }
}
