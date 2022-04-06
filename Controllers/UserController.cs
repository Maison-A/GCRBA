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
            Models.User u = new Models.User();
            u = u.GetUserSession();
           
            return View(u);
        }

        [HttpPost]
        public ActionResult Index(FormCollection col)
        {
            try
            {
                Models.User u = new Models.User();
                u = u.GetUserSession();
                u.FirstName = col["FirstName"];
                u.LastName = col["LastName"];
                u.Email = col["Email"];
                //u.UserID = col["UserID"];
                u.Password = col["Password"];

                if (u.FirstName.Length == 0 || u.LastName.Length == 0 || u.Email.Length == 0 || u.Password.Length == 0)
                {
                    u.ActionType = Models.User.ActionTypes.RequiredFieldMissing;
                    return View(u);
                }
                else
                {
                    if (col["btnSubmit"] == "newuser")
                    { //sign up button pressed
                        Models.User.ActionTypes at = Models.User.ActionTypes.NoType;
                        at = u.Save();
                        switch (at)
                        {
                            case Models.User.ActionTypes.InsertSuccessful:
                                u.SaveUserSession();
                                // TODO: return to user profile page/index
                                return RedirectToAction("Home","Index");
                            //break;
                            default:
                                return View(u);
                                //break;
                        }
                    }
                    else
                    {
                        return View(u);
                    }
                }
            }
            catch (Exception)
            {
                Models.User u = new Models.User();
                return View(u);
            }
        }

        public ActionResult AddNewUser()
        {
            Models.User u = new Models.User();
            return View(u);
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
                // I commented this out to avoid it throwing an error for now but in the User class, I changed
                // intStateID to strState because I now added the ability for it to pull the state name in, not just the ID
                // but I didn't want to change this without knowing if it would affect anything so let me know. -Katie 
                //u.intStateID = 0;
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


        // TODO: bring up how to manage initilization
        // will we be forcing members to become Users? 
        public ActionResult AddNewMember()
        {
            // if user exists: update user value to member
            Models.User u = new Models.User();
            return View(u);

            // if user doesnt exist - redirect to sign up?
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
