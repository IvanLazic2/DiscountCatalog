using AbatementHelper.CommonModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public class UserProcessor
    {
        public static DataBaseUser ProcessUser(DataBaseUser user, string requestRole)
        {
            if (requestRole == "Admin")
            {
                return user;
            }
            else if (requestRole == "User")
            {

                return user;
            }
            else if (requestRole == "Store")
            {
                return user;
            }
            else
            {
                return user = null;
            }
        }
    }
}