
//Hlomla Version
using Best_BabySitter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Best_BabySitter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Parent_Register()
        {
            if (ModelState.ContainsKey("Error"))
                ModelState.Remove("Error");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Parent_Register(Parent parent){
            if (ModelState.IsValid)
            {
                int valid = SqlDataAccess.registerParent(parent);
                if (valid == 1)
                {
                    TempData["alertMessage"] = "Succefully Registered Created";
                    int id = SqlDataAccess.getParentID(parent);
                    if(id!= -1){
                        Session["parentID"] = id;
                    return RedirectToAction("Index", "Parent");
                    }
                }else if (valid == 0)
                {
                    ModelState.AddModelError("Error", new Exception("account already exists login"));
                    return RedirectToAction("Parent_Login");
                }
                else
                {
                    return View();
                }
            }
            TempData["alertMessage"] = "Error occured while registering parent";
            return View();
        }


        public ActionResult Parent_Login()
        {
            if (ModelState.ContainsKey("Error"))
                ModelState.Remove("Error");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Parent_Login(Parent parent)
        {

                int parent_ID = SqlDataAccess.LoginParent(parent);
                if (parent_ID != -1)
                {
                    Session["parentID"] = parent_ID;
                    return RedirectToAction("Index", "Parent");
                }
                else
                {
                    ModelState.AddModelError("Error", new Exception("Incorrect email/password"));
                    return View();
                }
        }

        public ActionResult Sitter_Login()
        {
            if(ModelState.ContainsKey("Error"))
                ModelState.Remove("Error");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sitter_Login(Sitter sitter)
        {
            int sitter_ID = SqlDataAccess.LoginSitter(sitter);
            if (sitter_ID != -1)
            {
                Session["SitterID"] = sitter_ID;
                
                return RedirectToAction("Index", "Sitter");
            }
            else
            {
                ModelState.AddModelError("Error", new Exception("Incorrect email/password"));
                return View();
            }
        }

        public ActionResult Sitter_Register()
        {
            if (ModelState.ContainsKey("Error"))
                ModelState.Remove("Error");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sitter_Register(Sitter sitter)
        {
          
            int valid = SqlDataAccess.registerSitter(sitter);
            if (valid == 1)
            {
                TempData["alertMessage"] = "Succefully Registered as a Sitter";
                int sitter_ID = SqlDataAccess.LoginSitter(sitter);
                if (sitter_ID != -1)
                {
                    Session["SitterID"] = sitter_ID;
                    //TempData["sitterCV"] = Response.Cookies["sitterCV"].Value;
                    return RedirectToAction("Index", "Sitter");
                }
            }
            else if (valid == 0)
            {
                ModelState.AddModelError("Error", new Exception("account already exists login"));
                return RedirectToAction("Sitter_Login");
            }
            else
            {
                return View();
            }
            return View();
        }
        private void updateSitter(Sitter sitter)
        {
            SqlDataAccess.updateSitterInfo(sitter);
        } 

        [HttpPost]
        public JsonResult uploadCVFile()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath("~/Content/Sitters_Data/temp/"));
                    string path = Path.Combine(Server.MapPath("~/Content/Sitters_Data/temp/"), fileName);
                    // save the file
                    file.SaveAs(path);
                    Response.Cookies["sitterCV"].Value = Path.GetFileName(path);
                    return Json("Successfully Uploaded");
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }
            return Json("no files were selected !");
        }
    }
   
}