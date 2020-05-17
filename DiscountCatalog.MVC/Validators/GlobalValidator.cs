using DiscountCatalog.MVC.REST.Account;
using DiscountCatalog.MVC.REST.Manager;
using DiscountCatalog.MVC.REST.Product;
using DiscountCatalog.MVC.REST.Store;
using DiscountCatalog.MVC.REST.StoreAdmin;
using DiscountCatalog.MVC.Validators.AbstractValidators;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Validators
{
    public class GlobalValidator
    {
        public static bool IsAccountValid(AccountREST account)
        {
            if (account != null)
            {
                AccountValidator validator = new AccountValidator();
                ValidationResult result = validator.Validate(account);

                return result.IsValid;
            }

            return false;
        }

        public static bool IsManagerValid(ManagerREST manager)
        {
            if (manager != null)
            {
                if (manager.Identity != null)
                {
                    AccountValidator validator = new AccountValidator();
                    ValidationResult result = validator.Validate(manager.Identity);

                    return result.IsValid;
                }
            }

            return false;
        }

        public static bool IsProductValid(ProductREST product)
        {
            if (product != null)
            {
                ProductValidator validator = new ProductValidator();
                ValidationResult result = validator.Validate(product);

                return result.IsValid;
            }

            return false;
        }

        public static bool IsStoreValid(StoreREST store)
        {
            if (store != null)
            {
                StoreValidator validator = new StoreValidator();
                ValidationResult result = validator.Validate(store);

                return result.IsValid;
            }

            return false;
        }

        public static bool IsStoreAdminValid(StoreAdminREST storeAdmin)
        {
            StoreAdminValidator validator = new StoreAdminValidator();
            ValidationResult result = validator.Validate(storeAdmin);

            return result.IsValid;
        }
    }
}