using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Job.Entity;

namespace JobBroad.Models
{
    public class JobViewModel
    {
        public JobO Job { get; set; }       
        public IEnumerable<SelectListItem> LocationList { get; set; }
        public Company Company { get; set; }
        public C_JobSeeker jobSeeker { get; set; }
        public User User { get; set; }
        public C_JobSeeker JobSeeker { get; set; }
        public Seeker Seeker { get; set; }
        public IEnumerable<C_JobSeeker> JobSeekerList { get; set; }
        public IEnumerable<JobO> JobList { get; set; }

    }
}