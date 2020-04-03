using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiscountCatalog.MVC.Extensions
{
    internal static class RedirectMessageExtensions
    {
        public static void SetError(string message)
        {
            CreateCookieWithFlashMessage(Notification.Error, message);
        }

        public static void SetWarning(string message)
        {
            CreateCookieWithFlashMessage(Notification.Warning, message);
        }

        public static void SetSuccess(string message)
        {
            CreateCookieWithFlashMessage(Notification.Success, message);
        }

        public static void SetInformation(string message)
        {
            CreateCookieWithFlashMessage(Notification.Info, message);
        }

        private static void CreateCookieWithFlashMessage(Notification notification, string message)
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(string.Format("Flash.{0}", notification), message) { Path = "/" });
        }

        private enum Notification
        {
            Error,
            Warning,
            Success,
            Info
        }
    }
}