using GCRBA.Models;
using GCRBA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCRBA.Controllers
{
    public class ProfileController : Controller
    {
   
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
           // u = u.GetUserSession();
            return View(u);
        }

        [HttpPost]
        public ActionResult Login(FormCollection col)
        {
            try
            {
                // create instance of user object to pass to the view 
                Models.User user = new Models.User();

                // has submit button with value login been pressed?
                if (col["btnSubmit"] == "login")
                {
                    // yes, assign Username and Password values to Username and Password properties in User object
                    user.Username = col["Username"];
                    user.Password = col["Password"];
                    
                    // are input fields empty? 
                    if (user.Username.Length == 0 || user.Password.Length == 0)
                    {
                        // yes, change User ActionType and return View with User object as argument 
                        user.ActionType = Models.User.ActionTypes.RequiredFieldMissing;
                        return View(user);
                    }
                   
                    // call Login method on User object
                    // method will either return a User object or null
                    user = user.NonAdminLogin();

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
               
                // redirect to AddNewUser form if signup clicked
                else if(col["btnSubmit"] == "signup")
                {
                    return RedirectToAction("AddNewUser","User");
                }

                return View(user);
                
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
            u = u.GetUserSession();
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
                        user = user.AdminLogin();

                        if (user != null && user.UID > 0)
                        {
                            if (user.isAdmin == 1)
                            {
                                // user is not null and is not 0 so we can save the current user session 
                                user.SaveUserSession();

                                // user is an admin so send them to the admin interface 
                                return RedirectToAction("Index", "AdminPortal");
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

        [HttpPost]
        public ActionResult NonMember(FormCollection col)
        {
            Models.User user = new Models.User();
            user = user.GetUserSession();
            
            if (col["btnSubmit"] == "join")
            {
                return RedirectToAction("AddNewMember", "User");
            }

            if (col["btnSubmit"] == "home")
            {
                return RedirectToAction("Index", "Home");
            }

            if (user.IsAuthenticated)
            { 
                ViewBag.Name = user.FirstName + " " + user.LastName; 
            }
            return View();
        }

        public ActionResult Member() {
            // create user object 
            User user = new User();

            // get current user session 
            user = user.GetUserSession();

            return View(user);
        }

        [HttpPost]
        public ActionResult Member(FormCollection col) {
            
            if (col["btnSubmit"].ToString() == "editProfile")
			{
                return RedirectToAction("EditProfile", "Profile");
			}

            if (col["btnSubmit"].ToString() == "editCompanyInfo")
			{
                return RedirectToAction("EditCompanyInfo", "Profile");
			}

            if (col["btnSubmit"].ToString() == "editLandingPage")
			{
                return RedirectToAction("EditLandingPage", "Profile");
			}

            return View();
        }
        
        public ActionResult EditProfile(FormCollection col)
		{
            // initialize MemberVM
            // - create user object + get current user session 
            MemberVM vm = InitMemberVM();

            // get list of states 
            vm = GetStates(vm);

            return View(vm);
		}

        private MemberVM InitMemberVM()
		{
            // create MemberVM object
            MemberVM vm = new MemberVM();

            // create new user object 
            // then get current user session
            vm.User = new User();
            vm.User = vm.User.GetUserSession();

            return vm;
		}

        private MemberVM GetStates(MemberVM vm)
		{
            try
			{
                // create database object
                Database db = new Database();

                // create states object 
                // then get list of states from database
                vm.States = new List<State>();
                vm.States = db.GetStates();

                return vm;
			}
            catch (Exception ex) { throw new Exception(ex.Message); }
		}

        public ActionResult Logout()
        {
            // create user object
            User u = new User();

            // remove current session, which "logs them out"
            u.RemoveUserSession();

            // redirect to main page
            return RedirectToAction("Index", "Home");
        }

    }
}