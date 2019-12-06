using AbatementHelper.CommonModels.Models;
using AbatementHelper.CommonModels.WebApiModels;
using AbatementHelper.WebAPI.DataBaseModels;
using AbatementHelper.WebAPI.Models;
using AbatementHelper.WebAPI.Processors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AbatementHelper.WebAPI.Repositories
{
    public class DataBaseEntityRepository
    {


        public async Task<string> ReturnUserName(UserManager userManager, string usernameOrEmail)
        {
            var username = usernameOrEmail;
            if (usernameOrEmail.Contains("@"))
            {
                var userForEmail = await userManager.FindByEmailAsync(usernameOrEmail);
                if (userForEmail != null)
                {
                    username = userForEmail.UserName;
                }
            }
            //return await userManager.FindAsync(username, password);
            return username;
        }

        public ApplicationUser ReadUserById(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                var user = (from u in context.Users
                            where u.Id == id
                            select u).FirstOrDefault();

                return user;
            }
        }

        public static WebApiResult ReadEmail(string email)
        {

            using (var context = new ApplicationUserDbContext())
            {
                var user = (from u in context.Users where u.Email == email select u).FirstOrDefault();

                return new WebApiResult()
                {
                    Value = user.Email,
                    Message = "Query successfull",
                    Success = true
                };
            }
        }

        public static WebApiResult ReadUserName(string email)
        {
            using (var context = new ApplicationUserDbContext())
            {
                var user = (from u in context.Users where u.Email == email select u).FirstOrDefault();

                return new WebApiResult()
                {
                    Value = user.UserName,
                    Message = "Query successfull",
                    Success = true
                };
            }
        }

        //admin

        public List<ApplicationUser> ReadAllUsers()
        {
            return new UserManager().Users.ToList();
        }

        public Response EditUser(WebApiUser user)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser userEntity = context.Users.Find(user.Id);
                    context.Users.Attach(userEntity);
                    userEntity.UserName = user.UserName;
                    userEntity.FirstName = user.FirstName;
                    userEntity.LastName = user.LastName;
                    userEntity.Email = user.Email;
                    userEntity.EmailConfirmed = user.EmailConfirmed;
                    userEntity.PhoneNumber = user.PhoneNumber;
                    userEntity.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                    userEntity.Country = user.Country;
                    userEntity.City = user.City;
                    userEntity.PostalCode = user.PostalCode;
                    userEntity.Street = user.Street;
                    userEntity.TwoFactorEnabled = user.TwoFactorEnabled;
                    userEntity.Approved = user.Approved;
                    userEntity.Deleted = user.Deleted;

                    if (user.Role != null)
                    {
                        var roles = new UserManager().GetRoles(user.Id);
                        if (roles[0] != user.Role)
                        {
                            new UserManager().RemoveFromRole(user.Id, roles[0]);
                            new UserManager().AddToRole(user.Id, user.Role);
                        }
                    }

                    context.SaveChanges();

                    response.ResponseMessage = "Edited successfully";
                    response.Success = true;
                }
            }
            catch (DbUpdateException)
            {
                response.ResponseMessage = $"User {user.UserName} already exsists";
                response.Success = false;
            }

            return response;
        }

        public void DeleteUser(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = context.Users.Find(id);
                context.Users.Attach(user);
                user.Deleted = true;
                context.SaveChanges();
            }
        }

        public void RestoreUser(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                ApplicationUser user = context.Users.Find(id);
                context.Users.Attach(user);
                user.Deleted = false;
                context.SaveChanges();
            }
        }

        //User

        public Response Edit(WebApiUser user)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    ApplicationUser userEntity = context.Users.Find(user.Id);
                    context.Users.Attach(userEntity);

                    userEntity.UserName = user.UserName;
                    userEntity.FirstName = user.FirstName;
                    userEntity.LastName = user.LastName;
                    userEntity.Email = user.Email;
                    userEntity.PhoneNumber = user.PhoneNumber;
                    userEntity.Country = user.Country;
                    userEntity.City = user.City;
                    userEntity.PostalCode = user.PostalCode;
                    userEntity.Street = user.Street;
                    userEntity.TwoFactorEnabled = user.TwoFactorEnabled;

                    context.SaveChanges();

                    response.ResponseMessage = "Successfully edited";
                    response.Success = true;
                }
            }
            catch (DbUpdateException)
            {
                response.ResponseMessage = $"UserName {user.UserName} already exists";
                response.Success = false;
            }

            return response;

        }

        //StoreAdmin

        public async Task<StoreEntity> ReadStoreById(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                var store = await context.Stores.FindAsync(id);


                return store;
            }
        }

        public List<WebApiStore> GetAllStores(string storeAdminId)
        {
            using (var context = new ApplicationUserDbContext())
            {
                List<WebApiStore> stores = new List<WebApiStore>();

                foreach (var store in context.Stores.Where(s => s.StoreAdmin.Id == storeAdminId).ToList())
                {
                    store.StoreAdmin = context.Users.Find(storeAdminId);

                    stores.Add(StoreProcessor.StoreEntityToWebApiStore(store)); // tu je nest null

                }

                return stores;
            }
        }

        public Response CreateStore(WebApiStore store)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {

                    StoreEntity processedStore = StoreProcessor.WebApiStoreToStoreEntity(store);
                    processedStore.StoreAdmin = context.Users.Find(store.StoreAdminId);
                    context.Stores.Add(processedStore);

                    

                    context.SaveChanges();

                    response.ResponseMessage = "Successfully created";
                    response.Success = true;

                    //try
                    //{
                    //    context.SaveChanges();
                    //}
                    //catch (DbEntityValidationException ex)
                    //{
                    //    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    //    {
                    //        // Get entry

                    //        DbEntityEntry entry = item.Entry;
                    //        string entityTypeName = entry.Entity.GetType().Name;

                    //        // Display or log error messages

                    //        foreach (DbValidationError subItem in item.ValidationErrors)
                    //        {
                    //            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                    //                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                    //            Debug.WriteLine(message);
                    //        }
                    //    }
                    //}


                }
            }
            catch (Exception)
            {
                response.ResponseMessage = $"Store {store.StoreName} already exists";
                response.Success = false;
            }



            return response;

        }

        public Response EditStore(WebApiStore store)
        {
            Response response = new Response();

            try
            {
                using (var context = new ApplicationUserDbContext())
                {
                    StoreEntity storeEntity = context.Stores.Find(store.Id);
                    context.Stores.Attach(storeEntity);

                    storeEntity.StoreName = store.StoreName;
                    storeEntity.WorkingHoursWeek = store.WorkingHoursWeek;
                    storeEntity.WorkingHoursWeekends = store.WorkingHoursWeekends;
                    storeEntity.WorkingHoursHolidays = store.WorkingHoursHolidays;
                    storeEntity.Country = store.Country;
                    storeEntity.City = store.City;
                    storeEntity.PostalCode = store.PostalCode;
                    storeEntity.Street = store.Street;

                    context.SaveChanges();

                    response.ResponseMessage = "Successfully edited";
                    response.Success = true;
                }
            }
            catch (DbUpdateException)
            {
                response.ResponseMessage = $"Store {store.StoreName} already exists";
                response.Success = false;
            }

            return response;
        }

        public void DeleteStore(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = context.Stores.Find(id);
                context.Stores.Attach(store);
                store.Deleted = true;
                context.SaveChanges();
            }
        }

        public List<WebApiStore> GetAllDeletedStores(string storeAdminId)
        {
            using (var context = new ApplicationUserDbContext())
            {
                List<WebApiStore> stores = new List<WebApiStore>();

                foreach (var store in context.Stores.Where(s => s.StoreAdmin.Id == storeAdminId && s.Deleted).ToList())
                {
                    store.StoreAdmin = context.Users.Find(storeAdminId);

                    stores.Add(StoreProcessor.StoreEntityToWebApiStore(store));

                }

                return stores;
            }
        }

        public void RestoreStore(string id)
        {
            using (var context = new ApplicationUserDbContext())
            {
                StoreEntity store = context.Stores.Find(id);
                context.Stores.Attach(store);
                store.Deleted = false;
                context.SaveChanges();
            }
        }
    }


}