using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttachmentsDemoWeb.Constants
{
    // Used for Exchange attachment only
    public class AttachmentRequest
    {
        public string AuthToken { get; set; }
        public string AttachmentId { get; set; }
        public string EwsUrl { get; set; }
    }
}