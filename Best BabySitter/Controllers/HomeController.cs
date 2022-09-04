
//Hlomla Version
using Best_BabySitter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult ParentRegister()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ParentRegister(Parent parent){
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
                    return RedirectToAction("ParentLogin");
                }
                else
                {
                    return View();
                }
            }
            TempData["alertMessage"] = "Error occured while registering parent";
            return View();
        }
    }
}