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
            u = u.GetUserSession();
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

        [HttpPost]
        public ActionResult NonMember(FormCollection col)
        {
            Models.User user = new Models.User();
            user = user.GetUserSession();
            if (user.IsAuthenticated)
            {
                ViewBag.Name = user.FirstName + " " + user.LastName;
                if (col["btnSubmit"].ToString() == "join")
                {
                    return RedirectToAction("AddNewMember", "User");
                }

            }
            return View();
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
            User u = new User();
            u = u.GetUserSession();
           
            return View(u);
        }

        [HttpPost]
        public ActionResult Admin(FormCollection col)
        {
            if (col["btnSubmit"].ToString() == "editMainBanner")
            {
                return RedirectToAction("EditMainBanner", "Profile");
            }

            if (col["btnSubmit"].ToString() == "editCompanies")
            {
                return RedirectToAction("EditCompanies", "Profile");
            }

            return View();
        }

        public ActionResult EditMainBanner()
        {
            // create view model object 
            AdminBannerViewModel vm = new AdminBannerViewModel();

            // create user objects and populate 
            vm.CurrentUser = new User();

            // get admin status because page should only be viewable by admin
            vm.CurrentUser = vm.CurrentUser.GetUserSession();

            // create new database object
            Database db = new Database();

            // create list to hold banners 
            List<MainBanner> listOfMainBanners = new List<MainBanner>();

            // add previous + current banners to list 
            listOfMainBanners = db.GetMainBanners();

            // add list of banners to view model 
            vm.MainBanners = listOfMainBanners;

            // return view with view model object passed as argument so we can access it in view
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditMainBanner(FormCollection col)
        {
            // create view model object so that we can show data from more than one
            // model in the view 
            AdminBannerViewModel vm = new AdminBannerViewModel();
            
            // create view model user object 
            vm.CurrentUser = new User();

            // is user logged in?
            vm.CurrentUser = vm.CurrentUser.GetUserSession();
            
            // get current main banner 
            vm.MainBanner = new MainBanner();

            // create variable to hold new banner if that option is chosen 
            Database db = new Database();

            // set default to 0
            ViewBag.Flag = 0;

            // create list to hold banners 
            List<MainBanner> listOfMainBanners = new List<MainBanner>();

            // add previous + current banners to list 
            listOfMainBanners = db.GetMainBanners();

            // add list of banners to view model 
            vm.MainBanners = listOfMainBanners;

            // return to main admin portal if user selects cancel  button 
            if (col["btnSubmit"].ToString() == "cancel")
            {
                return RedirectToAction("Admin", "Profile");
            }

            // button to submit new banner is selected 
            if (col["btnSubmit"].ToString() == "submitNewBanner")
            {
                // drop down option with value = "new" selected 
                if (col["mainBanners"].ToString() == "new")
                {
                    // add text from textarea to view model's banner property
                    vm.MainBanner.Banner = col["newBanner"];

                    // try to add new banner to database
                    if (db.InsertNewMainBanner(vm.MainBanner) == true)
                    {
                        // banner successfully added, use this flag so we know what to show on view
                        // 0 - unsuccessful
                        // 1 - successful
                        ViewBag.Flag = 1;
                    }
                    // return view with view model as argument 
                    return View(vm);                   
                } 
                // one of the previous banners in the drop down selected to use for new banner
                else
                {
                    // set view model's BannerID property to value (ID from database) of selected option
                    vm.MainBanner.BannerID = Convert.ToInt16(col["mainBanners"].ToString());

                    // get banner text from list of banners
                    vm.MainBanner.Banner = listOfMainBanners[vm.MainBanner.BannerID - 1].Banner;

                    // try to add banner to newest row in table in db 
                    if (db.InsertNewMainBanner(vm.MainBanner) == true)
                    {
                        // banner successfully added, use this flag so we know what to show on view 
                        // 0 - unsuccessful
                        // 1 - successful
                        ViewBag.Flag = 1;
                    }
                    // return view with view model as argument 
                    return View(vm);
                }
            }
            return View(vm);
        }

        public ActionResult EditCompanies()
        {
            // create new user object
            User u = new User();

            // get current user session 
            u = u.GetUserSession();

            // return view 
            return View(u);
        }

        [HttpPost]
        public ActionResult EditCompanies(FormCollection col)
        {
            // create new user object
            User u = new User();

            // get current user session
            u = u.GetUserSession();

            if (col["btnSubmit"].ToString() == "addCompany")
            {
                return RedirectToAction("AddCompany", "Profile");
            }

            if (col["btnSubmit"].ToString() == "deleteCompany")
            {
                return RedirectToAction("DeleteCompany", "Profile/Admin");
            }

            if (col["btnSubmit"].ToString() == "editCompany")
            {
                return RedirectToAction("EditExistingCompany", "Profile");
            }

            return View(u);

        }

        public ActionResult AddCompany()
        {
            // create object of view model
            EditCompaniesViewModel vm = new EditCompaniesViewModel();

            // create new user object with vm 
            vm.CurrentUser = new User();

            // get current user session
            vm.CurrentUser = vm.CurrentUser.GetUserSession();

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddCompany(FormCollection col)
        {
            // create objects of what we will use 
            EditCompaniesViewModel vm = new EditCompaniesViewModel();
            vm.CurrentUser = new User();
            vm.CurrentCompany = new Company();
            Database db = new Database();

            // get current user session
            vm.CurrentUser = vm.CurrentUser.GetUserSession();

            // get input from form 
            if (col["btnSubmit"].ToString() == "submit")
            {
                vm.CurrentCompany.Name = col["CurrentCompany.Name"];
                vm.CurrentCompany.About = col["CurrentCompany.About"];
                vm.CurrentCompany.Year = col["CurrentCompany.Year"];
            }

            // add to database
            vm.CurrentCompany.ActionType = vm.CurrentCompany.Save();

            return RedirectToAction("Admin", "Profile"); 
        }

        public ActionResult DeleteCompany()
        {
            return View();
        }

        public ActionResult EditExistingCompany()
        {
            return View();
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