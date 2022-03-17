using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web2.Controllers
{
// ----------------------------------------------------------- // 
// Name: Indices mgmt section
// Desc:
// ----------------------------------------------------------- // 

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection col)
        {  
            if (col["btnHomeSubmit"].ToString() == "join")
            {
               
                return RedirectToAction("AddNewMember", "Member");
            }
            return View();
        }

    }
}