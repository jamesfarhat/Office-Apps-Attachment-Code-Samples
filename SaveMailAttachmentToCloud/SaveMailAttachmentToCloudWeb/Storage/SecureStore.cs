using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttachmentsDemoWeb.Storage
{
    public class OneDrive
    {
        public static string RefreshToken { get; set; }
    }
    public class Dropbox
    {
        public static DropNet.Models.UserLogin Token { get; set; }
        
    }

    public class GoogleDrive
    {
        public static Acts.REST.GoogleDrive.DriveToken Token { get; set; }

    }
}