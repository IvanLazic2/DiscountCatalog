using DiscountCatalog.MVC.Cookies.Contractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Cookies.Implementation
{
    public class CookieHandler : ICookieHandler
    {
        public string Get(string key, HttpContext context)
        {
            return context.Request.Cookies[key].Value;
        }

        public void Set(string key, string value, bool httpOnly, HttpContext context)
        {
            context.Response.Cookies.Add(new HttpCookie(key)
            {
                Value = value,
                HttpOnly = httpOnly
            });
        }

        public void Clear(string key, HttpContext context)
        {
            context.Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);
        }

        public void ClearAll(HttpContext context)
        {
            string[] allKeys = context.Request.Cookies.AllKeys;

            foreach (string key in allKeys)
            {
                context.Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
}