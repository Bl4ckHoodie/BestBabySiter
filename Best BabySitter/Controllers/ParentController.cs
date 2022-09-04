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

            Session["parentID"] = 1;
            if(Session["parentID"] == null){
                return RedirectToAction("ParentLogin","Home");
            }
            Parent parent = SqlDataAccess.getParentData(int.Parse(Session["parentID"].ToString()));
            Session["parentName"] = parent.f_name.Substring(0,1).ToUpper() + "." + parent.l_name.ToUpper();
            TempData["slots"] = SqlDataAccess.getSlots(int.Parse(Session["parentID"].ToString()));
            TempData["openAdvert"] = SqlDataAccess.getOpenAdverts(int.Parse(Session["parentID"].ToString()));
            TempData["appointment"]= SqlDataAccess.getAppointments(int.Parse(Session["parentID"].ToString()),1);
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
            TempData["errorMessage"] = "Error occured while creating Advert";
            
            return View();
        }
        public ActionResult ManageAdvert()
        {
            //Session["parentID"] = 1;
            if(Session["parentID"] == null){
                return RedirectToAction("ParentLogin","Home");
            }
            List <Advert> openAdverts = SqlDataAccess.getOpenAdverts(int.Parse(Session["parentID"].ToString()));
            ViewData["openAdverts"] = openAdverts;
            return View() ;
        }

        public JsonResult AdvertDetail(string id)
        {
            int pos = int.Parse(id);
            TempData["curAdvert"] = id;
            List<Advert> list = SqlDataAccess.getOpenAdverts(int.Parse(Session["parentID"].ToString()));
            return Json(list[pos]);
          
        }

        public JsonResult GetResponses(){
            List<Advert> list = SqlDataAccess.getOpenAdverts(int.Parse(Session["parentID"].ToString()));
            Advert advert = list[int.Parse(TempData["curAdvert"].ToString())];
            List<Sitter> sitters = SqlDataAccess.getResponses(advert.ID);
            return Json(sitters);
        }
        public int UpdateAdvert(string obj){
            List<Advert> list = SqlDataAccess.getOpenAdverts(int.Parse(Session["parentID"].ToString()));
            Advert advert = list[int.Parse(TempData["curAdvert"].ToString())];
            var js =  new JavaScriptSerializer();
            Advert newAdd = js.Deserialize<Advert>(obj);
            int valid = SqlDataAccess.updateAdvert(advert);
            if(valid == 1)
            {
                TempData["alertMessage"] = "Advert Succefully Updated";
            }else{
                TempData["errorMessage"] = "Error occured while creating Updating";
            }

            return valid;
        }

        public ActionResult Logout()
        {
            Session.Clear();
            ViewData.Clear();
            TempData.Clear();
            return RedirectToAction("Index", "Home");
        }
        public int TestAjax(string id)
        {
            return int.Parse(id.ToString());
        }

    }
}