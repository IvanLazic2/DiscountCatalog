using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Models.Cookies
{
    public class StoreCookie
    {
        public string StoreID { get; set; }
        public string StoreName { get; set; }

        public StoreCookie(string storeId, string storeName)
        {
            StoreID = storeId;
            StoreName = storeName;
        }

        public StoreCookie()
        {

        }
    }
}