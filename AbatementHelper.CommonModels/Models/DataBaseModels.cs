using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.CommonModels.Models
{
    public class DataBaseResult
    {
        public string Value { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class DataBaseResultUser
    {
        public DataBaseUser Value { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class DataBaseResultListOfUsers
    {
        public List<DataBaseUser> Value { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}