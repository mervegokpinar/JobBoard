using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Job.Entity;
using Job.Common;
using Job.Repository;
using JobBroad.Models;
using JobBroad.Models.VM;


namespace JobBroad.Controllers
{
    public class HomeController : Controller
    {
        #region Connections
        private static CAREEREntities db = Tools.GetConnection();            
        #endregion
        #region Index
        public ActionResult Index()
        {
            Job0Repository jr = new Job0Repository();
            var i = 0;
            foreach (JobO item in jr.List().ProcessResult)
            {
                i++;
            }
            ViewBag.TotalJob = i;

            JobViewModel jwm = new JobViewModel();
            
            return View();
        }
        #endregion
        #region Register

        public ActionResult Register()
        { 
            return View();
        }

        [HttpPost]
        public ActionResult Register(User u)
        {
            InstanceResult<User> resultt = new InstanceResult<User>();
            UserRepository ur = new UserRepository();
            CompanyRepository cr = new CompanyRepository();
            SeekerRepository sr = new SeekerRepository();

            resultt.resultint = ur.Insert(u);
            Session["User"] = ur.GetLatestObj(1).ProcessResult[0];
            if (resultt.resultint.IsSucceeded)
            {
                if (((User)Session["User"]).userRoleId==1)
                {
                    Seeker s = new Seeker();
                    s.empUserId = ((User)Session["User"]).userId;
                    s.empName= ((User)Session["User"]).userName;
                    s.empSurname= ((User)Session["User"]).userSurname;
                    sr.Insert(s);
                }
                if (((User)Session["User"]).userRoleId == 2)
                {
                    Company c = new Company();
                    c.CompUserId= ((User)Session["User"]).userId;
                    c.CompName = ((User)Session["User"]).userName;
                    cr.Insert(c);
                }

                ViewBag.SignIn = "Your account has been sucessfully created ! Please Sign In.";
                return View("Login");
            }
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.SignIn = "Please try again !";
            return View();
        }
        #endregion
        #region Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User model)
        {
            var user = db.Users.FirstOrDefault(x => x.userName == model.userName && x.userPassword == model.userPassword);
            if (user != null)
            {
                string cookieName = "UserLogin";
                string cookieValue = user.userName.ToUpper();
                HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
                cookie.Expires = DateTime.Now.AddMonths(1);
                HttpContext.Response.Cookies.Add(cookie);
                ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;

                string cookieId = "UserId";
                var cookieValueId = user.userId;
                HttpCookie cookie2 = new HttpCookie(cookieId, Convert.ToString(cookieValueId));
                cookie2.Expires = DateTime.Now.AddMonths(1);
                HttpContext.Response.Cookies.Add(cookie2);
                ViewBag.UserId = Request.Cookies["UserId"].Value;

                string cookieRole = "UserRole";
                var cookieRoleId = user.userRoleId;
                HttpCookie cookie3 = new HttpCookie(cookieRole, Convert.ToString(cookieRoleId));
                cookie3.Expires = DateTime.Now.AddMonths(1);
                HttpContext.Response.Cookies.Add(cookie3);
                ViewBag.UserRole = Request.Cookies["UserRole"].Value;

                return View("Index");
            }

            ViewBag.Unmatched = "Incorrect username or password";
            return RedirectToAction("Index", "Home");
        }

        #endregion
        #region Listing All Job
        public ActionResult ListAllJob()
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            Job0Repository jr = new Job0Repository();
            InstanceResult<JobO> result = new InstanceResult<JobO>();
            result.resultList = jr.List();
            return View(result.resultList.ProcessResult);
        }
        #endregion
        #region Resume Adding

        public ActionResult ResumeAdd()
        {
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ResumeViewModel rwm = new ResumeViewModel();

            rwm.cv = null;
            return View(rwm);
        }

        [HttpPost]
        public ActionResult ResumeAdd(ResumeViewModel model)
        {
            InstanceResult<JobO> resultJ = new InstanceResult<JobO>();
            InstanceResult<Seeker> resultS = new InstanceResult<Seeker>();
            InstanceResult<CV> resultttt = new InstanceResult<CV>();
            InstanceResult<Education> resulttt = new InstanceResult<Education>();
            InstanceResult<Experience> result = new InstanceResult<Experience>();

            SeekerRepository sr = new SeekerRepository();
            CVRepository cr = new CVRepository();
            ExperienceRepository er = new ExperienceRepository();
            EducationRepository edr = new EducationRepository();
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            resultS.resultint = sr.Insert(model.Seeker);
            resultttt.resultint = cr.Insert(model.cv);
            resulttt.resultint = edr.Insert(model.Education);
            result.resultint = er.Insert(model.Experience);

            return View(model);
        }

        #endregion
        #region Job Adding

        public ActionResult AddJob()
        {
            LocationRepository lr = new LocationRepository();
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;
            JobViewModel jwm = new JobViewModel();
            List<SelectListItem> LocationList = new List<SelectListItem>();

            foreach (Location item in lr.List().ProcessResult)
            {
                LocationList.Add(new SelectListItem { Value = item.locationId.ToString(), Text = item.locationName });
            }
            jwm.LocationList = LocationList;
            decimal x = Convert.ToDecimal(ViewBag.UserId);
            
            jwm.Company = db.Companies.Where(c => c.CompUserId == x).FirstOrDefault();
            jwm.User = db.Users.Where(u => u.userId == x).FirstOrDefault();
            return View(jwm);
        }

        [HttpPost]
        public ActionResult AddJob(HttpPostedFileBase photoPath, JobViewModel j)
        {
            Job0Repository jr = new Job0Repository();
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            string PhotoName = "";
            if (photoPath != null)
            {
                if (photoPath.ContentLength > 0)
                {
                    PhotoName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                    string path = Server.MapPath("~/Upload/" + PhotoName);
                    photoPath.SaveAs(path);
                }
                j.Job.JobPhoto = PhotoName;
                if (ModelState.IsValid)
                {
                    InstanceResult<JobO> resultJ = new InstanceResult<JobO>();
                    j.Job.UserId = j.User.userId;
                    j.Job.JobCompId = j.Company.CompId;
                    resultJ.resultint = jr.Insert(j.Job);
                    if (resultJ.resultint.IsSucceeded)
                        return RedirectToAction("ListAllJob", "Home");
                    else
                    {
                        return View(j);
                    }
                }
                else
                    return View();
            }
            else
            {
                ViewBag.BrandErr = "Please Upload Pictures";
                return Redirect("~/Home/AddJob");
            }
        }
        #endregion
        #region LogOut
        public ActionResult LogOut()
        {
            string cookieName1 = "UserLogin";
            HttpCookie myCookie = new HttpCookie(cookieName1);
            string cookieName2 = "UserRole";
            HttpCookie myCookie2 = new HttpCookie(cookieName2);

            myCookie.Expires = DateTime.Now.AddMonths(-1);
            myCookie2.Expires = DateTime.Now.AddMonths(-1);
            Response.Cookies.Add(myCookie);
            Response.Cookies.Add(myCookie2);
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region ApplyJob    
       
        public ActionResult ApplyJob(int id)
        {
            ViewBag.UserId = Request.Cookies["UserId"].Value;
            JobSeekerRepository jr = new JobSeekerRepository();
            InstanceResult<C_JobSeeker> resultt = new InstanceResult<C_JobSeeker>();

            C_JobSeeker cj = new C_JobSeeker();
            cj.JobId = id;
            decimal x = Convert.ToDecimal(ViewBag.UserId);
            cj.SeekerId = db.Seekers.Where(k=>k.empUserId==x).SingleOrDefault().empId;

            resultt .resultint = jr.Insert(cj);
          
                return RedirectToAction("ListAllJob", "Home");
           
        }

        #endregion
        #region Search Job
        public ActionResult SearchJob(SearchModel model)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var y = db.Locations.Where(t => t.locationName.Contains(model.Location.locationName)).FirstOrDefault().locationId;
                List<JobO> finding = db.JobOes.Where(x => x.JobTitle.Contains(model.keyword) && x.JobLocationId == y).ToList();
               
                if (finding.Count==0)
                {
                    ViewBag.WarnNull = "There are no jobs that meet the criteria you are looking for";
                    return View("Index");
                }
                else
                {
                    model.findings = finding;
                    return View(model);
                }               
            }
        }
        #endregion
        #region AppliedJobForCandidates

        public ActionResult AppliedJobList(int id)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            JobViewModel j = new JobViewModel();
            decimal t = Convert.ToDecimal(ViewBag.UserId);
            j.Seeker = db.Seekers.Where(c => c.empUserId == t).FirstOrDefault();
           
            return View(j);
        }
        #endregion
        #region PostedJobsForEmployers
        public ActionResult PostedJobList(int id)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            JobViewModel j = new JobViewModel();
            decimal t = Convert.ToDecimal(ViewBag.UserId);
            j.Company = db.Companies.Where(c => c.CompUserId == t).FirstOrDefault();

            return View(j);
        }
        #endregion
        #region ApllierListForEmployers
        public ActionResult AppliedSeekers(int id)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            JobViewModel j = new JobViewModel();            
            j.Job = db.JobOes.Where(c => c.JobId == id).FirstOrDefault();

            return View(j);
        }
        #endregion
        #region ShowResume
        public ActionResult ShowResume(int id)
        {
            JobViewModel jwm = new JobViewModel();
            jwm.Seeker = db.Seekers.Where(s => s.empId == id).FirstOrDefault();
            return View(jwm);
        }
        #endregion
        #region JobDetail
        
        public ActionResult JobDetail(int id)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;
            Job0Repository jr = new Job0Repository();

            JobO ji = jr.GetObjById(id).ProcessResult;
            return View(ji);
           
        }
       
        #endregion
    }
}



