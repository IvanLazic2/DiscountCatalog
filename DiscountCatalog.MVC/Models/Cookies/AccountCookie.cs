using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Models.Cookies
{
    public class AccountCookie
    {
        public string Id { get; set; }
        public string Access_Token { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public AccountCookie()
        {

        }

        public AccountCookie(string id, string access_token, string userName, string email, string role)
        {
            Id = id;
            Access_Token = access_token;
            UserName = userName;
            Email = email;
            Role = role;
        }
    }
}