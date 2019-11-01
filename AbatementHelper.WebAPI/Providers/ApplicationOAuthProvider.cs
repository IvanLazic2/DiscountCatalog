using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using AbatementHelper.WebAPI.Models;

namespace AbatementHelper.WebAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            var storeManager = context.OwinContext.GetUserManager<ApplicationStoreManager>();

            ApplicationStore store = await storeManager.FindAsync(context.UserName, context.Password);

            if (user != null)
            {
                ClaimsIdentity oAuthIdentityUser = await user.GenerateUserIdentityAsync(userManager,
                               OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookiesIdentityUser = await user.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                List<Claim> rolesUser = oAuthIdentityUser.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                AuthenticationProperties propertiesUser = CreateProperties(user.Id, user.UserName, user.Email, user.Role);

                AuthenticationTicket userTicket = new AuthenticationTicket(oAuthIdentityUser, propertiesUser);
                context.Validated(userTicket);
                context.Request.Context.Authentication.SignIn(cookiesIdentityUser);
            }
            else if (store != null)
            {
                ClaimsIdentity oAuthIdentityStore = await store.GenerateUserIdentityAsync(storeManager,
               OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookiesIdentityStore = await store.GenerateUserIdentityAsync(storeManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                List<Claim> rolesStore = oAuthIdentityStore.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
                AuthenticationProperties propertiesStore = CreateProperties(store.Id, store.UserName, store.Email, store.Role);

                AuthenticationTicket storeTicket = new AuthenticationTicket(oAuthIdentityStore, propertiesStore);
                context.Validated(storeTicket);
                context.Request.Context.Authentication.SignIn(cookiesIdentityStore);
            }
            //if (user == null)
            //{
            //    context.SetError("invalid_grant", "The user name or password is incorrect.");
            //    return;
            //}



            //


            //if (store == null)
            //{
            //    context.SetError("invalid_grant", "The user name or password is incorrectt.");
            //    return;
            //}


            //AuthenticationProperties properties = CreateProperties(user.UserName, user.Role);
            

            
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string id, string userName, string email, string role)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "Id", id },
                { "userName", userName },
                { "Email", email },
                { "Role", role }
            };
            return new AuthenticationProperties(data);
        }
    }
}