﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Job.Common;

namespace JobBroad.Models
{
    public class InstanceResult<T>
    {
       
            public Result<List<T>> resultList { get; set; }
            public Result<int> resultint { get; set; }
            public Result<T> TResult { get; set; }
        
    }
}