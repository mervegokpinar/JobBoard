using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Job.Entity;
using Job.Common;
using System.Web.Mvc;

namespace JobBroad.Models
{
    public class SearchModel
    {
        public Location Location { get; set; }
        public string keyword { get; set; }
        public List<Location> LocationList { get; set; }
        public List<JobO> findings { get; set; }
    }
}