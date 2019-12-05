using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.CommonModels.WebApiModels
{
    public class WebApiResult
    {
        public string Value { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class WebApiUserResult
    {
        public WebApiUser Value { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class WebApiListOfUsersResult
    {
        public List<WebApiUser> Value { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class WebApiStoreResult
    {
        public WebApiStore Value { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class WebApiListOfStoresResult
    {
        public List<WebApiStore> Value { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}