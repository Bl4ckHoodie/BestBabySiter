using Best_BabySitter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Best_BabySitter.Controllers
{
    public class SitterController : Controller
    {
        // GET: Sitter
        public ActionResult Index()
        {
            if (Session["SitterID"] == null)
            {
                return RedirectToAction("Sitter_Login", "Home");
            }
            TempData["OpenAdvert"] = SqlDataAccess.getOpenAdvertsForSitter(int.Parse(Session["SitterID"].ToString()));
            List<Appointment> appAd = SqlDataAccess.getAppointments(int.Parse(Session["SitterID"].ToString()), 2);
            List<Appointment> appSlot = SqlDataAccess.getSitterAppointmentSlots(int.Parse(Session["SitterID"].ToString()));
            TempData["createdSlots"] = SqlDataAccess.getSitterCreatedSlotCount(int.Parse(Session["SitterID"].ToString()));
            TempData["JobsDone"] = SqlDataAccess.getSitterJobDone(int.Parse(Session["SitterID"].ToString()));
            foreach (Appointment ap in appSlot)
            {
                appAd.Add(ap);
            }
            Sitter sitter = SqlDataAccess.getSitterActualData(Session["SitterID"].ToString());
            if (TempData["sitterCV"] != null)
            {
                string src = TempData["sitterCV"].ToString();
                // create the uploads folder if it doesn't exist
                Directory.CreateDirectory(Server.MapPath("~/Content/Sitters_Data/Sitter" + sitter.sitter_ID + "/"));
                string[] files = Directory.GetFiles("~/Content/Sitters_Data/temp/"+src);
                string path = "";
                foreach(string file in files)
                {
                    path = "~/Content/Sitters_Data/Sitter" + sitter.sitter_ID + "/" + Path.GetFileName(file);
                    Directory.Move(file,path);
                }
                sitter.cv_filePath = path;
                SqlDataAccess.updateSitterInfo(sitter);
            }
            Session["sitterCV"] = null;
            
            TempData["appointment"] = appAd;
            sitter.password = null;
            return View(sitter);
        }


        public ActionResult Advert(){
            if (Session["SitterID"] == null)
            {
                return RedirectToAction("Sitter_Login", "Home");
            }
            List<Advert> adverts = SqlDataAccess.getOpenAdvertsForSitter(int.Parse(Session["SitterID"].ToString()));
            return View(adverts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Advert(Advert adver)
        {
            if (Session["curAdvert"] != null)
            {
                List<Advert> list = SqlDataAccess.getOpenAdvertsForSitter(int.Parse(Session["SitterID"].ToString()));
                if (list.Count > 0) {
                    Advert advert = list[int.Parse(Session["curAdvert"].ToString())];
                     int sitterID = int.Parse(Session["SitterID"].ToString());
                    if (Session["SitterID"] != null)
                    {
                        sitterID = int.Parse(Session["SitterID"].ToString());
                    }
                    bool validate = SqlDataAccess.applyForJob(sitterID, advert.ID);

                    if (validate)
                    {
                        TempData["alertMessage"] = "Succefully Applied For Job #AD0" + advert.ID;
                    }
                    else
                    {
                        TempData["errorMessage"] = "Error occured while applying for job,\n please try again\nor Contact Support Service";
                    } 
                }
                List<Advert> adverts = SqlDataAccess.getOpenAdvertsForSitter(int.Parse(Session["SitterID"].ToString()));
                Session["curAdvert"] = null;
                return View(adverts);
            }
            else
            {
                List<Advert> adverts = SqlDataAccess.getOpenAdvertsForSitter(int.Parse(Session["SitterID"].ToString()));
                return View(adverts);
            }
        }

        [HttpPost]
        public JsonResult AdvertDetail(string id)
        {
            int pos = int.Parse(id);
            Session["curAdvert"] = id;
            List<Advert> list = SqlDataAccess.getOpenAdvertsForSitter(int.Parse(Session["SitterID"].ToString()));
            Advert advert = list[int.Parse(Session["curAdvert"].ToString())];
            return Json(list[pos]);

        }


        public ActionResult Logout()
        {
            Session.Clear();
            TempData.Clear();
            ViewData.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Slots()
        {
            if (Session["SitterID"] == null)
            {
                return RedirectToAction("Sitter_Login", "Home");
            }
            List<Slot> slots = new List<Slot>();
            slots = SqlDataAccess.getSitterOpenSlots(int.Parse(Session["SitterID"].ToString()));
            return View(slots);
        }



        public ActionResult Settings()
        {
            if (Session["SitterID"] == null)
            {
                return RedirectToAction("Sitter_Login", "Home");
            }
            Sitter sitter = SqlDataAccess.getSitterData(Session["SitterID"].ToString());

            if (sitter != null)
            {
                sitter.password = "";
                return View(sitter);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(Sitter sitter)
        {
            int valid = SqlDataAccess.updateSitterInfo(sitter);
            if(valid == 1)
            {
                TempData["alertMessage"] = "Your information is Successfully Updated";
            }
            else
            {
                TempData["errorMessage"] = "There was an Error updating your infomation";
            }
            sitter = SqlDataAccess.getSitterData(Session["SitterID"].ToString());

            if (sitter != null)
            {
                sitter.password = "";
                return View(sitter);
            }
            return View();
        }
    }
}