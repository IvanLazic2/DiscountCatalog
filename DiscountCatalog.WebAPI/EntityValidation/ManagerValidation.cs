//using DiscountCatalog.WebAPI.Extentions;
//using DiscountCatalog.WebAPI.Interfaces;
//using DiscountCatalog.WebAPI.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;

//namespace DiscountCatalog.WebAPI.EntityValidation
//{
//    public class ManagerValidation : IManagerValidation
//    {
//        public async Task<ModelStateResponse> ValidateAsync(ApplicationUser manager)
//        {
//            var modelState = new ModelStateResponse();

//            using (var userManager = new UserManager())
//            {
//                ApplicationUser existingUser = await userManager.FindByEmailAsync(manager.Email);

//                if (existingUser != null)
//                {
//                    modelState.ModelState.Add(ObjectExtensions.GetPropertyName(() => manager.Email), $"Email is already taken.");
//                }
//                else
//                {
//                    existingUser = await userManager.FindByNameAsync(manager.UserName);

//                    if (existingUser != null)
//                    {
//                        modelState.ModelState.Add(ObjectExtensions.GetPropertyName(() => manager.UserName), $"Username is already taken.");
//                    }
//                }
//            }

//            return modelState;
//        }
//    }
//}