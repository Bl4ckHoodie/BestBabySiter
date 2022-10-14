using Best_BabySitter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Best_BabySitter.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login","Admin");

            TempData["unverifiedUsers"] = SqlDataAccess.getUnverified();
            TempData["flaggedUsers"] = SqlDataAccess.getFlagged();
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin admin)
        {
            int id = SqlDataAccess.LoginAdmin(admin);
            if (id != -1)
            {
                Session["admin"] = id;

                return Redirect("Index");
            }
            return View();
        }
        public ActionResult Suspend_User()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login", "Admin");
            List<Sitter> sitter = SqlDataAccess.getSittersToSuspend();
            return View(sitter);
        }

        public ActionResult Verify_Sitter()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login", "Admin");
            List<Sitter> sitter = SqlDataAccess.getUnverified();
            return View(sitter);
        }
        public JsonResult getSitterToVerify(string id)
        {
            Session["verifySitter"] = id;
            Sitter sitter = SqlDataAccess.getSitterData(id);
            return Json(sitter);
        }

        public JsonResult verifySitter(string id)
        {
            if (Session["verifySitter"] != null)
            {
                int verified = SqlDataAccess.verifySitter(id, int.Parse(Session["verifySitter"].ToString()));
                if (verified == 1)
                    TempData["alertMessage"] = "Changes Successfully made...";
                else
                    TempData["errorMessage"] = "DataSQlAccess Error when verifying Sitter...";
            }
            else
            {
                TempData["errorMessage"] = "Error While verifying Sitter...";
            }
            return Json("");
        }
        public ActionResult Update_Info()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login", "Admin");
            Session["admin"] = 1;
            Admin admin = SqlDataAccess.getAdminData(Session["admin"]);
            if(admin != null)
                return View(admin);
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update_Info(Admin admin)
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login", "Admin");
            int valid = SqlDataAccess.updateAdminInfo(admin);
            
            if (valid == 1)
            {
                TempData["alertMessage"] = "Your information is Successfully Updated";
            }
            else
            {
                TempData["errorMessage"] = "There was an Error updating your infomation";
            }
            admin = SqlDataAccess.getAdminData(Session["admin"]);
            if (admin != null)
                return View(admin);
            return View();

        }
        public ActionResult Logout()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login", "Admin");
            Session.Clear();
            ViewData.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}