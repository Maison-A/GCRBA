using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCRBA.Controllers
{
    public class MemberController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Index(FormCollection col)
        //{
        //    if (col["btnHomeSubmit"].ToString() == "join")
        //    {

        //        return RedirectToAction("Index", "Member");
        //    }
        //    return View();
        //}


        public ActionResult AddNewMember()
        {
            return View();
        }
    }

  
}