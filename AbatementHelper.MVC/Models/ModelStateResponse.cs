using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.MVC.Models
{
    public class ModelStateResponse
    {
        public string Message { get; set; } = "";
        public Dictionary<string, string[]> ModelSate { get; set; } = new Dictionary<string, string[]>();
        public bool IsValid
        {
            get
            {
                if (ModelSate.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            private set
            {

            }
        }

        public ModelStateResponse()
        {
        }
    }
}