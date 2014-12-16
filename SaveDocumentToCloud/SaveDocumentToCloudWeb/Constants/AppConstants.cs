using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Configuration;

namespace AttachmentsDemoWeb.Constants
{
    public class AppConstants
    {

        #region Client ID and Secrets

        public static  string DropboxClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["DropboxClientId"];
            }
        }

        public static  string DropboxClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["DropboxClientSecret"];
            }
        }
        public static  string GoogleDriveClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["GoogleDriveClientId"];
            }
        }
        public static  string GoogleDriveClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["GoogleDriveClientSecret"];
            }
        }
        public static  string OneDriveClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["OneDriveClientId"];
            }
        }
        public static  string OneDriveClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["OneDriveClientSecret"];
            }
        }
        #endregion

        #region RedirectURL
        public static string DropboxReturnURL
        {
            get
            {
                return ConfigurationManager.AppSettings["DropboxReturnURL"];
            }
        }
        public static string OneDriveURL
        {
            get
            {
                return ConfigurationManager.AppSettings["OneDriveURL"];
            }
        }
        #endregion

        public static string oneDriveResourceId
        {
            get
            {
                return ConfigurationManager.AppSettings["oneDriveResourceId"];
            }
        }
        public static string oneDriveApiEndpoint
        {
            get
            {
                return ConfigurationManager.AppSettings["oneDriveApiEndpoint"];
            }
        }

    }
}