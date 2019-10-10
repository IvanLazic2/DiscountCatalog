using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.MVC.Models
{
    public class UserForAuthentication
    {
        private string Email { get; set; }
        private string Password { get; set; }

        public UserForAuthentication(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}