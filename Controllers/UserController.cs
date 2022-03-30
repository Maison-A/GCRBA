using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace GCRBA.Controllers
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
                u.Address = string.Empty;
                u.City = string.Empty;
                u.intStateID = 0;
                u.Zip = string.Empty;
                u.Phone = string.Empty;
                u.MemberShipType = string.Empty;
                u.PaymentType = string.Empty;
                
                if (col["btnSubmit"].ToString() == "newuser")
                {
                    if (u.FirstName.Length == 0 || u.LastName.Length == 0 || u.Email.Length == 0 || u.Username.Length == 0
                        || u.Password.Length == 0)
                    {
                        u.ActionType = Models.User.ActionTypes.RequiredFieldMissing;
                        return View(u);
                    }

                    //validate data - trying to check pass values match
                    //if (col["passver1"].ToString() != col["passver2"].ToString())
                    //{
                    //    u.ActionType = Models.User.ActionTypes.Unknown;
                    //    return View(u);
                    //}
                }
                // send data if valid to db
                else
                {

                    // return to member page - be sure to maintain current user
                    return RedirectToAction("Index", "Home");
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
