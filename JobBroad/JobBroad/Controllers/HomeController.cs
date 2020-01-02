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
using System.IO;
using Rotativa;

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
        public ActionResult Register(User u, HttpPostedFileBase photo)
        {
            InstanceResult<User> resultt = new InstanceResult<User>();
            UserRepository ur = new UserRepository();
            CompanyRepository cr = new CompanyRepository();
            SeekerRepository sr = new SeekerRepository();
            string photoPath = "";

            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {
                    photoPath = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                    string path = Server.MapPath("~/Upload/" + photoPath);
                    photo.SaveAs(path);

                }

                u.userPhoto = photoPath;
                resultt.resultint = ur.Insert(u);
                Session["User"] = ur.GetLatestObj(1).ProcessResult[0];
                if (resultt.resultint.IsSucceeded)
                {
                    if (((User)Session["User"]).userRoleId == 1)
                    {
                        Seeker s = new Seeker();
                        s.empUserId = ((User)Session["User"]).userId;
                        s.empName = ((User)Session["User"]).userName;
                        s.empSurname = ((User)Session["User"]).userSurname;
                        s.empEmail = ((User)Session["User"]).userMail;
                        s.empPhoto = ((User)Session["User"]).userPhoto;
                        sr.Insert(s);
                    }
                    if (((User)Session["User"]).userRoleId == 2)
                    {
                        Company c = new Company();
                        c.CompUserId = ((User)Session["User"]).userId;
                        c.CompName = ((User)Session["User"]).userName;
                        c.CompPhoto = ((User)Session["User"]).userPhoto;
                        cr.Insert(c);
                    }

                    ViewBag.SignIn = "Your account has been sucessfully created ! Please Sign In.";
                    return View("Login");
                }
                ViewBag.UserRole = Request.Cookies["UserRole"].Value;
                ViewBag.SignIn = "Please try again !";
                return View();
            }
            else
            {
                TempData["Msg"] = "Please Upload Pictures";
                return Redirect("~/Home/Register");
            }
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

            var user = db.Users.FirstOrDefault(x => x.userName == model.userMail && x.userPassword == model.userPassword);
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

                Job0Repository jr = new Job0Repository();
                var i = 0;
                foreach (JobO item in jr.List().ProcessResult)
                {
                    i++;
                }
                ViewBag.TotalJob = i;

                JobViewModel jwm = new JobViewModel();

                return View("Index");
            }

            ViewBag.Unmatched = "Incorrect username or password";
            return RedirectToAction("Index", "Home");
        }

        #endregion
        #region Edit Profile for Employer

        public ActionResult EditProfile(int id)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            CompanyRepository cr = new CompanyRepository();
            InstanceResult<Company> result = new InstanceResult<Company>();

            result.TResult = cr.GetObjById(id);
            return View(result.TResult.ProcessResult);

        }
        [HttpPost]
        public ActionResult EditProfile(Company model, HttpPostedFileBase photo)
        {
            InstanceResult<Company> result = new InstanceResult<Company>();
            CompanyRepository cr = new CompanyRepository();
            string photoName = model.CompPhoto;
            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {
                    string ext = Path.GetExtension(photo.FileName);
                    photoName = Guid.NewGuid().ToString().Replace("-", "");
                    if (ext == ".jpg")
                        photoName += ext;
                    else if (ext == ".png")
                        photoName += ext;
                    else if (ext == ".bmp")
                        photoName += ext;
                    else
                    {
                        ViewBag.Mesaj = "Please upload photos of type .jpg,.png,.bmp ";
                        return View(model);
                    }
                    string path = Server.MapPath("~/Upload/" + photoName);
                    photo.SaveAs(path);
                }
            }
            model.CompPhoto = photoName;
            result.resultint = cr.Update(model);
            if (result.resultint.IsSucceeded)
                return RedirectToAction("ListAllJob");
            else
                return View(model);
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
        #region Listing All Candidates
        public ActionResult ListCandidate()
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            SeekerRepository sr = new SeekerRepository();
            InstanceResult<Seeker> result = new InstanceResult<Seeker>();
            result.resultList = sr.List();
            return View(result.resultList.ProcessResult);
        }
        #endregion
        #region Education Adding
        public ActionResult EducationAdd(ResumeViewModel r)
        {
            ViewBag.UserId = Request.Cookies["UserId"].Value;
           
            InstanceResult<C_JobSeeker> resultt = new InstanceResult<C_JobSeeker>();
            EducationRepository er = new EducationRepository();
            decimal x = Convert.ToDecimal(ViewBag.UserId);
          
            r.Education.UserId = x;
            r.Education.CvId = r.CV.CVId;
            resultt.resultint = er.Insert(r.Education);
            
            return RedirectToAction("ResumeDetail", "Home");
        }
        #endregion
        #region Resume Adding
       
        public ActionResult ResumeAdd(int id)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            EducationRepository er = new EducationRepository();
            ResumeViewModel rwm = new ResumeViewModel();
            decimal x = Convert.ToDecimal(ViewBag.UserId);

            rwm.Seeker = db.Seekers.Where(t => t.empUserId == x).FirstOrDefault();
            rwm.User = db.Users.Where(t=>t.userId==id).FirstOrDefault();
            return View(rwm);
        }

        [HttpPost]
        public ActionResult ResumeAdd(ResumeViewModel model2)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            InstanceResult<JobO> resultJ = new InstanceResult<JobO>();
            InstanceResult<Seeker> resultS = new InstanceResult<Seeker>();
            InstanceResult<CV> resultttt = new InstanceResult<CV>();
            InstanceResult<Education> resulttt = new InstanceResult<Education>();
            InstanceResult<Experience> result = new InstanceResult<Experience>();
            InstanceResult<User> resultu = new InstanceResult<User>();

            SeekerRepository sr = new SeekerRepository();
            CVRepository cr = new CVRepository();
            ExperienceRepository er = new ExperienceRepository();
            EducationRepository edr = new EducationRepository();
            UserRepository ur = new UserRepository();

            User u = model2.User;
            
            CV c = model2.CV;
            c.UserId= model2.User.userId;
            c.CvName = model2.CV.CvName;
            resultttt.resultint = cr.Insert(c);
            
            c.CVId = model2.CV.CVId;
            u.userCVID = model2.CV.CVId;

            resultu.resultint = ur.Update(u);

            return RedirectToAction("ResumeDetail","Home", new { model = model2 });
            
        }

        #endregion
        #region Job Adding

        public ActionResult AddJob()
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            JobO j = new JobO();
            decimal t = Convert.ToDecimal(ViewBag.UserId);
            Company c = db.Companies.SingleOrDefault(p => p.CompUserId == t);
            User u = db.Users.SingleOrDefault(s => s.userId == t);
            j.JobCompId = c.CompId;
            j.JobPhoto = c.CompPhoto;
            j.UserId = u.userId;

            return View(j);
        }
        [HttpPost]
        public ActionResult AddJob(JobO x, HttpPostedFileBase photo)
        {
            Job0Repository jr = new Job0Repository();
            InstanceResult<JobO> result = new InstanceResult<JobO>();
            JobO h = new JobO();
            
            h.JobCompId = x.JobCompId;
            h.UserId = x.UserId;
            h.JobTitle = x.JobTitle;
            h.JobDetail = x.JobDetail;
            h.JobPosition = x.JobPosition;
            h.JobLocationId = x.JobLocationId;
            h.JobPhoto = x.JobPhoto;
            result.resultint = jr.Insert(h);
            if (result.resultint.ProcessResult > 0)
            {
                return RedirectToAction("PostedJobList", "Home", new { id = x.UserId });
            }
            return View(x);
        }

        #endregion
        #region Work Experience Adding
        public ActionResult WorkAdd (ResumeViewModel r)
        {
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            InstanceResult<C_JobSeeker> resultt = new InstanceResult<C_JobSeeker>();
            ExperienceRepository er = new ExperienceRepository();
            decimal x = Convert.ToDecimal(ViewBag.UserId);

            r.Experience.UserId = x;
            r.Experience.CvID = r.CV.CVId;
            resultt.resultint = er.Insert(r.Experience);

            return RedirectToAction("ResumeDetail", "Home");
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
          
            
                cj.SeekerId = db.Seekers.Where(k => k.empUserId == x).FirstOrDefault().empId;
                resultt.resultint = jr.Insert(cj);
                return RedirectToAction("ListAllJob", "Home");
         
        }

        #endregion
        #region EditPostedJob

        public ActionResult EditJob(int id)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            Job0Repository jr = new Job0Repository();
            InstanceResult<JobO> result = new InstanceResult<JobO>();

            result.TResult = jr.GetObjById(id);
            return View(result.TResult.ProcessResult);
        }
        [HttpPost]
        public ActionResult EditJob(JobO model)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;
            Job0Repository jr = new Job0Repository();
            InstanceResult<JobO> result = new InstanceResult<JobO>();


            result.resultint = jr.Update(model);
            if (result.resultint.ProcessResult > 0)
            {
                ViewBag.Mesaj = result.resultint.UserMessage;
                return RedirectToAction("PostedJobList", "Home", new { id = Convert.ToInt32(ViewBag.UserId) });
            }
            else
                return View(model);
        }
        #endregion
        #region DeletePostedJob
        public ActionResult DeleteJob(int id)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            Job0Repository jr = new Job0Repository();
            InstanceResult<JobO> result = new InstanceResult<JobO>();
            result.resultint = jr.Delete(id);

            return RedirectToAction("PostedJobList", "Home", new { id = Convert.ToInt32(ViewBag.UserId) });

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

                if (finding.Count == 0)
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
            EducationRepository er = new EducationRepository();
            WorkRepository wr = new WorkRepository();
            List<Education> ListEdu = new List<Education>();
            List<Experience> ListWork = new List<Experience>();

            decimal x = Convert.ToDecimal(ViewBag.UserId);
            ResumeViewModel rwm = new ResumeViewModel();
            foreach (Experience item in wr.List().ProcessResult)
            {
                if (item.UserId == id)
                {
                    ListWork.Add(item);
                }
            }
            foreach (Education item in er.List().ProcessResult)
            {
                if (item.UserId == id)
                {
                    ListEdu.Add(item);
                }
            }
            ViewBag.CurrentUser = "a";
            ViewBag.UserRole = "2";
            
            rwm.EduList = ListEdu;
            rwm.WorkList = ListWork;

            rwm.Seeker = db.Seekers.Where(s => s.empUserId == id).FirstOrDefault();
            rwm.CV = db.CVs.Where(s => s.UserId == id).FirstOrDefault();

            return View(rwm);

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
        #region Resume Detail
        [HttpGet]
        public ActionResult ResumeDetail(ResumeViewModel rwm)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            EducationRepository er = new EducationRepository();
            WorkRepository wr = new WorkRepository();
            List<Education> ListEdu = new List<Education>();
            List<Experience> ListWork = new List<Experience>();
           
            decimal x = Convert.ToDecimal(ViewBag.UserId);
            foreach (Experience item in wr.List().ProcessResult)
            {
                if (item.UserId == x)
                {
                    ListWork.Add(item);
                }
            }
            foreach (Education item in er.List().ProcessResult)
            {
                if (item.UserId == x)
                {
                    ListEdu.Add(item);
                }
            }
        
            rwm.EduList = ListEdu;
            rwm.WorkList = ListWork;
            rwm.Seeker = db.Seekers.Where(y=>y.empUserId==x).FirstOrDefault();
            rwm.CV = db.CVs.Where(y => y.UserId == x).FirstOrDefault();
            return View(rwm);
        }
        #endregion
        #region PdfCV
        [HttpGet]
       public ActionResult ExportPDF(int Id)
        {
           
            return new ActionAsPdf("ShowResume",new {id=Id})
            {
                FileName = Server.MapPath("~/Content/Resume.pdf")
            };
        }
        #endregion
        #region Edit Candidate Profile
       public ActionResult EditCanProf(int id)
        {
            ViewBag.CurrentUser = Request.Cookies["UserLogin"].Value;
            ViewBag.UserRole = Request.Cookies["UserRole"].Value;
            ViewBag.UserId = Request.Cookies["UserId"].Value;

            SeekerRepository sr = new SeekerRepository();
            InstanceResult<Seeker> result = new InstanceResult<Seeker>();

            result.TResult = sr.GetObjById(id);
            return View(result.TResult.ProcessResult);
        }

        [HttpPost]
        public ActionResult EditCanProf(Seeker model, HttpPostedFileBase photo)
        {
            InstanceResult<Seeker> result = new InstanceResult<Seeker>();
            SeekerRepository sr = new SeekerRepository();
            string photoName = model.empPhoto;
            if (photo != null)
            {
                if (photo.ContentLength > 0)
                {
                    string ext = Path.GetExtension(photo.FileName);
                    photoName = Guid.NewGuid().ToString().Replace("-", "");
                    if (ext == ".jpg")
                        photoName += ext;
                    else if (ext == ".png")
                        photoName += ext;
                    else if (ext == ".bmp")
                        photoName += ext;
                    else
                    {
                        ViewBag.Mesaj = "Please upload photos of type .jpg,.png,.bmp ";
                        return View(model);
                    }
                    string path = Server.MapPath("~/Upload/" + photoName);
                    photo.SaveAs(path);
                }
            }
            model.empPhoto = photoName;
            result.resultint = sr.Update(model);
            if (result.resultint.IsSucceeded)
                return RedirectToAction("Index");
            else
                return View(model);
        }
        #endregion
    }
}



