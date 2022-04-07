﻿using GCRBA.Models;
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
            user = user.GetUserSession();
            if (user.IsAuthenticated)
            {
                ViewBag.Name = user.FirstName + " " + user.LastName;
            }
            return View(user);
        }



        public ActionResult Login()
        {
            User u = new User();
            return View(u);
        }

        [HttpPost]
        public ActionResult Login(FormCollection col)
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

                            // create instance of datbase object 
                            Database db = new Database();

                            // call method that determines if current user is member or not 
                            db.IsUserMember(user);

                            // show logged in profile 
                            if (user.isAdmin == 1)
                            {
                                // this login area is for members/non-members only, not admin 
                                user.ActionType = Models.User.ActionTypes.LoginFailed;
                            }
                            else
                            {
                                if (user.isMember == 0)
                                {
                                    // user is not a member, so send them to non-member interface
                                    return RedirectToAction("NonMember");
                                }
                                else
                                {
                                    // user is a member, so send them to the member interface
                                    return RedirectToAction("Member");
                                }
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

        public ActionResult AdminLogin()
        {
            User u = new User();
            return View(u);
        }

        [HttpPost]
        public ActionResult AdminLogin(FormCollection col)
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
                            if (user.isAdmin == 1)
                            {
                                // user is not null and is not 0 so we can save the current user session 
                                user.SaveUserSession();

                                // user is an admin so send them to the admin interface 
                                return RedirectToAction("Admin");
                            }
                            else
                            {
                                user.ActionType = Models.User.ActionTypes.LoginFailed;
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

        public ActionResult NonMember()
        {
            Models.User user = new Models.User();
            user = user.GetUserSession();
            if (user.IsAuthenticated)
            {
                ViewBag.Name = user.FirstName + " " + user.LastName;
            }
            return View(user);
        }

        public ActionResult Member()
        {
            Models.User user = new Models.User();
            user = user.GetUserSession();
            if (user.IsAuthenticated)
            {
                ViewBag.Name = user.FirstName + " " + user.LastName;
            }
            return View(user);
        }

        public ActionResult Admin()
        {
            // get current user to pass to the view 
            Models.User u = new Models.User();
            u = u.GetUserSession();

            Database db = new Database();
            // get list of company IDs and names, and add to viewbag
            ViewBag.Companies = db.GetCompanies();

            return View(u);
        }

        [HttpPost]
        public ActionResult Admin(FormCollection col)
        {
            if (col["btnSubmit"].ToString() == "editMainBanner")
            {
                return RedirectToAction("EditMainBanner", "Profile");
            }

            return View();
        }

        public ActionResult EditMainBanner()
        {
            // initialize variable(s)/create new object(s)
            ViewBag.isAdmin = false;
            User u = new User();

            // get current session of user
            ViewBag.isAdmin = u.GetAdminStatus();

            // create new database object
            Database db = new Database();

            // get main banner list (bannerID, banner string)
            ViewBag.Banners = db.GetMainBanners();

            return View();
        }

        //    [HttpPost]
        //    public ActionResult EditMainBanner(FormCollection col)
        //    {
        //        // submit button pressed
        //        if (col["btnSubmit"].ToString() == "submitNewBanner")
        //        {
        //            // INSERT NEW BANNER TO DATABASE
        //        }

        //        // cancel button pressed
        //        //if (col["btnSubmit"].ToString() == "cancel")
        //        //{
        //        //    return RedirectToAction("Admin", "Profile");
        //        //}

        //        //return View();
        //   // }

        //}
    }
}