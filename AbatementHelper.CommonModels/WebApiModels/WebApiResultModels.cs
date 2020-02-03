using AbatementHelper.CommonModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.CommonModels.WebApiModels
{
    public class WebApiResult
    {
        public List<KeyValuePair<string, string>> ModelState { get; set; } = new List<KeyValuePair<string, string>>();
        public string Message { get; set; }
        public bool Success
        {
            get
            {
                if (ModelState.Count > 0)
                {
                    return false;
                }
                else
                {
                    if (Exception != null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            private set
            {
            }
        }
        public Exception Exception { get; set; }

        public void AddModelError(string key, string value)
        {
            var element = new KeyValuePair<string, string>(key, value);
            ModelState.Add(element);
        }
    }

    public class WebApiUserResult : WebApiResult
    {
        public WebApiUser User { get; set; }
    }

    public class WebApiListOfUsersResult : WebApiResult
    {
        public List<WebApiUser> Users { get; set; }
    }

    public class WebApiStoreResult : WebApiResult
    {
        public WebApiStore Store { get; set; }
    }

    public class WebApiListOfStoresResult : WebApiResult
    {
        public List<WebApiStore> Stores { get; set; }
    }

    public class WebApiManagerResult : WebApiResult
    {
        public WebApiManager Manager { get; set; }
    }

    public class WebApiListOfManagersResult : WebApiResult
    {
        public List<WebApiManager> Managers { get; set; }
    }

    public class WebApiProductResult : WebApiResult
    {
        public WebApiProduct Product { get; set; }
    }

    public class WebApiListOfProductsResult : WebApiResult
    {
        public List<WebApiProduct> Products { get; set; }
    }

    public class WebApiSelectedStoreResult : WebApiResult
    {
        public SelectedStore Store { get; set; }
    }

    public class WebApiManagerStoreResult : WebApiResult
    {
        public WebApiManagerStore Store { get; set; }
    }

    public class WebApiListOfManagerStoresResult : WebApiResult
    {
        public List<WebApiManagerStore> Stores { get; set; }
    }

    public class WebApiAuthenticatedUserResult : WebApiResult
    {
        public AuthenticatedUser User { get; set; }
    }
}