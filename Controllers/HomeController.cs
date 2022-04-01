using GCRBA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCRBA.Controllers {
// ----------------------------------------------------------- // 
// Name: Indices mgmt section
// Desc:
// ----------------------------------------------------------- // 

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MainBanner = "";

            try
            {
                Database db = new Database();
                ViewBag.MainBanner = db.GetMainBanner();
                return View();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        [HttpPost]
        public ActionResult Index(FormCollection col)
        {  
            if(col["btnSubmit"].ToString() == "join") {
                return RedirectToAction("AddNewMember", "User");
            }

            if(col["btnSubmit"].ToString() == "login")
            {
                return RedirectToAction("Index", "Profile");
            }

            if (col["btnSubmit"].ToString() == "signup")
            {
                return RedirectToAction("AddNewUser", "User");
            }

            if (col["btnSubmit"].ToString() == "bakery") {
				return RedirectToAction("Index", "Bakery");
			}

            if (col["btnSubmit"].ToString() == "vendor") {
                return RedirectToAction("Index", "Vendor");
            }

            if (col["btnSubmit"].ToString() == "education") {
                return RedirectToAction("Index", "Education");
            }
            return View();
        } 
    }
}