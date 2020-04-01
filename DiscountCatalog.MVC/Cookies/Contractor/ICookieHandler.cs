using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DiscountCatalog.MVC.Cookies.Contractor
{
    public interface ICookieHandler<TCookie> where TCookie : class
    {
        TCookie Get(HttpContext context);
        bool IsValid(TCookie cookie);
    }

    public interface ICookieHandler
    {
        string Get(string key, HttpContext context);
        void Set(string key, string value, bool httpOnly, HttpContext context);
        void Clear(string key, HttpContext context);
        void ClearAll(HttpContext context);
    }
}
