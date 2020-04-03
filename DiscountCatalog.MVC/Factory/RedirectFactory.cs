using DiscountCatalog.MVC.Cookies.Contractor;
using DiscountCatalog.MVC.Cookies.Implementation;
using DiscountCatalog.MVC.Extensions;
using DiscountCatalog.MVC.Models.Redirect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.MVC.Factory
{
    public class RedirectFactory
    {
        private readonly ICookieHandler cookieHandler;
        private readonly AccountCookieHandler accountCookieHandler;
        private readonly StoreCookieHandler storeCookieHandler;
        private readonly HttpContext CurrentContext;
        private readonly HttpContextBase ContextBase;
        private readonly List<string> AllowedActions;
        private readonly List<string> AllowedControllers;

        public RedirectFactory(HttpContext currentContext, HttpContextBase contextBase, List<string> allowedControllers, List<string> allowedActions)
        {
            cookieHandler = new CookieHandler();
            accountCookieHandler = new AccountCookieHandler();
            storeCookieHandler = new StoreCookieHandler();
            CurrentContext = currentContext;
            ContextBase = contextBase;
            AllowedActions = allowedActions;
            AllowedControllers = allowedControllers;
        }

        private bool AreAccountCookiesValid()
        {
            return accountCookieHandler.IsValid(accountCookieHandler.Get(CurrentContext));
        }

        private bool AreStoreCookiesValid()
        {
            return storeCookieHandler.IsValid(storeCookieHandler.Get(CurrentContext));
        }

        public void GenerateRedirect(RedirectModel model)
        {
            if (!AreAccountCookiesValid())
            {
                if (!AllowedControllers.Contains(model.ControllerName))
                {
                    if (!AllowedActions.Contains(model.ActionName))
                    {
                        cookieHandler.ClearAll(CurrentContext);
                        RedirectMessageExtensions.SetError("Something went wrong, please log in.");
                        ContextBase.Response.Redirect("/Account/Login");
                    }
                }
            }

            if (model.ControllerName == "Store")
            {
                if (!AreStoreCookiesValid())
                {
                    RedirectMessageExtensions.SetError("Something went wrong, please select the store again.");
                    ContextBase.Response.Redirect($"/{cookieHandler.Get("Role", CurrentContext)}/GetAllStores"); //zasad
                }
            }
        }
    }
}