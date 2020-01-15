using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.CommonModels.WebApiModels
{
    public class WebApiResult
    {
        //public string Value { get; set; }
        public Dictionary<string, string> ModelState { get; set; } = new Dictionary<string, string>();
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
    }

    public class WebApiUserResult
    {
        public WebApiUser User { get; set; }
        public Dictionary<string, string> ModelState { get; set; } = new Dictionary<string, string>();
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
    }

    public class WebApiListOfUsersResult
    {
        public List<WebApiUser> Users { get; set; }
        public Dictionary<string, string> ModelState { get; set; } = new Dictionary<string, string>();
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
    }

    public class WebApiStoreResult
    {
        public WebApiStore Store { get; set; }
        public Dictionary<string, string> ModelState { get; set; } = new Dictionary<string, string>();
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
    }

    public class WebApiListOfStoresResult
    {
        public List<WebApiStore> Stores { get; set; }
        public Dictionary<string, string> ModelState { get; set; } = new Dictionary<string, string>();
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
                    if(Exception != null)
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
    }

    public class WebApiManagerResult
    {
        public WebApiManager Manager { get; set; }
        public Dictionary<string, string> ModelState { get; set; } = new Dictionary<string, string>();
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
    }

    public class WebApiListOfManagersResult
    {
        public List<WebApiManager> Managers { get; set; }
        public Dictionary<string, string> ModelState { get; set; } = new Dictionary<string, string>();
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
    }

    public class WebApiProductResult
    {
        public WebApiProduct Product { get; set; }
        public Dictionary<string, string> ModelState { get; set; } = new Dictionary<string, string>();
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
    }

    public class WebApiListOfProductsResult
    {
        public List<WebApiProduct> Products { get; set; }
        public Dictionary<string, string> ModelState { get; set; } = new Dictionary<string, string>();
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
    }
}