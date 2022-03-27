using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCRBA.Controllers
{
    public class LoginController : Controller
    {
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
                user.UserID = col["UserID"];
                user.Password = col["Password"];

                // are input fields empty? 
                if (user.UserID.Length == 0 || user.Password.Length == 0)
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
                        // yes, assign UserID and Password values to UserID and Password properties in User object
                        user.UserID = col["UserID"];
                        user.Password = col["Password"];

                        // call Login method on User object
                        // method will either return a User object or null
                        user = user.Login();

                        if (user != null && user.UID > 0)
                        {
                            // user is not null and is not 0 so we can save the current user session
                            user.SaveUserSession();

                            // redirect to page for logged in members
                            // NEED TO DIFFERENTIATE BETWEEN MEMBER AND NON-MEMBER USER FOR AFTER LOGIN 
                            return RedirectToAction("Index", "Member");
                        }
                        else
                        {
                            user = new Models.User();
                            user.UserID = col["UserID"];
                            user.ActionType = Models.User.ActionTypes.LoginFailed;
                            return View(user);
                        }
                    }
                    return View(user);
                }
            } catch (Exception)
            {
                Models.User user = new Models.User();
                return View(user);
            }
        }

    }
}