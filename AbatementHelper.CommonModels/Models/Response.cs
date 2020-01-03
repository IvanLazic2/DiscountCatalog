﻿using AbatementHelper.CommonModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbatementHelper.CommonModels.Models
{
    public class Response
    {
        public int ResponseCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public AuthenticatedUser User { get; set; }
    }
}