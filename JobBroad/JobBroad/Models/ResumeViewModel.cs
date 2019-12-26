using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Job.Entity;


namespace JobBroad.Models
{
    public class ResumeViewModel
    {
        public Seeker Seeker { get; set; }
        public Experience Experience { get; set; }
        public Education Education { get; set; }
        public IEnumerable<SelectListItem> LocationList { get; set; }
        public List<Education> EduList { get; set; }
        public List<Experience> WorkList { get; set; }
        public CV cv { get; set; }
    }
}