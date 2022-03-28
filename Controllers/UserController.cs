using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCRBA.Views.User
{
    public class UserController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewUser()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddNewUser(FormCollection col)
        {
            // check if checkbox is checked, if so then submit all data and redirect to new member form?
            // or maybe pull a partial view up?

            if (col["btnNewUser"].ToString() == "newuser")
            {
                //validate data

                // send data if valid to db

                // return to member page - use generated user id as 
                // param
                return RedirectToAction("Index", "Member");
            }
            return View();
        }

        public ActionResult AddNewMember()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddNewMember(FormCollection col)
        {
            if (col["btnSignUp"].ToString() == "submit")
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