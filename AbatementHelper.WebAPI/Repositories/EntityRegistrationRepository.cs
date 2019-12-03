using AbatementHelper.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Data.Entity;
using AbatementHelper.WebAPI.DataBaseModels;

namespace AbatementHelper.WebAPI.Repositories
{
    public static class EntityRegistrationRepository
    {
        //public static ApplicationUserDbContext context = new ApplicationUserDbContext();

        //public static void AddUserInfo(UserInfoEntity userInfo)
        //{
        //    try
        //    {
        //        context.UserInfo.Add(userInfo);               
        //    }
        //    finally
        //    {
        //        context.SaveChanges();
        //    }
        //}

        //public static void AddStoreInfo(ManagerInfoEntity storeInfo)
        //{
        //    try
        //    {
        //        context.ManagerInfo.Add(storeInfo);
        //        //var errors = context.GetValidationErrors();
        //    }
        //    finally
        //    {
        //        context.SaveChanges();
        //    }
        //}

        //public static void AddStoreAdminInfo(StoreAdminInfoEntity storeAdminInfo)
        //{
        //    try
        //    {
        //        context.StoreAdminInfo.Add(storeAdminInfo);
        //    }
        //    finally
        //    {
        //        context.SaveChanges();
        //    }
        //}

        //public static void AddAdminInfo(AdminInfoEntity adminInfo)
        //{
        //    try
        //    {
        //        context.AdminInfo.Add(adminInfo);
        //    }
        //    finally
        //    {
        //        context.SaveChanges();
        //    }
        //}

        //public static bool FindEmailDuplicates(string email)
        //{
        //    using (var context = new ApplicationUserDbContext())
        //    {
        //        var list = context.Users.Where(u => u.Email == email).ToList();
        //        if (list.Count < 1 && list.Count > -1)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //}
    }
}