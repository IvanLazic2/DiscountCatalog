using AbatementHelper.CommonModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.CommonModels.Models
{
    public class Response
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public AuthenticatedUser User { get; set; }
        public List<AuthenticatedUser> Users { get; set; }
    }
}