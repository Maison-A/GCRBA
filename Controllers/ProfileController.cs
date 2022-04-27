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

        // -------------------------------------------------------------------------------------------------
        // ACTIONRESULT METHODS  
        // -------------------------------------------------------------------------------------------------


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
            // create user object
            User u = new User();

            // get current user session
            u = u.GetUserSession();

            // get notification(s)
            u.Notifications = new List<Notification>();
            u.Notifications = GetUserNotifications(u);

            // create user notification object
            u.Notification = new Notification();

            // check if any of the messages are unread 
            u.Notification.UnreadNotifications = GetIfUnread(u);

            return View(u);
        }

        [HttpPost]
        public ActionResult NonMember(FormCollection col)
        {
            // create user object
            User u = new User();

            // get current user session
            u = u.GetUserSession();

            // get notification(s)
            u.Notifications = new List<Notification>();
            u.Notifications = GetUserNotifications(u);

            // create user notification object 
            u.Notification = new Notification();

            if (col["btnSubmit"].ToString() == "editProfile")
			{
                return RedirectToAction("EditProfile", "Profile");
			}

            if (col["btnSubmit"].ToString() == "viewNotifications")
			{
                return RedirectToAction("UserNotifications", "Profile");
			}

            return View(u);
        }

        public ActionResult UserNotifications()
		{
            // create user object
            // then get current user status
            User u = new User();
            u = u.GetUserSession();

            // get notification(s)
            u.Notifications = new List<Notification>();
            u.Notifications = GetUserNotifications(u);

            u.Notification = new Notification();

            return View(u);
        }

        [HttpPost]
        public ActionResult UserNotifications(FormCollection col)
		{
            // create user object
            // then get current user status
            User u = new User();
            u = u.GetUserSession();

            // get notification(s)
            u.Notifications = new List<Notification>();
            u.Notifications = GetUserNotifications(u);

            string notificationIDs;

            if (col["btnSubmit"].ToString() == "delete")
			{
                // get list of messages selected 
                // then delete from db and return view 
                notificationIDs = col["notification"];
                u.ActionType = DeleteNotifications(u, notificationIDs);

                // get updated list of user notifications 
                u.Notifications = GetUserNotifications(u);

                return View(u);
			}

            if (col["btnSubmit"].ToString() == "markAsRead")
			{
                // get list of messages selected 
                // then update status as read in db and return view 
                notificationIDs = col["notification"];
                u.ActionType = UpdateNotificationStatus(u, notificationIDs);

                // get updated list of user notifications 
                u.Notifications = GetUserNotifications(u);

                return View(u);
			}

            return View(u);
        }

        public ActionResult Member() {
            // create user object 
            User u = new User();

            // get current user session 
            u = u.GetUserSession();

            // get notification(s)
            u.Notifications = new List<Notification>();
            u.Notifications = GetUserNotifications(u);

            // create user notification object
            u.Notification = new Notification();

            // check if any messages are unread
            u.Notification.UnreadNotifications = GetIfUnread(u);

            return View(u);
        }

        [HttpPost]
        public ActionResult Member(FormCollection col) {

            // create user object 
            User u = new User();

            // get current user session 
            u = u.GetUserSession();

            // create user notification object
            u.Notification = new Notification();

            if (col["btnSubmit"].ToString() == "viewNotifications")
			{
                return RedirectToAction("UserNotifications", "Profile");
			}
            
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

            return View(u);
        }
        
        public ActionResult EditProfile()
		{
            // initialize MemberVM
            // - create user object + get current user session 
            ProfileViewModel vm = InitProfileViewModel();

            // get list of states 
            vm = GetStates(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditProfile(FormCollection col)
		{
            // initialize MemberVM
            // - create user object + get current user session 
            ProfileViewModel vm = InitProfileViewModel();

            // get list of states 
            vm = GetStates(vm);

            if (col["btnSubmit"].ToString() == "submit")
			{
                // declare variable to temporarily hold password while we validate it 
                string tempPassword = col["User.Password"];

                // if new password is different than current password, check if new password was 
                // re-entered in second input 
                if (tempPassword != vm.User.Password)
                {
                    if (col["password2"].ToString() != tempPassword)
                    {
                        // show error message that passwords must match 
                        vm.User.ActionType = Models.User.ActionTypes.RequiredFieldMissing;
                        return View(vm);
                    } else
                    {
                        // change class property values to updated changes 
                        vm.User.FirstName = col["User.FirstName"];
                        vm.User.LastName = col["User.LastName"];
                        vm.User.Address = col["User.Address"];
                        vm.User.City = col["User.City"];
                        vm.User.intState = Convert.ToInt16(col["states"]);
                        vm.User.Zip = col["User.Zip"];
                        vm.User.Phone = col["User.Phone"];
                        vm.User.Email = col["User.Email"];
                        vm.User.Password = tempPassword;

                        // submit to db 
                        vm.User.ActionType = UpdateUser(vm);

                        return View(vm);
                    }
                } 
                else
				{
                    // get input 
                    vm.User.FirstName = col["User.FirstName"];
                    vm.User.LastName = col["User.LastName"];
                    vm.User.Address = col["User.Address"];
                    vm.User.City = col["User.City"];
                    vm.User.intState = Convert.ToInt16(col["states"]);
                    vm.User.Zip = col["User.Zip"];
                    vm.User.Phone = col["User.Phone"];
                    vm.User.Email = col["User.Email"];

                    // submit to db 
                    vm.User.ActionType = UpdateUser(vm);

                    return View(vm);
                }
            }

            return View(vm);
		}

        public ActionResult EditCompanyInfo()
		{
            // initialize ProfileVM which also creates instance of user object 
            ProfileViewModel vm = InitProfileViewModel();

            // get current company session
            vm.Company = GetCompany(vm);

            return View(vm);

		}

        [HttpPost]
        public ActionResult EditCompanyInfo(FormCollection col)
		{
            ProfileViewModel vm = InitProfileViewModel();

            // get current company session
            vm.Company = GetCompany(vm);

            if (col["btnSubmit"].ToString() == "addLocation")
			{
                return RedirectToAction("AddNewLocation", "Bakery");
			}

            if (col["btnSubmit"].ToString() == "editCompany")
			{
                return RedirectToAction("EditExistingCompany", "Profile");
			}

            return View(vm);
		}

        public ActionResult EditExistingCompany()
		{
            ProfileViewModel vm = InitProfileViewModel();

            // create company object
            vm.Company = new Company();

            // get company associated with member 
            vm.Company = GetCompany(vm);

            return View(vm);

		}

		[HttpPost]
		public ActionResult EditExistingCompany(FormCollection col)
		{
			ProfileViewModel vm = InitProfileViewModel();

            // create company object 
            vm.Company = new Company();

            // get company associated with user 
            vm.Company = GetCompany(vm);

            int editedColumn = 0;

			if (col["btnSubmit"].ToString() == "submit")
			{
                if (col["Company.Name"] != vm.Company.Name)
                {
                    // get previous value so we can show admin before and after edit 
                    string previousVersion = vm.Company.Name;

                    vm.Company.Name = col["Company.Name"];

                    // 1 is PK for changeType of "strCompanyName" in tblChangeType
                    editedColumn = 1;

                    // send notification to admin about change 
                    SendEditNotificationToAdmin(vm.User, editedColumn, previousVersion, vm.Company.Name);
                }
                else
				{
                    vm.Company.Name = null;
				}

                if (col["Company.About"] != vm.Company.About)
				{
                    // get previous value so we can show admin before and after edit 
                    string previousVersion = vm.Company.About;

                    vm.Company.About = col["Company.About"];

                    // 2 is PK for changeType of "strAbout" in tblChangeType
                    editedColumn = 2;

                    // send notification to admin about change
                    SendEditNotificationToAdmin(vm.User, editedColumn, previousVersion, vm.Company.About);
                }
                else
				{
                    vm.Company.About = null;
                }

                if (col["Company.Year"] != vm.Company.Year)
				{
                    // get previous value so we can show admin before and after edit 
                    string previousVersion = vm.Company.Year;

                    vm.Company.Year = col["Company.Year"];

                    // 3 is PK for changeType of "strBizYear" in tblChangeType
                    editedColumn = 3;

                    // send notification to admin about change
                    SendEditNotificationToAdmin(vm.User, editedColumn, previousVersion, vm.Company.Year);

                }
                else
				{
                    vm.Company.Year = null;
				}

                // update company info in database 
                UpdateCompanyInfo(vm.Company);

                // get updated company info 
                vm.Company = GetCompany(vm);

                // set actiontype to updatesuccessful
                vm.Company.ActionType = Company.ActionTypes.UpdateSuccessful;

                return View(vm);
			}

            if (col["btnSubmit"].ToString() == "cancel")
			{
                return RedirectToAction("EditCompanyInfo", "Profile");
			}

            return View(vm);
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


        // -------------------------------------------------------------------------------------------------
        // ADDING/DELETING FROM DATABASE   
        // -------------------------------------------------------------------------------------------------

        private void SendEditNotificationToAdmin(User u, int editedColumnID, string previousVersion, string newVersion)
        {
            try
            {
                // create database object
                Database db = new Database();

                // send to admin 
                db.InsertAdminNotificationCompanyEdit(u, editedColumnID, previousVersion, newVersion);
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void UpdateCompanyInfo(Company c)
        {
            try
            {
                // create database object
                Database db = new Database();

                // send update to db 
                db.UpdateCompanyInfo(c);
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private User.ActionTypes UpdateUser(ProfileViewModel vm)
        {
            try
            {
                // create database object 
                Database db = new Database();

                // submit to db 
                vm.User.ActionType = db.UpdateUser(vm);

                // return actiontype
                return vm.User.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void DeleteUserNotification(User u)
        {
            try
            {
                // create database object
                Database db = new Database();

                // delete record from db 
                db.DeleteNotification(u);
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }


        // -------------------------------------------------------------------------------------------------
        // RETRIEVING DATA FROM DATABASE   
        // -------------------------------------------------------------------------------------------------

        private Company GetCompany(ProfileViewModel vm)
        {
            try
            {
                // create database object 
                Database db = new Database();

                // create new vm company object
                vm.Company = new Company();

                // get company based on memberID 
                vm.Company = db.GetCompanyByMember(vm);

                return vm.Company;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private ProfileViewModel GetStates(ProfileViewModel vm)
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
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }


        // -------------------------------------------------------------------------------------------------
        // INITIALIZING COMMONLY USED CLASSES  
        // -------------------------------------------------------------------------------------------------

        private ProfileViewModel InitProfileViewModel()
        {
            // create MemberVM object
            ProfileViewModel vm = new ProfileViewModel();

            // create new user object 
            // then get current user session
            vm.User = new User();
            vm.User = vm.User.GetUserSession();

            return vm;
        }

        private bool GetIfUnread(User u)
        {
            try
            {
                int count = 0;

                for (int i = 0; i < u.Notifications.Count; i++)
                {
                    if (u.Notifications[i].NotificationStatusID == 2)
                    {
                        count += 1;
                    }
                }

                if (count > 0)
                {
                    u.Notification.UnreadNotifications = true;
                }

                return u.Notification.UnreadNotifications;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<Notification> GetUserNotifications(User u)
        {
            try
            {
                // create database object
                Database db = new Database();

                // get user notifications 
                u.Notifications = db.GetUserNotifications(u);

                return u.Notifications;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private User.ActionTypes UpdateNotificationStatus(User u, string notificationIDs)
        {
            try
            {
                // create database object
                Database db = new Database();

                // create array by splitting string at each comma 
                string[] Notifications = notificationIDs.Split(',');

                // create user notification object 
                u.Notification = new Notification();

                // loop through array and update in db 
                foreach (string item in Notifications)
                {
                    u.Notification.NotificationID = int.Parse(item);
                    u.ActionType = db.UpdateNotificationStatus(u);
                }

                return u.ActionType;

            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private User.ActionTypes DeleteNotifications(User u, string notificationIDs)
        {
            try
            {
                // create database object
                Database db = new Database();

                // create array by splitting string at each comma 
                string[] Notifications = notificationIDs.Split(',');

                // create user notification object 
                u.Notification = new Notification();

                // loop through array and delete from db 
                foreach (string item in Notifications)
                {
                    u.Notification.NotificationID = int.Parse(item);
                    u.ActionType = db.DeleteNotification(u);
                }

                return u.ActionType;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}