using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.WebAPI.Processors
{
    public static class UserProcessor
    {
        public static WebApiUser ApplicationUserToWebApiUser(ApplicationUser user)
        {
            var roles = new UserManager().GetRoles(user.Id);
            //var userRoleList = user.Roles.ToList();
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            //var identityRole = roleManager.FindById(userRoleList[0].RoleId);


            return new WebApiUser
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Country = user.Country,
                City = user.City,
                PostalCode = user.PostalCode,
                Street = user.Street,
                Role = roles[0],
                TwoFactorEnabled = user.TwoFactorEnabled,
                Approved = user.Approved,
                Deleted = user.Deleted
            };
        }

        public static ApplicationUser WebApiUserToApplicationUser(WebApiUser user)
        {

            //var userRoleList = user.Roles.ToList();
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            //var identityRole = roleManager.FindById(userRoleList[0].RoleId);

            if (user.Role != null)
            {
                var roles = new UserManager().GetRoles(user.Id);
                new UserManager().RemoveFromRole(user.Id, roles[0]);
                new UserManager().AddToRole(user.Id, user.Role);
            }
            else
            {
                return null;
            }


            return new ApplicationUser
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Country = user.Country,
                City = user.City,
                PostalCode = user.PostalCode,
                Street = user.Street,

                TwoFactorEnabled = user.TwoFactorEnabled,
                Approved = user.Approved,
                Deleted = user.Deleted
            };
        }
    }
}