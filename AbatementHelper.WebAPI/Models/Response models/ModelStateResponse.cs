using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.WebAPI.Models
{
    public class ModelStateResponse
    {
        public Dictionary<string, string> ModelState { get; set; } = new Dictionary<string, string>();
        public bool IsValid
        {
            get
            {
                if (ModelState.Count > 0)
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
        public Exception Exception { get; set; }
        public string Message { get; set; }
    }
}
