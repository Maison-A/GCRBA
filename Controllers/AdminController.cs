using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using GCRBA.Models;
using GCRBA.ViewModels;

namespace GCRBA.Controllers
{
    public class AdminPortalController : Controller
    {
        // -------------------------------------------------------------------------------------------------
        // ACTIONRESULT METHODS  
        // -------------------------------------------------------------------------------------------------
        public ActionResult Index() {

            User u = new User();
            u = u.GetUserSession();
            u.AdminNotifications = new List<AdminNotification>();
            u.AdminNotifications = GetAdminNotifications(u);
            u.AdminNotification = new AdminNotification();
            u.AdminNotification.UnreadNotifications = GetIfUnread(u);
            return View(u);
        }

        private bool GetIfUnread(User u)
        {
            try
            {
                int count = 0;

                for (int i = 0; i < u.AdminNotifications.Count; i++)
                {
                    if (u.AdminNotifications[i].NotificationStatusID == 2)
                    {
                        count += 1;
                    }
                }

                if (count > 0)
                {
                    u.AdminNotification.UnreadNotifications = true;
                }

                return u.AdminNotification.UnreadNotifications;

            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        [HttpPost]
        public ActionResult Index(FormCollection col)
        {
            User u = new User();
            u = u.GetUserSession();
            u.AdminNotification = new AdminNotification();

            if (col["btnSubmit"].ToString() == "viewLocationRequests")
            {
                return RedirectToAction("LocationRequests", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "viewMembershipRequests")
            {
                return RedirectToAction("MembershipRequests", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "editMainBanner") {
                return RedirectToAction("EditMainBanner", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "editCompanies") {
                return RedirectToAction("EditCompanies", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "viewNotifications")
            {
                return RedirectToAction("AdminNotification", "AdminPortal");
            }

            return View(u);
        }

        public ActionResult AdminNotification()
        {
            User u = new User();
            u = u.GetUserSession();

            // get notifications 
            u.AdminNotifications = new List<AdminNotification>();
            u.AdminNotifications = GetAdminNotifications(u);
            u.AdminNotification = new AdminNotification();
            return View(u);
        }

        [HttpPost]
        public ActionResult AdminNotification(FormCollection col)
        {
            User u = new User();
            u = u.GetUserSession();

            u.AdminNotifications = new List<AdminNotification>();
            u.AdminNotifications = GetAdminNotifications(u);

            string notificationIDs;

            if (col["btnSubmit"].ToString() == "delete")
            {
                if (col["notification"] != null)
                {
                    notificationIDs = col["notification"];
                    u.ActionType = DeleteNotifications(u, notificationIDs);

                    u.AdminNotifications = GetAdminNotifications(u);

                    return View(u);
                }
            }

            if (col["btnSubmit"].ToString() == "markAsRead")
            {
                if (col["notification"] != null)
                {
                    notificationIDs = col["notification"];
                    u.ActionType = UpdateAdminNotificationStatus(u, notificationIDs);
                    u.AdminNotifications = GetAdminNotifications(u);
                    return View(u);
                }
            }
            return View(u);
        }

        private User.ActionTypes UpdateAdminNotificationStatus(User u, string notificationIDs)
        {
            try
            {
                // create database object
                Database db = new Database();

                // create array by splitting string at each comma 
                string[] Notifications = notificationIDs.Split(',');

                // create user notification object 
                u.AdminNotification = new AdminNotification();

                // loop through array and update in db 
                foreach (string item in Notifications)
                {
                    u.AdminNotification.NotificationID = int.Parse(item);
                    u.ActionType = db.UpdateAdminNotificationStatus(u);
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
                u.AdminNotification = new AdminNotification();

                // loop through array and delete from db 
                foreach (string item in Notifications)
                {
                    u.AdminNotification.NotificationID = int.Parse(item);
                    u.ActionType = db.DeleteAdminNotifications(u);
                }

                return u.ActionType;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<AdminNotification> GetAdminNotifications(User u)
        {
            try
            {
                Database db = new Database();

                u.AdminNotifications = db.GetAdminNotifications();

                return u.AdminNotifications;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SelectListItem> GetAllAdminRequest(List<Models.AdminRequest> lstAdminRequest) {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Models.AdminRequest req in lstAdminRequest) {

                items.Add(new SelectListItem { Text = req.strRequestedChange, Value = req.intAdminRequest.ToString() });
            }
            return items;
        }

        public ActionResult LocationRequests()
        {
            User u = new User();

            Models.Database db = new Models.Database();
            List<Models.AdminRequest> adminRequests = new List<AdminRequest>();
            adminRequests = db.GetLocationRequests();
            Models.AdminRequestList adminRequestList = new Models.AdminRequestList()
            {
                SelectedAdminRequests = new[] { 1 },
                AdminRequests = GetAllAdminRequest(adminRequests)
            };

            return View(adminRequestList);
        }

        [HttpPost]
        public ActionResult LocationRequests(FormCollection col, AdminRequestList request)
        {
            Models.Database db = new Models.Database();
            Models.AdminRequestList adminRequestList = new Models.AdminRequestList();
            adminRequestList.lstAdminRequest = db.GetLocationRequests();
            request.AdminRequests = GetAllAdminRequest(adminRequestList.lstAdminRequest);
            if (col["btnSubmit"].ToString() == "approve" && request.SelectedAdminRequests != null)
            {
                List<SelectListItem> selectedItems = request.AdminRequests.Where(p => request.SelectedAdminRequests.Contains(int.Parse(p.Value))).ToList();
                foreach (var Request in selectedItems)
                {
                    Request.Selected = true;
                    Models.AdminRequest adminReq = new Models.AdminRequest();
                    adminReq = db.GetSingleLocationRequest(Convert.ToInt16(Request.Value));
                    Models.LocationList locList = new Models.LocationList();
                    locList.lstLocations[0] = db.GetTempLocation(Convert.ToInt16(Request.Value));

                    List<Models.CategoryItem> categoryItems = new List<CategoryItem>();
                    List<Models.CategoryItem>[] arrCategoryInfo = new List<CategoryItem>[10];
                    categoryItems = db.GetTempCategories(locList.lstLocations[0].lngLocationID);
                    arrCategoryInfo[0] = categoryItems;

                    foreach (Models.CategoryItem category in categoryItems)
                    {
                        foreach (Models.CategoryItem categoryCheck in locList.lstLocations[0].bakedGoods.lstBakedGoods)
                        {
                            if (categoryCheck.ItemID == category.ItemID)
                            {
                                categoryCheck.blnAvailable = true;
                            }
                        }
                    }


                    List<Models.Days>[] arrLocHours = new List<Days>[10];
                    List<Models.Days> LocationHours = new List<Days>();
                    LocationHours = db.GetTempLocationHours(locList.lstLocations[0].lngLocationID);
                    arrLocHours[0] = LocationHours;

                    List<Models.ContactPerson>[] arrContactInfo = new List<ContactPerson>[10];
                    List<Models.ContactPerson> contactPeople = new List<Models.ContactPerson>();
                    contactPeople = db.GetTempContacts(locList.lstLocations[0].lngLocationID);
                    arrContactInfo[0] = contactPeople;

                    List<Models.SocialMedia>[] arrSocialMediaInfo = new List<SocialMedia>[10];
                    List<Models.SocialMedia> socialMedias = new List<SocialMedia>();
                    socialMedias = db.GetTempSocialMedia(locList.lstLocations[0].lngLocationID);
                    arrSocialMediaInfo[0] = socialMedias;

                    List<Models.Website>[] arrWebsites = new List<Website>[10];
                    List<Models.Website> websites = new List<Website>();
                    websites = db.GetTempWebsite(locList.lstLocations[0].lngLocationID);
                    arrWebsites[0] = websites;

                    db.DeleteTempLocation(locList.lstLocations[0].lngLocationID, locList.lstLocations[0].lngCompanyID);
                    db.DeleteAdminRequest(adminReq.intAdminRequest);

                    locList.StoreNewLocation(arrCategoryInfo, arrLocHours, arrSocialMediaInfo, arrWebsites, arrContactInfo, adminReq);

                    Models.AdminRequestList updatedRequestList = new Models.AdminRequestList();
                    updatedRequestList.lstAdminRequest = db.GetLocationRequests();
                    request.AdminRequests = GetAllAdminRequest(updatedRequestList.lstAdminRequest);
                }
                return View(request);
            }

            if (col["btnSubmit"].ToString() == "deny" && request.SelectedAdminRequests != null)
            {
                List<SelectListItem> selectedItems = request.AdminRequests.Where(p => request.SelectedAdminRequests.Contains(int.Parse(p.Value))).ToList();
                foreach (var Request in selectedItems)
                {
                    Request.Selected = true;
                    Models.AdminRequest adminReq = new Models.AdminRequest();
                    adminReq = db.GetSingleLocationRequest(Convert.ToInt16(Request.Value));
                    Models.LocationList locList = new Models.LocationList();
                    locList.lstLocations[0] = db.GetTempLocation(Convert.ToInt16(Request.Value));

                    db.DeleteTempLocation(locList.lstLocations[0].lngLocationID, locList.lstLocations[0].lngCompanyID);
                    db.DeleteAdminRequest(adminReq.intAdminRequest);

                    Models.AdminRequestList updatedRequestList = new Models.AdminRequestList();
                    updatedRequestList.lstAdminRequest = db.GetLocationRequests();
                    request.AdminRequests = GetAllAdminRequest(updatedRequestList.lstAdminRequest);
                }
                return View(request);
            }
            return View();
        }

        public ActionResult MembershipRequests()
        {
            AdminVM vm = new AdminVM();

            vm.User = new User();

            vm.User = vm.User.GetUserSession();

            vm.MemberRequest = new MemberRequest();

            vm.MemberRequests = new List<MemberRequest>();

            // get membership requests 
            vm.MemberRequests = GetMembershipRequests(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult MembershipRequests(FormCollection col)
        {
            AdminVM vm = new AdminVM();

            vm.User = new User();

            vm.User = vm.User.GetUserSession();

            vm.MemberRequests = new List<MemberRequest>();

            // get membership requests 
            vm.MemberRequests = GetMembershipRequests(vm);

            // create MemberRequest object 
            // then get current session 
            // if none, null 
            vm.MemberRequest = new MemberRequest();
            vm.MemberRequest = vm.MemberRequest.GetMemberRequestSession();

            if (col["btnSubmit"].ToString() == "viewRequest")
            {
                vm.MemberRequest.MemberID = Convert.ToInt16(col["requests"]);

                // get member info from db 
                vm.MemberRequest = GetMemberInfo(vm);

                // save MemberID in CurrentRequest session 
                vm.MemberRequest.SaveMemberRequestSession();

                return View(vm);
            }

            if (col["btnSubmit"].ToString() == "approve")
            {
                // update in db 
                vm.MemberRequest.ActionType = UpdateMemberStatus(vm);

                // send user notification 
                // 1 is PK in tblNotification for membership approval message 
                // 2 is the PK in tblNotificationStatus for unread message 
                SendUserNotification(vm.MemberRequest, 1, 2);

                // remove MemberRequestSession 
                vm.MemberRequest.RemoveMemberRequestSession();

                // get membership requests 
                vm.MemberRequests = GetMembershipRequests(vm);

                // reset MemberID to 0
                vm.MemberRequest.MemberID = 0;

                return View(vm);
            }

            if (col["btnSubmit"].ToString() == "deny")
            {
                // delete record in db 
                vm.MemberRequest.ActionType = DeleteMemberRequest(vm.MemberRequest);

                // send user notificaiton
                // 2 in first param is PK in tblNotification for membership denial message
                // 2 in second param is PK in tblNotificationStatus for unread message 
                SendUserNotification(vm.MemberRequest, 2, 2);

                // remove member request session 
                vm.MemberRequest.RemoveMemberRequestSession();

                // get membership requests 
                vm.MemberRequests = GetMembershipRequests(vm);

                // reset MemberID to 0
                vm.MemberRequest.MemberID = 0;

                return View(vm);
            }

            return View(vm);
        }

        private void SendUserNotification(MemberRequest m, int intNotificationID, int intNotificationStatusID)
        {
            try
            {
                // create database object
                Database db = new Database();

                // send user notification 
                db.SendUserNotification(m, intNotificationID, intNotificationStatusID);
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private MemberRequest.ActionTypes DeleteMemberRequest(MemberRequest m)
        {
            try
            {
                // create database object
                Database db = new Database();

                // delete record from db 
                m.ActionType = db.DeleteMemberRequest(m);

                return m.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private MemberRequest.ActionTypes UpdateMemberStatus(AdminVM vm)
        {
            try
            {
                // create database object
                Database db = new Database();

                // update in db 
                vm.MemberRequest.ActionType = db.UpdateMemberStatus(vm.MemberRequest);

                return vm.MemberRequest.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private MemberRequest GetMemberInfo(AdminVM vm)
        {
            try
            {
                // create database object
                Database db = new Database();

                // get info from database
                vm.MemberRequest = db.GetMemberInfo(vm);

                return vm.MemberRequest;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public ActionResult EditMainBanner()
        {
            // initialize ProfileVM 
            ProfileViewModel vm = new ProfileViewModel();

            vm.User = new User();
            vm.User = vm.User.GetUserSession();

            // get banners list 
            vm.MainBanner = new MainBanner();
            vm.MainBannerList = new List<MainBanner>();
            vm.MainBannerList = GetBannersList();

            // return view with view model object passed as argument so we can access it in view
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditMainBanner(FormCollection col)
        {
            ProfileViewModel vm = new ProfileViewModel();
            vm.User = new User();
            vm.User = vm.User.GetUserSession();

            // get banners list 
            vm.MainBannerList = GetBannersList();

            // get current main banner 
            vm.MainBanner = new MainBanner();

            // button to submit new banner is selected 
            if (col["btnSubmit"].ToString() == "submit")
            {
                // drop down option with value = "new" selected 
                if (col["banners"].ToString() == "new")
                {
                    // add text from textarea to view model's banner property
                    vm.MainBanner.Banner = col["banner-input"];

                    // submit new banner to database 
                    vm.MainBanner.ActionType = UpdateMainBanner(vm.MainBanner);

                    // get updated banner list 
                    vm.MainBannerList = GetBannersList();

                    // return view with view model as argument 
                    return View(vm);
                }
                // one of the previous banners in the drop down selected to use for new banner
                else
                {
                    // set view model's BannerID property to value (ID from database) of selected option
                    vm.MainBanner.BannerID = Convert.ToInt16(col["banners"].ToString());

                    // get banner text from list of banners
                    vm.MainBanner.Banner = vm.MainBannerList[vm.MainBanner.BannerID - 1].Banner;

                    vm.MainBanner.ActionType = UpdateMainBanner(vm.MainBanner);

                    // return view with view model as argument 
                    return View(vm);
                }
            }
            return View(vm);
        }

        private MainBanner.ActionTypes UpdateMainBanner(MainBanner mb)
        {
            Database db = new Database();

            return db.InsertNewMainBanner(mb);
        }

        public ActionResult EditCompanies()
        {
            // get current user session so we know who is logged in (member, nonmember, admin)
            User u = new User();
            u = u.GetUserSession();

            return View(u);
        }

        [HttpPost]
        public ActionResult EditCompanies(FormCollection col)
        {
            // get current user session so we know who is logged in (member, nonmember, admin)
            User u = new User();
            u = u.GetUserSession();

            // Add Company button pressed
            if (col["btnSubmit"].ToString() == "addCompany")
            {
                return RedirectToAction("AddCompany", "AdminPortal");
            }

            // Delete Company button pressed
            if (col["btnSubmit"].ToString() == "deleteCompany")
            {
                return RedirectToAction("DeleteCompany", "AdminPortal");
            }

            // Edit Company button pressed
            if (col["btnSubmit"].ToString() == "editCompany")
            {
                return RedirectToAction("EditExistingCompany", "AdminPortal");
            }

            return View(u);

        }

        public ActionResult AddCompany()
        {
            // create object of view model
            ProfileViewModel vm = new ProfileViewModel();

            // create new user object with vm 
            vm.User = new User();

            // get current user session
            vm.User = vm.User.GetUserSession();

            // get list of states
            vm.State = new State();
            vm.States = new List<State>();
            vm.States = GetStatesList();

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddCompany(FormCollection col)
        {
            ProfileViewModel vm = new ProfileViewModel();
            vm.User = new User();
            vm.User = vm.User.GetUserSession();
            vm.Company = new Company();

            vm.State = new State();
            vm.States = new List<State>();
            vm.States = GetStatesList();

            // get input from form 
            if (col["btnSubmit"].ToString() == "submit")
            {
                // get new company info
                vm.Company.Name = col["Company.Name"];
                vm.Company.About = col["Company.About"];
                vm.Company.Year = col["Company.Year"];

                // submit new company info
                // if submitted successfully, new companyID will be applied to company object 
                vm.Company.ActionType = InsertNewCompany(vm.Company);

                // is location form active?
                if (col["Location.Address"] != "")
                {
                    // yes, create location object to hold input 
                    vm.Location = new Location();

                    // get input 
                    vm.Location.Address = col["Location.Address"];
                    vm.Location.City = col["Location.City"];
                    vm.Location.intState = Convert.ToInt16(col["Location.State"]);
                    vm.Location.Zip = col["Location.Zip"];
                    vm.Location.Phone = col["Location.Phone"];


                    if (vm.Location.Address != "" && vm.Location.City != "" && vm.Location.intState > 0 && vm.Location.Zip != "" && vm.Location.Phone != "")
                    {
                        // get email input if not empty, else assign email property null value 
                        if (col["Location.Email"] != "")
                        {
                            vm.Location.Email = col["Location.Email"];
                        } else
                        {
                            vm.Location.Email = null;
                        }

                        // insert new location to database using new companyID
                        vm.Location.ActionType = InsertNewLocation(vm.Location, vm.Company);
                    }

                }

                return View(vm);
            }

            return View(vm);
        }

        private Company.ActionTypes InsertNewCompany(Company c)
        {
            // create database object
            Database db = new Database();

            // add new company to db 
            return db.InsertNewCompany(c);
        }

        private Location.ActionTypes InsertNewLocation(Location l, Company c)
        {
            Database db = new Database();

            return db.AddNewLocation(l, c);
        }

        public ActionResult DeleteCompany()
        {
            // create VM object
            AdminVM vm = InitEditCompanies();

            vm.Companies = GetCompaniesList(vm);

            // return view 
            return View(vm);
        }

        private Company.ActionTypes DeleteCompany(AdminVM vm)
        {
            try
            {
                // create database object
                Database db = new Database();

                // delete company from database 
                vm.Company.ActionType = db.DeleteCompany(vm);

                return vm.Company.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        [HttpPost]
        public ActionResult DeleteCompany(FormCollection col)
        {
            // set initial flag value to 0
            ViewBag.Flag = 0;

            // create VM object
            AdminVM vm = InitEditCompanies();

            // get list of companies
            vm.Companies = GetCompaniesList(vm);

            // get selection 
            vm.Company.CompanyID = Convert.ToInt16(col["companies"].ToString());

            // delete button pressed
            if (col["btnSubmit"].ToString() == "delete")
            {
                // save action type correlating to success of deletion from database
                vm.Company.ActionType = DeleteCompany(vm);

                // get updated list of companies now in db 
                vm.Companies = GetCompaniesList(vm);

                return View(vm);
            }

            // cancel button pressed
            if (col["btnSubmit"].ToString() == "cancel")
            {
                // redirect to admin portal
                return RedirectToAction("Index", "AdminPortal");
            }

            return View(vm);
        }

        public ActionResult EditExistingCompany()
        {
            // create EditCompaniesVM object 
            AdminVM vm = new AdminVM();

            // create VM user object
            vm.User = new User();

            // get current user session
            vm.User = vm.User.GetUserSession();

            // get companies list 
            vm.Companies = GetCompaniesList(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditExistingCompany(FormCollection col)
        {
            // create EditCompaniesVM object 
            AdminVM vm = InitEditCompanies();

            // get companies list 
            vm.Companies = GetCompaniesList(vm);

            // get companyID from company selected from dropdown
            vm.Company.CompanyID = Convert.ToInt16(col["companies"]);

            // get rest of info associated with CompanyID
            vm.Company = vm.Company.GetCompanyInfo();

            // save current  ID so we can access it in other view 
            vm.Company.SaveCompanySession();

            if (col["btnSubmit"].ToString() == "addNewLocation")
            {
                return RedirectToAction("AddNewLocation", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "deleteLocation")
            {
                return RedirectToAction("DeleteLocation", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "editContacts")
            {
                return RedirectToAction("EditContacts", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "editCategories")
            {
                return RedirectToAction("EditCategories", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "editSpecials")
            {
                return RedirectToAction("EditSpecials", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "editGeneralInfo")
            {
                return RedirectToAction("EditGeneralInfo", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "cancel")
            {
                return RedirectToAction("Index", "AdminPortal");
            }

            return View(vm);
        }

        public ActionResult AddNewLocation()
        {
            AdminVM vm = InitEditCompanies();

            vm = InitLocationInfo(vm);

            // get current company session so we know which company we are editing information for 
            vm.Company = vm.Company.GetCompanySession();

            // get list of locations 
            vm.Locations = GetLocations(vm.Company);

            return View(vm);
        }

        //[HttpPost]
        //public ActionResult AddNewLocation(FormCollection col)
        //{
        //    try
        //    {
        //        AdminVM vm = InitEditCompanies();

        //        // initial location objects 
        //        vm = InitLocationInfo(vm);

        //        // get current company session
        //        vm = GetCompanySession(vm);

        //        // get list of locations 
        //        vm = GetLocations(vm);

        //        vm.NewLocation.lngCompanyID = vm.Company.CompanyID;

        //        if (col["btnSubmit"].ToString() == "addLocation")
        //        {
        //            vm.NewLocation.StreetAddress = col["NewLocation.StreetAddress"];
        //            vm.NewLocation.City = col["NewLocation.City"];
        //            vm.NewLocation.intState = Convert.ToInt16(col["states"]);
        //            vm.NewLocation.Zip = col["NewLocation.Zip"];

        //            // submit to db 
        //            vm.NewLocation.ActionType = SubmitLocationToDB(vm);

        //            return View(vm);
        //        }

        //        if (col["btnSubmit"].ToString() == "cancel")
        //        {
        //            return RedirectToAction("EditExistingCompany", "AdminPortal");
        //        }

        //        return View(vm);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        public ActionResult DeleteLocation()
        {
            // initialize EditCompaniesVM 
            AdminVM vm = InitEditCompanies();

            // get current company session 
            vm.Company = vm.Company.GetCompanySession();

            // create locations list object
            vm.Locations = new List<Location>();

            // get list of locations 
            vm.Locations = GetLocations(vm.Company);

            // create NewLocation object 
            vm.NewLocation = new NewLocation();

            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteLocation(FormCollection col)
        {
            try
            {
                // initialize EditCompaniesVM
                AdminVM vm = InitEditCompanies();

                // get current company session so we know which company we are making edits to
                vm.Company = vm.Company.GetCompanySession();

                // create locations list object
                vm.Locations = new List<Location>();

                // get list of locations to display in dropdown
                vm.Locations = GetLocations(vm.Company);

                // create Location object to hold location selected in dropdown to be deleted 
                vm.Location = new Location();

                // create NewLocation object 
                vm.NewLocation = new NewLocation();

                // if submit button pressed 
                if (col["btnSubmit"].ToString() == "submit")
                {
                    // get ID of location selected to be deleted 
                    vm.Location.LocationID = Convert.ToInt16(col["locations"]);

                    // send to database
                    vm.NewLocation.ActionType = DeleteLocation(vm);

                    // return view 
                    return View(vm);
                }

                // cancel button pressed
                if (col["btnSubmit"].ToString() == "cancel")
                {
                    // send back to Edit Existing Company page
                    return RedirectToAction("EditExistingCompany", "AdminPortal");
                }

                return View(vm);
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private Company GetCompanyInfo(Company c)
        {
            Database db = new Database();

            return db.GetCompanyInfo(c);
        }

        public ActionResult EditContacts()
        {
            ProfileViewModel vm = new ProfileViewModel();
            vm.User = new User().GetUserSession();
            vm.Company = new Company().GetCompanySession();

            // get list of contacts based on company 
            vm.ContactPerson = new ContactPerson();
            vm.Contacts = new List<ContactPerson>();
            vm.Contacts = vm.ContactPerson.GetContactsByCompany(vm.Company);

            // get list of contact person types to populate
            // contact person type dropdown
            vm.ContactPerson.Types = vm.ContactPerson.GetContactTypes();

            // get list of selected company's locations
            vm.Locations = new List<Location>();
            vm.Locations = GetLocations(vm.Company);

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditContacts(FormCollection col)
        {
            ProfileViewModel vm = new ProfileViewModel();
            vm.User = new User().GetUserSession();
            vm.Company = new Company().GetCompanySession();
            vm.Locations = new List<Location>();
            vm.Locations = GetLocations(vm.Company);

            // get list of contacts based on selected company 
            vm.ContactPerson = new ContactPerson();
            vm.Contacts = new List<ContactPerson>();
            vm.Contacts = vm.ContactPerson.GetContactsByCompany(vm.Company);

            // get list of contact person types to populate
            // contact person type dropdown
            vm.ContactPerson.Types = vm.ContactPerson.GetContactTypes();

            return View(vm);
        }

        public ActionResult EditCategories()
        {
            // initialize EditCompaniesVM object 
            ProfileViewModel vm = new ProfileViewModel();
            vm.User = new User();
            vm.User = vm.User.GetUserSession();
            vm.Company = new Company();
            vm.Company = vm.Company.GetCompanySession();
            vm.Category = new CategoryItem();
            vm.Categories = new List<CategoryItem>();
            vm.Location = new Location();
            vm.Locations = new List<Location>();
            vm.Locations = GetLocations(vm.Company);

            vm.Button = new Button();
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditCategories(FormCollection col)
        {
            ProfileViewModel vm = new ProfileViewModel();
            vm.User = new User();
            vm.User = vm.User.GetUserSession();
            vm.Company = new Company();
            vm.Company = vm.Company.GetCompanySession();
            vm.Category = new CategoryItem();
            vm.Categories = new List<CategoryItem>();
            vm.Location = new Location();
            vm.Locations = new List<Location>();
            vm.Locations = GetLocations(vm.Company);

            vm.Button = new Button();
            vm.Button = vm.Button.GetButtonSession();

            if (col["btnSubmit"].ToString() == "addLocation")
            {
                return RedirectToAction("AddNewLocation", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "addCategories")
            {
                vm.Button.RemoveButtonSession();
                vm.Button.CurrentButton = "add";
                vm.Button.SaveButtonSession();

                // get current LocationID
                vm.Location.LocationID = Convert.ToInt16(col["locations"]);

                // get list of categories not current applied to location
                vm.Categories = GetNotCategories(vm.Categories, vm.Location);
            }

            if (col["btnSubmit"].ToString() == "deleteCategories")
            {
                vm.Button.RemoveButtonSession();
                vm.Button.CurrentButton = "delete";
                vm.Button.SaveButtonSession();

                // get current LocationID
                vm.Location.LocationID = Convert.ToInt16(col["locations"]);

                // get list of categories currently applied to location 
                vm.Categories = GetCurrentCategories(vm.Categories, vm.Location);
            }

            if (col["btnSubmit"].ToString() == "submit")
            {
                vm.Location.LocationID = Convert.ToInt16(col["locations"]);

                // get category(s) selected (by ID)
                string categoryIDs = col["categories"];

                // handle INSERT
                if (vm.Button.CurrentButton == "add")
                {
                    // submit to db 
                    vm.Category.ActionType = AddCategoriesToDB(vm.Category, vm.Location, categoryIDs);
                }
                // handle DELETE 
                else if (vm.Button.CurrentButton == "delete")
                {
                    // submit to db 
                    vm.Category.ActionType = DeleteCategories(vm.Category, vm.Location, categoryIDs);
                }

                // reset LocationID to 0 to reset form
                vm.Location.LocationID = 0;

                // remove current button session b/c we no longer need it 
                vm.Button.RemoveButtonSession();

                return View(vm);
            }
            return View(vm);
        }

        public ActionResult EditSpecials()
        {
            // initialize EditCompaniesVM, CurrentUser, and CurrentCompany 
            // get CurrentUser session
            AdminVM vm = InitEditCompanies();

            // get current company session 
            vm.Company = vm.Company.GetCompanySession();

            vm.Locations = new List<Location>();

            // get list of locations for current company 
            vm.Locations = GetLocations(vm.Company);

            // create location object
            vm.Location = new Location();

            // create special object
            vm.Special = new SaleSpecial();

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditSpecials(FormCollection col)
        {
            // initialize EditCompaniesVM, CurrentUser, and CurrentCompany 
            // get CurrentUser session
            AdminVM vm = InitEditCompanies();

            // get current company session 
            vm.Company = vm.Company.GetCompanySession();

            vm.Locations = new List<Location>();

            // get list of locations for current company 
            vm.Locations = GetLocations(vm.Company);

            // create list of specials 
            vm.Specials = new List<SaleSpecial>();

            // create location object
            vm.Location = new Location();

            // get current location session
            vm.Location = vm.Location.GetLocationSession();

            // create new button object so we can track which button was selected
            vm.Button = new Button();

            // get current button session 
            vm.Button = vm.Button.GetButtonSession();

            // create new special object
            vm.Special = new SaleSpecial();

            // button to add location clicked 
            if (col["btnSubmit"].ToString() == "addLocation")
            {

                // redirect to add location page 
                return RedirectToAction("AddNewLocation", "AdminPortal");
            }

            // button to add new special clicked 
            if (col["btnSubmit"].ToString() == "addSpecial")
            {

                // remove previous location session 
                vm.Location.RemoveLocationSession();

                // are there any location selections?
                if (col["locations"] == null)
                {
                    // no, so let user know they need to select a location before proceeding 
                    vm.Location.ActionType = Location.ActionTypes.RequiredFieldMissing;
                    return View(vm);
                }
                else
                {
                    // yes, so save LocationID
                    vm.Location.LocationID = Convert.ToInt16(col["locations"]);
                    vm.Location.SaveLocationSession();
                }

                // remove current button session 
                vm.Button.RemoveButtonSession();

                // add new current button session
                vm.Button.CurrentButton = "add";

                // save new button session 
                vm.Button.SaveButtonSession();
            }

            if (col["btnSubmit"].ToString() == "deleteSpecial")
            {

                // remove previous location session 
                vm.Location.RemoveLocationSession();

                // are there any location selections?
                if (col["locations"] == null)
                {
                    // no, so let user know they need to select a location before proceeding 
                    vm.Location.ActionType = Location.ActionTypes.RequiredFieldMissing;
                    return View(vm);
                }
                else
                {
                    // yes, so save LocationID
                    vm.Location.LocationID = Convert.ToInt16(col["locations"]);
                    vm.Location.SaveLocationSession();
                }

                vm.Specials = GetSpecials(vm.Location.LocationID);

                // remove current button session 
                vm.Button.RemoveButtonSession();

                // add new current button session
                vm.Button.CurrentButton = "delete";

                // save new button session 
                vm.Button.SaveButtonSession();
            }

            // button to add special to locations clicked 
            if (col["btnSubmit"].ToString() == "submit")
            {

                if (vm.Button.CurrentButton == "add")
                {

                    // get input
                    vm.Special.strDescription = col["Special.strDescription"];

                    if (col["Special.monPrice"] == null || col["Special.monPrice"] == "")
					{
                        vm.Special.monPrice = 0;
					}
                    vm.Special.dtmStart = Convert.ToDateTime(col["Special.dtmStart"]);
                    vm.Special.dtmEnd = Convert.ToDateTime(col["Special.dtmEnd"]);

                    // add special to tblSpecial
                    // then add special and location to tblSpecialLocation
                    vm.Special.ActionType = AddSpecialToLocation(vm);

                    return View(vm);

                }
                else if (vm.Button.CurrentButton == "delete")
                {

                    // get specialID of selected special 
                    vm.Special.SpecialID = Convert.ToInt16(col["specials"]);

                    // delete from db 
                    vm.Special.ActionType = DeleteSpecialFromLocation(vm);

                    return View(vm);
                }

                if (col["btnSubmit"].ToString() == "cancel")
                {
                    return RedirectToAction("EditExistingCompany", "AdminPortal");
                }
            }

            return View(vm);
        }

        public ActionResult EditGeneralInfo()
        {

            return View();
        }

        public ActionResult EditCompanyInfo()
        {
            // initialize EditCompaniesVM object
            AdminVM vm = InitEditCompanies();

            // get companyID that was selected from  dropdown on previous page and saved in company session
            vm.Company = vm.Company.GetCompanySession();

            // create database object
            Database db = new Database();

            // get current company info based on selected company from previous page
            //vm.Company = db.GetCompanyInfo(vm);

            // get locations list
            vm.Locations = db.GetLocations(vm.Company);

            return View(vm);
        }

        // -------------------------------------------------------------------------------------------------
        // INITIALIZING COMMONLY USED CLASSES 
        // -------------------------------------------------------------------------------------------------

        private RequestsVM InitRequestsVM()
        {
            // create instance of RequestsVM
            RequestsVM vm = new RequestsVM();

            // create new User object
            // then get current user session 
            vm.User = new User();
            vm.User = vm.User.GetUserSession();

            return vm;
        }

        private AdminVM InitEditCategories(AdminVM vm)
        {
            // get current company session 
            vm.Company = vm.Company.GetCompanySession();


            vm.Locations = new List<Location>();

            // get list of locations for current company 
            vm.Locations = GetLocations(vm.Company);

            // create Location object that will hold selected location 
            vm.Location = new Location();

            // create Category object
            vm.Category = new CategoryItem();

            // create list of categories
            vm.Categories = new List<CategoryItem>();

            return vm;
        }

        private AdminVM InitEditSpecials(AdminVM vm)
        {
            // get current company session
            vm.Company = vm.Company.GetCompanySession();

            vm.Locations = new List<Location>();
            // get list of locations 
            vm.Locations = GetLocations(vm.Company);

            // create location object to hold selected location
            vm.Location = new Location();

            // create new Specials object
            vm.Special = new SaleSpecial();

            return vm;
        }


        private AdminVM InitEditCompanies()
        {
            // create EditCompaniesVM object 
            AdminVM vm = new AdminVM();

            // create VM user object
            vm.User = new User();

            // get current user session
            vm.User = vm.User.GetUserSession();

            // create new VM company object 
            vm.Company = new Company();

            return vm;
        }

        private AdminBannerViewModel InitAdminBannerVM()
        {
            // create view model object
            AdminBannerViewModel vm = new AdminBannerViewModel();

            // create user objects and populate 
            vm.CurrentUser = new User();

            // get admin status because page should only be viewable by admin
            vm.CurrentUser = vm.CurrentUser.GetUserSession();

            return vm;
        }

        private AdminVM InitLocationInfo(AdminVM vm)
        {
            // create VM location objects 
            vm.NewLocation = new NewLocation();
            vm.Locations = new List<Location>();
            vm.States = new List<State>();

            // get states to display in drop down 
            vm.States = GetStatesList();

            return vm;
        }

        // -------------------------------------------------------------------------------------------------
        // SUBMITTING DATA TO DATABASE 
        // -------------------------------------------------------------------------------------------------

        private CategoryItem.ActionTypes AddCategoriesToDB(CategoryItem c, Location l, string categoryIDs)
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
                    c.ItemID = int.Parse(item);

                    // add to database
                    c.ActionType = db.InsertCategories(c, l);
                }
                return c.ActionType;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private CategoryItem.ActionTypes DeleteCategories(CategoryItem c, Location l, string categoryIDs)
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
                    c.ItemID = int.Parse(item);

                    // add to database
                    c.ActionType = db.DeleteCategories(l, c);
                }
                return c.ActionType;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //private NewLocation.ActionTypes SubmitLocationToDB(AdminVM vm)
        //{
        //    try
        //    {
        //        Database db = new Database();

        //        // submit to db 
        //        vm.NewLocation.ActionType = db.AddNewLocation(vm);

        //        return vm.NewLocation.ActionType;
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        private NewLocation.ActionTypes DeleteLocation(AdminVM vm)
        {
            // create db object 
            Database db = new Database();

            // get action type from attemp to delete location from db 
            vm.NewLocation.ActionType = db.DeleteLocation(vm.Location.LocationID, vm.Location.CompanyID);

            return vm.NewLocation.ActionType;
        }

        private SaleSpecial.ActionTypes DeleteSpecialFromLocation(AdminVM vm)
        {
            try
            {
                // create db object
                Database db = new Database();

                // delete from table 
                vm.Special.ActionType = db.DeleteSpecialLocation(vm.Special, vm.Location);

                return vm.Special.ActionType;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private SaleSpecial.ActionTypes AddSpecialToLocation(AdminVM vm)
        {
            try
            {
                // create db object
                Database db = new Database();

                // add new special to tblSpecial first 
                vm.Special = db.InsertSpecial(vm.Special);

                // then add special and location to tblSpecialLocation 
                vm.Special.ActionType = db.InsertSpecialLocation(vm.Special, vm.Location);

                return vm.Special.ActionType;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // -------------------------------------------------------------------------------------------------
        // RETRIEVING DATA FROM DATABASE 
        // -------------------------------------------------------------------------------------------------

        private List<MemberRequest> GetMembershipRequests(AdminVM vm)
        {
            try
            {
                // create database object
                Database db = new Database();

                // get list from db 
                vm.MemberRequests = db.GetMembershipRequests();

                return vm.MemberRequests;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private string GetState(int intStateID)
        {
            try
            {
                // create db object
                Database db = new Database();

                // create variable to hold state
                string state = "";

                // get state
                state = db.GetState(intStateID);

                return state;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<State> GetStatesList()
        {
            // create database object 
            Database db = new Database();

            // get states from database 
            // each object in list contains StateID and State name
            return db.GetStates();
        }

        private List<MainBanner> GetBannersList()
        {
            // create new database object 
            Database db = new Database();

            // return list of banners
            return db.GetMainBanners();
        }

        private List<CategoryItem> GetNotCategories(List<CategoryItem> categories, Location l)
        {
            // create db object 
            Database db = new Database();

            // get category list 
            return db.GetNotCategories(categories, l);
        }

        private List<CategoryItem> GetCurrentCategories(List<CategoryItem> categories, Location l) 
        {
            // create db object
            Database db = new Database();

            // get current category list
            return db.GetCurrentCategories(categories, l);
        }


        private List<Location> GetLocations(Company c)
        {
            // create db object
            Database db = new Database();

            // get list of locations from db 
            return db.GetLocations(c);
        }

        private List<Location> GetLocationWhereNotContact(AdminVM vm)
        {
            // create db object
            Database db = new Database();

            // create location list objects 
            vm.Locations = new List<Location>();

            // get list of locations where selected contact is not a contact
            vm.Locations = db.GetLocationsNotContact(vm);

            return vm.Locations;
        }

        public List<Company> GetCompaniesList(AdminVM vm)
        {
            // create database object
            Database db = new Database();

            // create VM company list object 
            vm.Companies = new List<Company>();

            // get list of companies
            vm.Companies = db.GetCompanies();

            return vm.Companies;
        }

        private List<SaleSpecial> GetSpecials(long intLocationID)
        {
            try
            {
                // create specials list object
                List<SaleSpecial> specials = new List<SaleSpecial>();

                // create db object
                Database db = new Database();

                // get list of specials 
                specials = db.GetLandingSpecials(intLocationID);

                return specials;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // -------------------------------------------------------------------------------------------------
        // HANDLING SESSIONS 
        // -------------------------------------------------------------------------------------------------

        public void SaveButtonSession(string buttonValue)
        {
            try
            {
                // create button object 
                Button button = new Button();

                // get value of button pressed 
                button.CurrentButton = buttonValue;

                // save button session 
                button.SaveButtonSession();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // -------------------------------------------------------------------------------------------------
        // AJAX/JSON METHODS  
        // -------------------------------------------------------------------------------------------------

        // AdminPortal --> Edit Existing Company --> Edit Contact(s) --> Add New Contact
        // when button is clicked for just submitting section re: adding new contact, this method is called
        // get input data, make sure required fields aren't empty, and submit to database 
        // method returns action type based on validation of input/submission result 
        public ContactPerson.ActionTypes SubmitNewContact(string FirstName = "", string LastName = "", string Phone = "", string Email = "", long LocationID = 0, short TypeID = 0)
        {
            ContactPerson contact = new ContactPerson();

            // create contact object 
            contact = contact.CreateContact(FirstName, LastName, Phone, Email, LocationID, TypeID);

            // validate new contact form and get potential action types
            return contact.ValidateNewContactForm();

        }

        // AdminPortal --> Edit Existing Company --> Edit Contact(s) --> Edit Existing Contact
        // when button is clicked for submitting the edit contact section only, this method is called
		public ContactPerson.ActionTypes UpdateContact (string FirstName = "", string LastName = "", string Phone = "", string Email = "", int ContactID = 0)
		{
            ContactPerson contact = new ContactPerson();

            // get selected contact's current information 
            contact = contact.GetSpecificContact();

            return contact.UpdateContact(FirstName, LastName, Phone, Email, ContactID);
		}

        // AdminPortal --> Edit Existing Company --> Edit Contact(s) --> Remove Contact(s)
        // when contact(s) selected in drop down and button for removal section is clicked, 
        // this method is called, and submits removal to database
		public ContactPerson.ActionTypes RemoveContacts(List<string> SelectedContacts)
		{
			ContactPerson contact = new ContactPerson();

			return contact.RemoveContacts(SelectedContacts);
		}

		// AdminPortal --> Edit Existing Company --> Edit Contact(s) --> Edit Existing Contact 
		// when contact is selected in dropdown, this method is called
		// method gets contact info based on selected contact and returns said contact object 
		public JsonResult ContactInfo(int ID)
        {
            // initialize objects we're going to use 
            ProfileViewModel vm = new ProfileViewModel();
            vm.ContactPerson = new ContactPerson();
            vm.Contacts = new List<ContactPerson>();
            vm.Company = new Company().GetCompanySession();

            // get list of contacts based on selected company 
            vm.Contacts = vm.ContactPerson.GetContactsByCompany(vm.Company);

            // declare integer variable to hold index of contact in list 
            int index = 0;

            // get index of contact with given ID 
            for (int i = 0; i < vm.Contacts.Count; i++)
            {
                if (vm.Contacts[i].ContactPersonID == ID)
                {
                    index = i;
                }
            }

            // get contact based on index
            vm.ContactPerson = vm.Contacts[index];

            return Json(vm.ContactPerson, JsonRequestBehavior.AllowGet);
        }

        // AdminPortal --> Edit Existing Company --> Edit Contact(s) --> Manage Contacts by Location 
        public JsonResult ContactsByLocation(int ID)
        {
            ProfileViewModel vm = new ProfileViewModel();
            vm.Contacts = new List<ContactPerson>();
            vm.Contacts = vm.ContactPerson.GetContactsByLocation(ID);
            return Json(vm.Contacts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContactsByCompany()
		{
            ProfileViewModel vm = new ProfileViewModel();

            vm.Company = new Company().GetCompanySession();

            vm.Contacts = new List<ContactPerson>();
            vm.ContactPerson = new ContactPerson();
            vm.Contacts = vm.ContactPerson.GetContactsByCompany(vm.Company);

            return Json(vm.Contacts, JsonRequestBehavior.AllowGet);
		}

    }
}