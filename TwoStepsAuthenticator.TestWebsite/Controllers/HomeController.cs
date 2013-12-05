using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TwoStepsAuthenticator.TestWebsite.Users;

namespace TwoStepsAuthenticator.TestWebsite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private static readonly UsedCodesManager usedCodesManager = new UsedCodesManager();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            if (Membership.ValidateUser(login, password))
            {
                var user = WebsiteUserStorage.GetUser(login);
                if (user.DoubleAuthActivated)
                {
                    Session["AuthenticatedUser"] = user;
                    return View("DoubleAuth", user);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(login, true);
                    return RedirectToAction("Welcome");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DoubleAuth(string code)
        {
            WebsiteUser user = (WebsiteUser)Session["AuthenticatedUser"];
            var auth = new TwoStepsAuthenticator.TimeAuthenticator();
            if (auth.CheckCode(user.DoubleAuthKey, code) && usedCodesManager.IsCodeUsed(user.DoubleAuthKey, code))
            {
                usedCodesManager.AddCode(user.DoubleAuthKey, code);
                FormsAuthentication.SetAuthCookie(user.Login, true);
                return RedirectToAction("Welcome");
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Welcome()
        {
            return View() ;
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}
