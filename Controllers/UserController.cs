using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

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
            try
            { 
                // check if checkbox is checked, if so then submit all data and redirect to new member form?
                // or maybe pull a partial view up?
                Models.User u = new Models.User();

                u.FirstName = col["FirstName"];
                u.LastName = col["LastName"];
                u.Email = col["Email"];
                u.Username = col["Username"];
                u.Password = col["Password"];

                if (col["btnSubmit"].ToString() == "newuser")
                {
                    //validate data - trying to check pass values match
                    if (col["passver1"].ToString() != col["passver2"].ToString())
                    {
                        u.ActionType = Models.User.ActionTypes.Unknown;
                        return View(u);
                    }
                }
                // send data if valid to db
                else
                {

                    // return to member page - be sure to maintain current user
                    return RedirectToAction("Index", "User");
                }
                return View(u);
            }
            catch (Exception)
            {
                Models.User u = new Models.User();
                return View(u);
            }
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
