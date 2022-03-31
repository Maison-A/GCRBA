using GCRBA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCRBA.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            Models.User user = new Models.User();
            return View(user);
        }

        [HttpPost]
        public ActionResult Index(FormCollection col)
        {
            try
            {
                // create instance of user object to pass to the view 
                Models.User user = new Models.User();

                // get whatever input is in the textboxes 
                user.Username = col["Username"];
                user.Password = col["Password"];

                // are input fields empty? 
                if (user.Username.Length == 0 || user.Password.Length == 0)
                {
                    // yes, change User ActionType and return View with User object as argument 
                    user.ActionType = Models.User.ActionTypes.RequiredFieldMissing;
                    return View(user);
                }
                // no, fields aren't empty 
                else
                {
                    // has submit button with value login been pressed?
                    if (col["btnSubmit"] == "login")
                    {
                        // yes, assign Username and Password values to Username and Password properties in User object
                        user.Username = col["Username"];
                        user.Password = col["Password"];

                        // call Login method on User object
                        // method will either return a User object or null
                        user = user.Login();

                        if (user != null && user.UID > 0)
                        {
                            // user is not null and is not 0 so we can save the current user session 
                            user.SaveUserSession();
                            Database db = new Database();

                            if (db.IsUserMember(user))
                            {
                                return RedirectToAction("MemberProfile", "Profile");
                            }
                            else
                            {
                                return RedirectToAction("NonMemberProfile", "Profile");
                            }


                        }
                        else
                        {
                            user = new Models.User();
                            user.Username = col["Username"];
                            user.ActionType = Models.User.ActionTypes.LoginFailed;
                            return View(user);
                        }
                    }
                    return View(user);
                }
            }
            catch (Exception)
            {
                Models.User user = new Models.User();
                return View(user);
            }
        }

        public ActionResult MemberProfile()
        {
            // add more later but just return view for now to avoid error 
            return View();
        }

        // for when a user logs in and isn't a member 
        public ActionResult NonMemberProfile()
        {
            // add more but just return view for now to avoid error 
            return View();
        }

        // for when a user logs in and is an admin 
        public ActionResult AdminProfile()
        {
            // add more later but just return view for now to avoid error 
            return View();
        }
    }
}