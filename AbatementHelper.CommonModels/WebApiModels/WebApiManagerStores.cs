﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbatementHelper.CommonModels.WebApiModels
{
    public class WebApiManagerStore
    {
        public WebApiStore Store { get; set; }
        public WebApiManager Manager { get; set; }
        public bool Assigned { get; set; }
    }
}
