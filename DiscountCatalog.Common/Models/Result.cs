using DiscountCatalog.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.Common.Models
{
    public class Result //dodat responsecode i dodat redirectprocessor il tak nest koji ce primat result i vracat string path
    {
        public Result()
        {

        }

        public Result(string key, string value)
        {
            AddModelError(key, value);
        }

        public Result(string value)
        {
            Add(value);
        }

        public List<KeyValuePair<string, string>> ModelState { get; set; } = new List<KeyValuePair<string, string>>();
        public string SuccessMessage { get; set; }
        public bool Success
        {
            get
            {
                if (ModelState.Count > 0)
                {
                    return false;
                }
                else
                {
                    if (Exception != null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            private set { }
        }
        public Exception Exception { get; set; }

        public static Result GetQuickResult(string key, string value)
        {
            return new Result(key, value);
        }

        public static Result GetQuickResult(string value)
        {
            return new Result(value);
        }

        public void AddModelError(string key, string value)
        {
            var element = new KeyValuePair<string, string>(key, value);
            ModelState.Add(element);
        }

        public void Add(string value)
        {
            var element = new KeyValuePair<string, string>(string.Empty, value);
            ModelState.Add(element);
        }
    }
}