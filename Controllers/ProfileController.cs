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
                if (col["notification"] != null)
				{
                    // get list of messages selected 
                    // then delete from db and return view 
                    notificationIDs = col["notification"];
                    u.ActionType = DeleteNotifications(u, notificationIDs);

                    // get updated list of user notifications 
                    u.Notifications = GetUserNotifications(u);

                    return View(u);
                }
			}

            if (col["btnSubmit"].ToString() == "markAsRead")
			{
                if (col["notification"] != null)
				{
                    // get list of messages selected 
                    // then update status as read in db and return view 
                    notificationIDs = col["notification"];
                    u.ActionType = UpdateNotificationStatus(u, notificationIDs);

                    // get updated list of user notifications 
                    u.Notifications = GetUserNotifications(u);

                    return View(u);
                }
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

            Models.Database db = new Models.Database();
            u.lstMemberLocations = db.GetMemberLocations(u);

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

        public ActionResult EditLandingPage()
        {
            User u = new User();
            u = u.GetUserSession();
            Models.Database db = new Models.Database();
            List<Models.NewLocation> landingLocations = new List<NewLocation>();
            landingLocations = db.GetMemberLocations(u);
            Models.LandingLocationList landingLocList = new Models.LandingLocationList()
            {
                SelectedLandingLocationRequests = new[] { 1 },
                LandingLocations = GetAllLandingLocations(landingLocations)
            };
            return View(landingLocList);
        }

        public List<SelectListItem> GetAllLandingLocations(List<Models.NewLocation> landingLocations)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Models.NewLocation loc in landingLocations)
            {

                items.Add(new SelectListItem { Text = loc.LocationName, Value = loc.lngLocationID.ToString() });
            }
            return items;
        }

        [HttpPost]
        public ActionResult EditLandingPage(FormCollection col, LandingLocationList loc)
        {
            Models.Database db = new Models.Database();
            Models.LandingLocationList landingLocationList = new Models.LandingLocationList();
            Models.User u = new Models.User();
            u = u.GetUserSession();

            landingLocationList.lstLandingLocations = db.GetMemberLocations(u);
            loc.LandingLocations = GetAllLandingLocations(landingLocationList.lstLandingLocations);
            if (col["btnSubmit"].ToString() == "AddNewPhotos" && loc.SelectedLandingLocationRequests != null)
            {
                List<SelectListItem> selectedItems = loc.LandingLocations.Where(p => loc.SelectedLandingLocationRequests.Contains(int.Parse(p.Value))).ToList();
                foreach (var Request in selectedItems)
                {
                    Request.Selected = true;
                    Models.NewLocation landingLocation = new Models.NewLocation();
                    return RedirectToAction("Index", "Photo", new { @id = Convert.ToInt64(Request.Value) });
                }
            }
            return View();
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

            if (col["btnSubmit"].ToString() == "editLocation")
			{
                return RedirectToAction("EditExistingLocation", "Profile");
			}

            if (col["btnSubmit"].ToString() == "deleteLocation")
			{
                return RedirectToAction("DeleteLocation", "Profile");
			}

            if (col["btnSubmit"].ToString() == "editCategories")
            {
                return RedirectToAction("EditCategoriesByLocation", "Profile");
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

                    // send notification to admin about change 
                    SendEditNotification(vm.User, 1, 1, previousVersion, vm.Company.Name);
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

                    // send notification to admin about change
                    SendEditNotification(vm.User, 2, 1, previousVersion, vm.Company.About);
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

                    // send notification to admin about change
                    SendEditNotification(vm.User, 3, 1, previousVersion, vm.Company.Year);

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

        public ActionResult EditWebsites()
		{
            ProfileViewModel vm = InitProfileViewModel();
            vm.Websites = new List<Website>();
            vm.Website = new Website();
            vm.Button = new Button();
            vm.Company = new Company();

            return View(vm);
		}

        private void SaveButtonSession(string buttonValue)
        {
            try
            {
                // create button object 
                Button button = new Button();

                // get value of button pressed 
                button.CurrentButton = buttonValue;

                // save button session 
                button.SaveButtonSession();
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        [HttpPost]
        public ActionResult EditWebsites(FormCollection col)
		{
            ProfileViewModel vm = InitProfileViewModel();
            vm.Company = new Company();
            vm.Company = GetCompany(vm);

            int editedColumn = 0;

            // create website list object then get list of websites 
            vm.Websites = new List<Website>();

            vm.Website = new Website();

            // get current session of company websites associated with member
            // if none, will be null
            vm.Websites = vm.Website.GetWebsitesSession();

            // create button object so we can track which button has been clicked
            vm.Button = new Button();

            // get current button session -- if none, will return null button object
            vm.Button = vm.Button.GetButtonSession();

            if (col["btnSubmit"].ToString() == "editExistingWebsites")
			{
                // remove current button session so we can set it as this one 
                vm.Button.RemoveButtonSession();

                // set button to "edit" so we know what form input to show 
                vm.Button.CurrentButton = "edit";

                // save new button session 
                vm.Button.SaveButtonSession();

                // remove current session of websites if not null
                vm.Website.RemoveWebsitesSession();
                
                // get current list of websites associated with member 
                vm.Websites = GetCompanyWebsites(vm);

                // save websites list in session 
                vm.Website.SaveWebsitesSession(vm.Websites);

                return View(vm);
            }

            if (col["btnSubmit"].ToString() == "addNewWebsites")
			{
                // remove current button session so we can set it as this one 
                vm.Button.RemoveButtonSession();

                vm.Button.CurrentButton = "add";

                // set button to "add" so we know what form input to show 
                vm.Button.SaveButtonSession();

                // save new button session 
                vm.Button.SaveButtonSession();

                // remove current session of websites if not null
                vm.Website.RemoveWebsitesSession();

                // get current list of website types
                vm.Websites = GetWebsiteTypes(vm);

                // save websites list in session 
                vm.Website.SaveWebsitesSession(vm.Websites);

                return View(vm);
			}

            // see if user clicked delete button for existing company website 
            int test = 0;
            string input = col["btnSubmit"].ToString();
            bool result = int.TryParse(input, out test);

            // each delete button is the intWebsiteID for the corresponding website
            // if one is clicked, result will be true 
            // if true, delete record from db 
            if (result == true)
			{
                // get ID
                vm.Website.intWebsiteID = int.Parse(input);

                // delete from db 
                vm.Website.ActionType = DeleteWebsite(vm.Website);

                return View(vm);
			}

            if (col["btnSubmit"].ToString() == "submit")
			{
                string previousVersion = "";

              if (vm.Button.CurrentButton == "edit")
				{
                    // iterate through list of websites 
                    foreach (var item in vm.Websites)
                    {
                        // is current item's website type is equal to Main?
                        if (item.strWebsiteType == "Main")
                        {
                            // match 
                            // are the URLs the same?
                            if (col["Main"].ToString() != item.strURL)
                            {
                                if (col["Main"].ToString() != "")
								{
                                    // get previous version to send in admin notification 
                                    previousVersion = item.strURL;

                                    // no, update object URL
                                    item.strURL = col["Main"].ToString();

                                    // update new URL in datbase
                                    vm.Website.ActionType = UpdateWebsite(item);

                                    // notify admin of change 
                                    SendEditNotification(vm.User, 5, 2, previousVersion, item.strURL);
                                }
                            }
                        }

                        // is current item's website type is equal to Main?
                        if (item.strWebsiteType == "Kettle")
                        {
                            // match 
                            // are the URLs the same?
                            if (col["Kettle"].ToString() != item.strURL)
                            {
                               if (col["Kettle"].ToString() != "")
								{
                                    // get previous version to send in notification to admin 
                                    previousVersion = item.strURL;

                                    // no, update object URL
                                    item.strURL = col["Kettle"].ToString();

                                    // update new URL in datbase
                                    vm.Website.ActionType = UpdateWebsite(item);

                                    // notify admin of change 
                                    SendEditNotification(vm.User, 5, 2, previousVersion, item.strURL);
                                    
                                }
                            }
                        }

                        // is current item's website type is equal to Main?
                        if (item.strWebsiteType == "Ordering")
                        {
                            // match 
                            // are the URLs the same?
                            if (col["Ordering"].ToString() != item.strURL)
                            {             
                               if (col["Ordering"].ToString() != "")
								{
                                    // get previous version to send in notification to admin 
                                    previousVersion = item.strURL;

                                    // no, update object URL
                                    item.strURL = col["Ordering"].ToString();

                                    // update new URL in datbase
                                    vm.Website.ActionType = UpdateWebsite(item);

                                    // notify admin of changes
                                    SendEditNotification(vm.User, 5, 2, previousVersion, item.strURL);
                                }
                            }
                        }
                    }
                    return View(vm);
                }

                if (vm.Button.CurrentButton == "add")
                {
                    if (Convert.ToInt16(col["websiteTypes"]) > 0)
                    {
                        if (col["newWebsite"].ToString() != "")
						{
                            // get input for new url
                            vm.Website.strURL = col["newWebsite"];
                            vm.Website.intWebsiteTypeID = Convert.ToInt16(col["websiteTypes"]);
                            vm.Website.ActionType = AddWebsite(vm);

                            // notify admin of change
                            SendEditNotification(vm.User, 5, 2, "N/A", vm.Website.strURL);
                            return View(vm);
                        }
                    }
                }
            }


            return View(vm);
        }

        public ActionResult EditSocialMedia()
		{
            ProfileViewModel vm = InitProfileViewModel();
            vm.SocialMediaList = new List<SocialMedia>();
            vm.SocialMedia = new SocialMedia();
            vm.Button = new Button();
            vm.Company = new Company();

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditSocialMedia(FormCollection col)
		{
            ProfileViewModel vm = InitProfileViewModel();
            vm.Company = new Company();
            vm.Company = GetCompany(vm);

            int editedColumn = 0;

            // create list to hold social media links
            vm.SocialMediaList = new List<SocialMedia>();

            vm.SocialMedia = new SocialMedia();

            // get current session of company social media links -- if none, will return null object
            vm.SocialMediaList = vm.SocialMedia.GetSocialMediaListSession();

            // create button object so we can track which button has been pressed most recently 
            vm.Button = new Button();

            // get current button session
            vm.Button = vm.Button.GetButtonSession();

            if (col["btnSubmit"].ToString() == "editExistingSocialMedia")
			{
                // remove current button session so we can set it as this one 
                vm.Button.RemoveButtonSession();

                // set button to edit so we know what form input to show 
                vm.Button.CurrentButton = "edit";

                // save new button session
                vm.Button.SaveButtonSession();

                // remove current list of social media 
                vm.SocialMedia.RemoveSocialMediaListSession();

                // get current list of social media
                vm.SocialMediaList = GetCompanySocialMedia(vm);

                // save list of social media 
                vm.SocialMedia.SaveSocialMediaListSession(vm.SocialMediaList);

                return View(vm);
			}

            if (col["btnSubmit"].ToString() == "addNewSocialMedia")
			{
                vm.Button.RemoveButtonSession();

                vm.Button.CurrentButton = "add";

                // save new button session 
                vm.Button.SaveButtonSession();

                // remove current session of websites if not null
                vm.SocialMedia.RemoveSocialMediaListSession();

                // get current list of website types
                vm.SocialMediaList = GetSocialMediaTypes(vm);

                // save websites list in session 
                vm.SocialMedia.SaveSocialMediaListSession(vm.SocialMediaList);

                return View(vm);
            }

            // DEAL WITH DELETING SOCIAL MEDIA HERE
            //----------------------------------------- 

            if (col["btnSubmit"].ToString() == "submit")
			{
                string previousVersion = "";

                if (vm.Button.CurrentButton == "edit")
				{
                    // iterate through list of social media 
                    foreach (var item in vm.SocialMediaList)
					{
                        // is current item's social media type equal to Facebook
                        if (item.strPlatform == "Facebook")
						{
                            // yes, the types match
                            // are the links the same?
                            if (col["Facebook"].ToString() != item.strSocialMediaLink)
							{
                                // no, is input empty?
                                if (col["Facebook"].ToString() != "")
								{
                                    // no, get previous version so we can send before and after to admin 
                                    previousVersion = item.strSocialMediaLink;

                                    // update object to new link from input 
                                    item.strSocialMediaLink = col["Facebook"].ToString();

                                    // update new link in db 
                                    vm.SocialMedia.ActionType = UpdateSocialMedia(item);

                                    // 7 is PK for strSocialMediaLink in tblCompanySocialMedia 
                                    editedColumn = 7;

                                    // notify admin of changes
                                    SendEditNotification(vm.User, 7, 3, previousVersion, item.strSocialMediaLink);
								}
							}
						}

                        if (item.strPlatform == "Instagram")
                        {
                            // yes, the types match
                            // are the links the same?
                            if (col["Instagram"].ToString() != item.strSocialMediaLink)
                            {
                                // no, is input empty?
                                if (col["Instagram"].ToString() != "")
                                {
                                    // no, get previous version so we can send before and after to admin 
                                    previousVersion = item.strSocialMediaLink;

                                    // update object to new link from input 
                                    item.strSocialMediaLink = col["Instagram"].ToString();

                                    // update new link in db 
                                    vm.SocialMedia.ActionType = UpdateSocialMedia(item);

                                    // notify admin of changes
                                    SendEditNotification(vm.User, 7, 3, previousVersion, item.strSocialMediaLink);
                                }
                            }
                        }

                        if (item.strPlatform == "Snapchat")
                        {
                            // yes, the types match
                            // are the links the same?
                            if (col["Snapchat"].ToString() != item.strSocialMediaLink)
                            {
                                // no, is input empty?
                                if (col["Snapchat"].ToString() != "")
                                {
                                    // no, get previous version so we can send before and after to admin 
                                    previousVersion = item.strSocialMediaLink;

                                    // update object to new link from input 
                                    item.strSocialMediaLink = col["Snapchat"].ToString();

                                    // update new link in db 
                                    vm.SocialMedia.ActionType = UpdateSocialMedia(item);

                                    // notify admin of changes
                                    SendEditNotification(vm.User, 7, 3, previousVersion, item.strSocialMediaLink);
                                }
                            }
                        }

                        if (item.strPlatform == "TikTok")
                        {
                            // yes, the types match
                            // are the links the same?
                            if (col["TikTok"].ToString() != item.strSocialMediaLink)
                            {
                                // no, is input empty?
                                if (col["TikTok"].ToString() != "")
                                {
                                    // no, get previous version so we can send before and after to admin 
                                    previousVersion = item.strSocialMediaLink;

                                    // update object to new link from input 
                                    item.strSocialMediaLink = col["TikTok"].ToString();

                                    // update new link in db 
                                    vm.SocialMedia.ActionType = UpdateSocialMedia(item);

                                    // notify admin of changes
                                    SendEditNotification(vm.User, 7, 3, previousVersion, item.strSocialMediaLink);
                                }
                            }
                        }

                        if (item.strPlatform == "Twitter")
                        {
                            // yes, the types match
                            // are the links the same?
                            if (col["Twitter"].ToString() != item.strSocialMediaLink)
                            {
                                // no, is input empty?
                                if (col["Twitter"].ToString() != "")
                                {
                                    // no, get previous version so we can send before and after to admin 
                                    previousVersion = item.strSocialMediaLink;

                                    // update object to new link from input 
                                    item.strSocialMediaLink = col["Twitter"].ToString();

                                    // update new link in db 
                                    vm.SocialMedia.ActionType = UpdateSocialMedia(item);

                                    // notify admin of changes
                                    SendEditNotification(vm.User, 7, 3, previousVersion, item.strSocialMediaLink);
                                }
                            }
                        }

                        if (item.strPlatform == "Yelp")
                        {
                            // yes, the types match
                            // are the links the same?
                            if (col["Yelp"].ToString() != item.strSocialMediaLink)
                            {
                                // no, is input empty?
                                if (col["Yelp"].ToString() != "")
                                {
                                    // no, get previous version so we can send before and after to admin 
                                    previousVersion = item.strSocialMediaLink;

                                    // update object to new link from input 
                                    item.strSocialMediaLink = col["Yelp"].ToString();

                                    // notify admin of changes
                                    SendEditNotification(vm.User, 7, 3, previousVersion, item.strSocialMediaLink);
                                }
                            }
                        }
                    }
                    return View(vm);
				}

                if (vm.Button.CurrentButton == "add")
				{
                    if (Convert.ToInt16(col["platforms"]) > 0)
					{
                        if (col["newSocialMedia"].ToString() != "")
						{
                            // get input for new link
                            vm.SocialMedia.strSocialMediaLink = col["newSocialMedia"];
                            vm.SocialMedia.intSocialMediaID = Convert.ToInt16(col["platforms"]);
                            vm.SocialMedia.ActionType = AddSocialMedia(vm);

                            // notify admin of change 
                            SendEditNotification(vm.User, 7, 3, "N/A", vm.SocialMedia.strSocialMediaLink);
                            return View(vm);
						}
					}
 				}
			}
            return View(vm);
		}

        public ActionResult EditExistingLocation()
		{
            ProfileViewModel vm = InitProfileViewModel();

            vm.Company = new Company();

            vm.Company = GetCompany(vm);

            vm.Location = new Location();

            vm.Locations = new List<Location>();

            vm.Locations = GetLocations(vm);

            vm.States = new List<State>();

            vm = GetStates(vm);

            return View(vm);
		}

        [HttpPost]
        public ActionResult EditExistingLocation(FormCollection col)
		{
            ProfileViewModel vm = InitProfileViewModel();

            vm.Company = new Company();

            vm.Company = GetCompany(vm);

            vm.Location = new Location();

            // get current location session
            vm.Location = vm.Location.GetLocationSession();

            vm.Locations = new List<Location>();

            // get list of locations 
            vm.Locations = GetLocations(vm);

            vm.States = new List<State>();

            vm = GetStates(vm);

            int editedColumn = 0;

            if (col["btnSubmit"].ToString() == "editLocation")
			{
                // get selected locationID
                vm.Location.LocationID = Convert.ToInt16(col["locations"]);

                // loop through list of locations to get data of location with selected locationID
                for (var i = 0; i < vm.Locations.Count; i++)
				{
                    // do the select ID and current ID in list match?
                    if (vm.Locations[i].LocationID == vm.Location.LocationID)
					{
                        // remove location session if previously one 
                        vm.Location.RemoveLocationSession();

                        // yes, so add Location info to Location object 
                        vm.Location = vm.Locations[i];

                        // save location session 
                        vm.Location.SaveLocationSession();
					}
				}

                return View(vm);
			}

            if (col["btnSubmit"].ToString() == "submit")
			{
                string previousVersion = "";
                int intPreviousStateID = 0;

                if (col["Location.Address"].ToString() != vm.Location.Address)
				{
                    // get previous version to send in notification of change 
                    previousVersion = vm.Location.Address;

                    // get new input
                    vm.Location.Address = col["Location.Address"].ToString();

                    // send update to db 
                    SendEditNotification(vm.User, 8, 4, previousVersion, vm.Location.Address);

				} else
				{
                    vm.Location.Address = null;
				}

                if (col["Location.City"].ToString() != vm.Location.City)
                {
                    // get previous version  
                    previousVersion = vm.Location.City;

                    vm.Location.City = col["Location.City"].ToString();

                    SendEditNotification(vm.User, 9, 4, previousVersion, vm.Location.City);

                } else
                {
                    vm.Location.City = null;
                }

                if (Convert.ToInt16(col["states"]) != vm.Location.intState)
                {
                    intPreviousStateID = vm.Location.intState;

                    vm.Location.intState = Convert.ToInt16(col["states"]);

                    SendEditNotification(vm.User, 10, 4, intPreviousStateID.ToString(), vm.Location.intState.ToString());
                } else
                {
                    vm.Location.intState = 0;
                }

                if (col["Location.Zip"].ToString() != vm.Location.Zip)
                {
                    previousVersion = vm.Location.Zip;

                    vm.Location.Zip = col["Location.Zip"].ToString();

                    SendEditNotification(vm.User, 11, 4, previousVersion, vm.Location.Zip);
                } else
                {
                    vm.Location.Zip = null;
                }

                if (col["Location.Phone"].ToString() != vm.Location.Phone)
                {
                    previousVersion = vm.Location.Phone;

                    vm.Location.Phone = col["Location.Phone"].ToString();

                    SendEditNotification(vm.User, 12, 4, previousVersion, vm.Location.Phone);
                } else
                {
                    vm.Location.Phone = null;
                }

                if (col["Location.Email"].ToString() != vm.Location.Email)
                {
                    previousVersion = vm.Location.Email;

                    vm.Location.Email = col["Location.Email"].ToString();

                    SendEditNotification(vm.User, 13, 4, previousVersion, vm.Location.Email);
                } else
                {
                    vm.Location.Email = null;
                }

                // submit change(s) to database
                vm.Location.ActionType = UpdateLocation(vm.Location);

                // update our info 
                vm.Location = GetUpdatedLocationInfo(vm.Location);

                return View(vm);

            }

            return View(vm);
		}

        public ActionResult DeleteLocation()
		{
            ProfileViewModel vm = InitProfileViewModel();

            vm.Company = new Company();

            vm.Company = GetCompany(vm);

            vm.Locations = new List<Location>();

            // get list of locations 
            vm.Locations = GetLocations(vm);

            vm.Location = new Location();

            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteLocation(FormCollection col)
		{
            ProfileViewModel vm = InitProfileViewModel();

            vm.Company = new Company();

            vm.Company = GetCompany(vm);

            vm.Locations = new List<Location>();

            vm.Location = new Location();

            // get list of locations 
            vm.Locations = GetLocations(vm);

            if (col["btnSubmit"].ToString() == "delete")
			{
                // get selected LocationID
                vm.Location.LocationID = Convert.ToInt16(col["locations"]);

                // get location info based on locationID
                vm.Location = GetUpdatedLocationInfo(vm.Location);

                vm.Location.ActionType = DeleteLocation(vm);

                string previousVersion = vm.Location.Address + " " + vm.Location.City + ", intStateID = "  + vm.Location.intState + " " + vm.Location.Zip;

                // notify admin of change
                SendEditNotification(vm.User, 8, 4, previousVersion, "DELETED LOCATION");

                // get updated listed of locations 
                vm.Locations = GetLocations(vm);

                return View(vm);
			}
            return View(vm);
        }

        public ActionResult EditCategoriesByLocation()
		{
            ProfileViewModel vm = InitProfileViewModel();
            vm.Company = new Company();
            vm.Company = GetCompany(vm);
            vm.Locations = GetLocations(vm);
            vm.Location = new Location();
            vm.Category = new CategoryItem();
            vm.Categories = new List<CategoryItem>();

            return View(vm);
		}

        [HttpPost]
        public ActionResult EditCategoriesByLocation(FormCollection col)
		{
            ProfileViewModel vm = InitProfileViewModel();
            vm.Company = new Company();
            vm.Company = GetCompany(vm);
            vm.Locations = GetLocations(vm);
            vm.Location = new Location();
            vm.Category = new CategoryItem();
            vm.Categories = new List<CategoryItem>();
            vm.Button = new Button();
            vm.Button = vm.Button.GetButtonSession();

            if (col["btnSubmit"].ToString() == "addLocation")
            {
                return RedirectToAction("AddNewLocation", "Bakery");
            }

            if (col["btnSubmit"].ToString() == "addCategories")
            {
                vm.Button.CurrentButton = "add";
                // save button session for currently clicked button 
                vm.Button.SaveButtonSession();

                // get current LocationID
                vm.Location.LocationID = Convert.ToInt16(col["locations"]);

                // get list of categories not current applied to location
                vm.Categories = GetNotCategories(vm);
            }

            if (col["btnSubmit"].ToString() == "deleteCategories")
            {
                vm.Button.CurrentButton = "delete";

                // save button session for currently clicked button 
                vm.Button.SaveButtonSession();

                // get current LocationID
                vm.Location.LocationID = Convert.ToInt16(col["locations"]);

                // get list of categories currently applied to location 
                vm.Categories = GetCurrentCategories(vm);
            }

            if (col["btnSubmit"].ToString() == "submit")
            {
                // create button object
                Button button = new Button();

                // get current button session
                button = button.GetButtonSession();

                vm.Location.LocationID = Convert.ToInt16(col["locations"]);

                // get category(s) selected (by ID)
                string categoryIDs = col["categories"];

                // handle INSERT
                if (button.CurrentButton == "add")
                {
                    // submit to db 
                    vm.Category.ActionType = AddCategoriesToDB(vm, categoryIDs);
                }
                // handle DELETE 
                else if (button.CurrentButton == "delete")
                {
                    // submit to db 
                    vm.Category.ActionType = DeleteCategories(vm, categoryIDs);
                }

                // reset LocationID to 0 to reset form
                vm.Location.LocationID = 0;

                // remove current button session b/c we no longer need it 
                button.RemoveButtonSession();

                return View(vm);
            }
            return View(vm);
        }

        private CategoryItem.ActionTypes AddCategoriesToDB(ProfileViewModel vm, string categoryIDs)
        {
            try
            {
                // create database object
                Database db = new Database();

                // create array by splitting string at each comma 
                string[] AllStrings = categoryIDs.Split(',');

                // loop through array and assign CategoryID to Category object 
                // then add object to list of category items
                foreach (string item in AllStrings)
                {
                    // get categoryID 
                    vm.Category.ItemID = int.Parse(item);

                    // add to database
                    vm.Category.ActionType = db.InsertCategories(vm.Category, vm.Location);

                    // send notification of change to admin 
                    SendEditNotification(vm.User, 15, 5, "N/A", "ADDED CATEGORY TO LOCATION");
                }
                return vm.Category.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private CategoryItem.ActionTypes DeleteCategories(ProfileViewModel vm, string categoryIDs)
        {
            try
            {
                // create database object
                Database db = new Database();

                // create array by splitting string at each comma 
                string[] AllStrings = categoryIDs.Split(',');

                // loop through array and assign CategoryID to Category object 
                // then add object to list of category items
                foreach (string item in AllStrings)
                {
                    // get categoryID 
                    vm.Category.ItemID = int.Parse(item);

                    // add to database
                    vm.Category.ActionType = db.DeleteCategories(vm.Location, vm.Category);

                    SendEditNotification(vm.User, 15, 5, "N/A", "REMOVED CATEGORY FROM LOCATION");
                }
                return vm.Category.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<CategoryItem> GetNotCategories(ProfileViewModel vm)
        {
            try
            {
                // create db object 
                Database db = new Database();

                // get category list 
                vm.Categories = db.GetNotCategories(vm.Categories, vm.Location);

                return vm.Categories;

            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<CategoryItem> GetCurrentCategories(ProfileViewModel vm)
        {
            try
            {
                // create db object
                Database db = new Database();

                // get current category list
                vm.Categories = db.GetCurrentCategories(vm.Categories, vm.Location);

                return vm.Categories;

            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private Location.ActionTypes DeleteLocation(ProfileViewModel vm)
		{
            try
			{
                Database db = new Database();

                vm.Location.ActionType = db.MemberDeleteLocation(vm.Location);

                return vm.Location.ActionType;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
		}

        private List<Location> GetLocations(ProfileViewModel vm)
		{
            try
			{
                Database db = new Database();

                vm.Locations = db.GetLocations(vm.Company);

                return vm.Locations;
			}
            catch (Exception ex) { throw new Exception(ex.Message); }
		}

        private Location GetUpdatedLocationInfo(Location l)
		{
            try
			{
                Database db = new Database();

                l = db.GetLocation(l);

                return l;
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


        // -------------------------------------------------------------------------------------------------
        // ADDING/DELETING FROM DATABASE   
        // -------------------------------------------------------------------------------------------------

        private Website.ActionTypes AddWebsite(ProfileViewModel vm)
        {
            try
            {
                Database db = new Database();

                vm.Website.ActionType = db.InsertNewWebsite(vm.Website, vm.Company);

                return vm.Website.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private SocialMedia.ActionTypes AddSocialMedia(ProfileViewModel vm)
        {
            try
            {
                Database db = new Database();

                vm.SocialMedia.ActionType = db.InsertNewSocialMedia(vm.SocialMedia, vm.Company);

                return vm.SocialMedia.ActionType;

            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private Website.ActionTypes UpdateWebsite(Website w)
        {
            try
            {
                Database db = new Database();

                w.ActionType = db.UpdateWebsite(w);

                return w.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private SocialMedia.ActionTypes UpdateSocialMedia(SocialMedia s)
        {
            try
            {
                Database db = new Database();

                s.ActionType = db.UpdateSocialMedia(s);

                return s.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<Website> GetWebsiteTypes(ProfileViewModel vm)
        {
            try
            {
                // create database object
                Database db = new Database();

                // get list of website types 
                vm.Websites = db.GetWebsiteTypes();

                return vm.Websites;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<SocialMedia> GetSocialMediaTypes(ProfileViewModel vm)
        {
            try
            {
                // create database object
                Database db = new Database();

                // get list of website types 
                vm.SocialMediaList = db.GetSocialMediaTypes();

                return vm.SocialMediaList;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private Website.ActionTypes DeleteWebsite(Website w)
        {
            try
            {
                // create database object
                Database db = new Database();

                // delete website from database
                w.ActionType = db.DeleteWebsite(w);

                return w.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private void SendEditNotification(User u, int editedColumnID, int editedTableID, string previousVersion, string newVersion)
        {
            try
            {
                Database db = new Database();

                db.InsertAdminNotification(u, editedColumnID, editedTableID, previousVersion, newVersion);
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

        private Location.ActionTypes UpdateLocation(Location l)
        {
            try
            {
                // create database object
                Database db = new Database();

                // send update to db 
                l.ActionType = db.UpdateLocationInfo(l);

                return l.ActionType;

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

        private List<Website> GetCompanyWebsites(ProfileViewModel vm)
        {
            try
            {
                // create database object
                Database db = new Database();

                // get list of websites from db 
                vm.Websites = db.GetCompanyWebsites(vm.Company);

                return vm.Websites;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<SocialMedia> GetCompanySocialMedia(ProfileViewModel vm)
        {
            try
            {
                // create database object
                Database db = new Database();

                // get list of websites from db 
                vm.SocialMediaList = db.GetCompanySocialMedia(vm.Company);

                return vm.SocialMediaList;

            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

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