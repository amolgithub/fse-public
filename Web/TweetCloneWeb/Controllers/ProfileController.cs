using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using TweetCloneWeb.Entities;
using TweetCloneWeb.Util;

namespace TweetCloneWeb.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Main Page with login and signup view
        public ActionResult Index()
        {
            return View("../Index");
        }

        // Login action handler
        [HttpPost]
        public ActionResult Login(Person login)
        {
            var context = new TweetCloneDB();
            var user = context.People.FirstOrDefault(p => p.UserId.Equals(login.UserId, StringComparison.OrdinalIgnoreCase) && p.Password.Equals(login.Password));
            Session["userid"] = string.Empty;
            Session["fullname"] = "Not logged in";

            if (user!= null)
            {
                Session["userid"] = user.UserId;
                Session["fullname"] = user.FullName;
            }
            return Json(user != null);
        }

        [EntitlementFilter]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        // Signup action handler
        [HttpPost]
        public ActionResult Signup(Person signup)
        {
            var context = new TweetCloneDB();
            try
            {
                var foundUser = context.People.FirstOrDefault(p => p.UserId.Equals(signup.UserId, StringComparison.OrdinalIgnoreCase));

                if (foundUser != null) return Json("User already exist!");                
                signup.Joined = DateTime.Now;
                signup.Active = true;
                context.People.Add(signup);
                context.SaveChanges();
                Session["userid"] = signup.UserId;
                Session["fullname"] = signup.FullName;
                return Json(string.Empty);
            }
            catch(DbEntityValidationException ex)
            {
                var errors = new StringBuilder();
                foreach(var e in ex.EntityValidationErrors)
                {
                    foreach(var err in e.ValidationErrors)
                    {
                        errors.AppendFormat("{0}<br>",  err.ErrorMessage);
                    }
                }
                return Json(errors.ToString());
            }
            catch(Exception ex)
            {                
                return Json(ex.Message);
            }
        }

        // Profile edit view
        [EntitlementFilter]
        public ActionResult Edit()
        {
            return View(GetUser());
        }

        // Save profile changes
        [HttpPost,EntitlementFilter]
        public ActionResult Save(Person modifiedProfile)
        {
            var context = new TweetCloneDB();
            try
            {
                var existingProfile = context.People.FirstOrDefault(p => p.UserId.Equals(modifiedProfile.UserId, StringComparison.OrdinalIgnoreCase));
                existingProfile.FullName = modifiedProfile.FullName;
                existingProfile.Email = modifiedProfile.Email;
                existingProfile.Password = modifiedProfile.Password;
                context.SaveChanges();
                return Json(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        // Delete profile action
        [HttpPost,EntitlementFilter]
        public ActionResult Delete()
        {
            try
            {
                var context = new TweetCloneDB();                   
                String userId = Session["userid"] as String;
                var user = context.People.FirstOrDefault(p => p.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));
                context.Tweets.RemoveRange(user.Tweets);                
                context.Followings.RemoveRange(user.Followers);
                context.Followings.RemoveRange(user.Followings);
                context.People.Remove(user);
                context.SaveChanges();
                return PartialView("RemoveProfileResult");
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }


        // Search user
        public ActionResult Search()
        {
            return View();
        }

        private Person GetUser()
        {
            var context = new TweetCloneDB();

            String userId = Session["userid"] as String;
            return context.People.FirstOrDefault(p => p.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));
        }   
    }
}