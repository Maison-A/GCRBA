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


        public ActionResult AddNewMember()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddNewMember(FormCollection col)
        {
            if (col["btnSignUp"].ToString() == "signup")
            {
                //validate data

                // send data if valid to db

                // return to member page - use generated user id as 
                // param
                return RedirectToAction("Index", "Member");
            }
            return View();
        }

    }


}