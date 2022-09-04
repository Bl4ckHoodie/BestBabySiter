using System;
using System.Collections.Generic;
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
            return View();
        }
    }
}