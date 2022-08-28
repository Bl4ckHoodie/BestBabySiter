using Best_BabySitter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Best_BabySitter.Controllers
{
    public class ParentController : Controller
    {
        // GET: Parent
        public ActionResult Index()
        {
            //Session["parentID"] = 1;
            if(Session["parentID"] == null){
                return RedirectToAction("ParentLogin","Home");
            }else
            return View();
        }

        public ActionResult Advert()
        {
            return View();
        }
        public ActionResult CreateAdvert()
        {
            //Session["parentID"] = 1;
            if(Session["parentID"] == null){
                return RedirectToAction("ParentLogin","Home");
            }else
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdvert(Advert advert)
        {
            if (ModelState.IsValid)
            {
                int added = SqlDataAccess.createAdvert(advert, int.Parse( Session["parentID"].ToString()));
                if(added!= -1)
                {
                    TempData["alertMessage"] = "Advert Succefully Created";
              
                    return RedirectToAction("Index");
                }
      
            }
            TempData["alertMessage"] = "Error occured while creating Advert";
            
            return View();
        }
        public ActionResult ManageAdvert()
        {
            Session["parentID"] = 1;
            if(Session["parentID"] == null){
                return RedirectToAction("ParentLogin","Home");
            }
            List <Advert> openAdverts = SqlDataAccess.getOpenAdverts(int.Parse(Session["parentID"].ToString()));
            ViewData["openAdverts"] = openAdverts;
            return View() ;
        }

    }
}