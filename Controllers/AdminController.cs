using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GCRBA.Models;
using GCRBA.ViewModels;

namespace GCRBA.Controllers
{
    public class AdminPortalController : Controller
    {
        // -------------------------------------------------------------------------------------------------
        // ACTIONRESULT METHODS  
        // -------------------------------------------------------------------------------------------------

        public ActionResult Index()
        {
            // get current user session so we know who is logged in (member, nonmember, admin)  
            User u = new User();

            Models.Database db = new Models.Database();
            List<Models.AdminRequest> adminRequests = new List<AdminRequest>();
            adminRequests = db.GetAdminRequests();
            Models.AdminRequestList adminRequestList = new Models.AdminRequestList() {
                SelectedAdminRequests = new [] {1},
                AdminRequests = GetAllAdminRequest(adminRequests)
            };

            return View(adminRequestList);
            /*
            AdminVM vm = new AdminVM();

            vm.User = new User();
            vm.User = vm.User.GetUserSession();

            vm.Request = new Request();

            vm.Request.TotalRequests = GetTotalRequests();

            return View(vm);
            */
        }

        [HttpPost]
        public ActionResult Index(FormCollection col, AdminRequestList request)
        {
           Models.Database db = new Models.Database();
           Models.AdminRequestList adminRequestList = new Models.AdminRequestList();
           adminRequestList.lstAdminRequest = db.GetAdminRequests();
           request.AdminRequests = GetAllAdminRequest(adminRequestList.lstAdminRequest);
           if(col["btnSubmit"].ToString() == "approve" && request.SelectedAdminRequests != null) {
                List<SelectListItem> selectedItems = request.AdminRequests.Where(p => request.SelectedAdminRequests.Contains(int.Parse(p.Value))).ToList(); 
                foreach(var Request in selectedItems) {
                    Request.Selected = true;
                    Models.LocationList locList = new Models.LocationList();
                    locList.lstLocations[0] = db.GetTempLocation(Convert.ToInt16(Request.Value));

                    List<Models.CategoryItem> categoryItems = new List<CategoryItem>();
                    List<Models.CategoryItem>[] arrCategoryInfo = new List<CategoryItem>[10];
                    categoryItems = db.GetTempCategories(locList.lstLocations[0].lngLocationID);
                    arrCategoryInfo[0] = categoryItems;

                    foreach (Models.CategoryItem category in categoryItems) {
                        foreach (Models.CategoryItem categoryCheck in locList.lstLocations[0].bakedGoods.lstBakedGoods) {
                            if(categoryCheck.ItemID == category.ItemID) {
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

                    locList.StoreNewLocation(arrCategoryInfo, arrLocHours, arrSocialMediaInfo, arrWebsites, arrContactInfo);
                }
                return View(request);
			}

           
            // Edit Main Banner button pressed
            if (col["btnSubmit"].ToString() == "editMainBanner")
            {
                return RedirectToAction("EditMainBanner", "AdminPortal");
            }

            // Edit Companites button pressed
            if (col["btnSubmit"].ToString() == "editCompanies")
            {
                return RedirectToAction("EditCompanies", "AdminPortal");
            }
            return View();
        }

        public List<SelectListItem> GetAllAdminRequest(List<Models.AdminRequest> lstAdminRequest) {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Models.AdminRequest req in lstAdminRequest) {
                items.Add(new SelectListItem { Text = req.strRequestedChange, Value = req.intAdminRequest.ToString() });
            }
            return items;
            /*
            }
                if (col["btnSubmit"].ToString() == "viewRequests")
                {
                    return RedirectToAction("Requests", "AdminPortal");
                }

                return View();
            }
            */
        }

        public ActionResult Requests()
		{
            // initialize RequestsVM
            RequestsVM vm = InitRequestsVM();

            // get total requests 
            vm.Request.TotalRequests = GetTotalRequests();

            return View(vm);
		}

        public ActionResult EditMainBanner()
        {
            // create view model object 
            AdminBannerViewModel vm = InitAdminBannerVM();

            // get banners list 
            vm.MainBanners = GetBannersList(vm);

            // return view with view model object passed as argument so we can access it in view
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditMainBanner(FormCollection col)
        {
            // create view model object so that we can show data from more than one
            // model in the view 
            AdminBannerViewModel vm = InitAdminBannerVM();

            // create new database object
            Database db = new Database();

            // get banners list 
            vm.MainBanners = GetBannersList(vm);

            // get current main banner 
            vm.MainBanner = new MainBanner();

            // set default to 0
            ViewBag.Flag = 0;

            // return to main admin portal if user selects cancel  button 
            if (col["btnSubmit"].ToString() == "cancel")
            {
                return RedirectToAction("Index", "AdminPortal");
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
                    if (db.InsertNewMainBanner(vm) == true)
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
                    vm.MainBanner.Banner = vm.MainBanners[vm.MainBanner.BannerID - 1].Banner;

                    // try to add banner to newest row in table in db 
                    if (db.InsertNewMainBanner(vm) == true)
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
            AdminVM vm = InitEditCompanies();

            // create new user object with vm 
            vm.User = new User();

            // get current user session
            vm.User = vm.User.GetUserSession();

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddCompany(FormCollection col)
        {
            // create objects of what we will use 
            AdminVM vm = InitEditCompanies();
            vm.Company = new Company();
            Database db = new Database();

            // get input from form 
            if (col["btnSubmit"].ToString() == "submit")
            {
                vm.Company.Name = col["Company.Name"];
                vm.Company.About = col["Company.About"];
                vm.Company.Year = col["Company.Year"];
            }

            // add to database
            vm.Company.ActionType = InsertNewCompany(vm);

            return View(vm);
        }

        private Company.ActionTypes InsertNewCompany(AdminVM vm)
		{
            try
			{
                // create database object
                Database db = new Database();

                // add new company to db 
                vm.Company.ActionType = db.InsertNewCompany(vm);
                return vm.Company.ActionType;
			}
            catch (Exception ex) { throw new Exception(ex.Message); }
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
			}
            catch (Exception ex) { throw new Exception(ex.Message); }
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

            if (col["btnSubmit"].ToString() == "addContactPerson")
            {
                return RedirectToAction("AddContactPerson", "AdminPortal");
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
            vm = GetCompanySession(vm);

            // get list of locations 
            vm = GetLocations(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddNewLocation(FormCollection col)
        {
           try
            {
                AdminVM vm = InitEditCompanies();

                // initial location objects 
                vm = InitLocationInfo(vm);

                // get current company session
                vm = GetCompanySession(vm);

                // get list of locations 
                vm = GetLocations(vm);

                vm.NewLocation.lngCompanyID = vm.Company.CompanyID;

                if (col["btnSubmit"].ToString() == "addLocation")
                {
                    vm.NewLocation.StreetAddress = col["NewLocation.StreetAddress"];
                    vm.NewLocation.City = col["NewLocation.City"];
                    vm.NewLocation.intState = Convert.ToInt16(col["states"]);
                    vm.NewLocation.Zip = col["NewLocation.Zip"];

                    // submit to db 
                    vm.NewLocation.ActionType = SubmitLocationToDB(vm);

                    return View(vm);
                }

                if (col["btnSubmit"].ToString() == "cancel")
                {
                    return RedirectToAction("EditExistingCompany", "AdminPortal");
                }

                return View(vm);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public ActionResult DeleteLocation()
        {
            // initialize EditCompaniesVM 
            AdminVM vm = InitEditCompanies();

            // get current company session 
            vm = GetCompanySession(vm);

            // create locations list object
            vm.Locations = new List<Location>();

            // get list of locations 
            vm = GetLocations(vm);

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
                vm = GetCompanySession(vm);

                // create locations list object
                vm.Locations = new List<Location>();

                // get list of locations to display in dropdown
                vm = GetLocations(vm);

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
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
       
        public ActionResult AddContactPerson()
        {
            // create EditCompaniesVM object
            AdminVM vm = InitEditCompanies();

            // get current company session
            vm = GetCompanySession(vm);

            //initialize location list variable 
            vm.Locations = new List<Location>();

            // get list of locations for current company 
            vm = GetLocations(vm);

            // create ContactPerson object 
            vm.ContactPerson = new ContactPerson();

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddContactPerson(FormCollection col)
        {
            // create EditCompaniesVM object
            AdminVM vm = InitEditCompanies();

            // get current company session
            vm = GetCompanySession(vm);

            //initialize location list variable 
            vm.Locations = new List<Location>();

            // get list of locations for current company 
            vm = GetLocations(vm);

            // create new ContactPerson object 
            vm.Contacts = new List<ContactPerson>();

            vm.ContactPerson = new ContactPerson();

            if (col["btnSubmit"].ToString() == "addExistingContact")
            {
                return RedirectToAction("AddExistingContact", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "addNewContact")
            {
                return RedirectToAction("AddNewContact", "AdminPortal");
            }

            return View(vm);
        }

        public ActionResult AddExistingContact()
        {
            // initialize EditCompaniesVM
            AdminVM vm = InitEditCompanies();

            // get current company session 
            vm = GetCompanySession(vm);

            // create Contacts list object 
            vm.Contacts = new List<ContactPerson>();

            // create Locations list object 
            vm.Locations = new List<Location>();

            // get contacts based on company 
            vm.Contacts = GetContactsByCompany(vm);

            // create ContactPerson object 
            vm.ContactPerson = new ContactPerson();

            return View(vm);
        }

        [HttpPost]
        public ActionResult AddExistingContact(FormCollection col)
        {
            ViewBag.PersonSelected = 0;

            AdminVM vm = InitEditCompanies();

            vm = GetCompanySession(vm);

            vm.Contacts = new List<ContactPerson>();

            vm.Contacts = GetContactsByCompany(vm);

            vm.ContactPerson = new ContactPerson();

            vm.Locations = new List<Location>();

            vm.Location = new Location();

            if (col["btnSubmit"].ToString() == "getLocations")
            {
                vm.ContactPerson.lngContactPersonID = Convert.ToInt16(col["contacts"]);
                vm.Locations = GetLocationWhereNotContact(vm);
            }

            if (col["btnSubmit"].ToString() == "submit")
            {

            }

            if (col["btnSubmit"].ToString() == "cancel")
            {
                return RedirectToAction("AddContactPerson", "AdminPortal");
            }

            return View(vm);
        }

        public ActionResult EditCategories()
        {
            // initialize EditCompaniesVM object 
            AdminVM vm = InitEditCompanies();

            // get current company session 
            vm = InitEditCategories(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditCategories(FormCollection col)
        {
            AdminVM vm = InitEditCompanies();

            vm = InitEditCategories(vm);

            if (col["btnSubmit"].ToString() == "addLocation")
            {
                return RedirectToAction("AddNewLocation", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "addCategories")
            {
                // save button session for currently clicked button 
                SaveButtonSession("add");

                // get current LocationID
                vm.Location.LocationID = Convert.ToInt16(col["locations"]);

                // get list of categories not current applied to location
                vm = GetNotCategories(vm);
            }

            if (col["btnSubmit"].ToString() == "deleteCategories")
            {
                // save button session for currently clicked button 
                SaveButtonSession("delete");

                // get current LocationID
                vm.Location.LocationID = Convert.ToInt16(col["locations"]);

                // get list of categories currently applied to location 
                vm = GetCurrentCategories(vm);
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

        public ActionResult EditSpecials()
        {
            // initialize EditCompaniesVM, CurrentUser, and CurrentCompany 
            // get CurrentUser session
            AdminVM vm = InitEditCompanies();

            // get current company session 
            vm.Company = vm.Company.GetCompanySession();

            // create Locations object
            vm.Locations = new List<Location>();

            // get locations available for this company 
            vm = GetLocations(vm);

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

            // create Locations object
            vm.Locations = new List<Location>();

            // get locations available for this company 
            vm = GetLocations(vm);

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
            if (col["btnSubmit"].ToString() == "addLocation") {

                // redirect to add location page 
                return RedirectToAction("AddNewLocation", "AdminPortal");
			}

            // button to add new special clicked 
            if (col["btnSubmit"].ToString() == "addSpecial") {

                // remove previous location session 
                vm.Location.RemoveLocationSession();

                // are there any location selections?
                if (col["locations"] == null)
				{
                    // no, so let user know they need to select a location before proceeding 
                    vm.Location.ActionType = Location.ActionTypes.RequiredFieldMissing;
                    return View(vm);
				} else
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
                } else
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
            if (col["btnSubmit"].ToString() == "submit") {

                if (vm.Button.CurrentButton == "add") 
                {

                    // get input
                    vm.Special.strDescription = col["Special.strDescription"];
                    vm.Special.monPrice = Convert.ToDecimal(col["Special.monPrice"]);
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
            vm.Company = db.GetCompanyInfo(vm);

            // get locations list
            vm.Locations = db.GetLocations(vm);

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
            vm = GetCompanySession(vm);

            // get list of locations for current company 
            vm = GetLocations(vm);

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
            vm = GetCompanySession(vm);

            // get list of locations 
            vm = GetLocations(vm);

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

        private CategoryItem.ActionTypes AddCategoriesToDB(AdminVM vm, string categoryIDs)
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
                    vm.Category.ActionType = db.InsertCategories(vm);
                }
                return vm.Category.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private CategoryItem.ActionTypes DeleteCategories(AdminVM vm, string categoryIDs)
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
                    vm.Category.ActionType = db.DeleteCategories(vm);
                }
                return vm.Category.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private NewLocation.ActionTypes SubmitLocationToDB(AdminVM vm)
        {
            try
            {
                Database db = new Database();

                // submit to db 
                vm.NewLocation.ActionType = db.AddNewLocation(vm);

                return vm.NewLocation.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private NewLocation.ActionTypes DeleteLocation(AdminVM vm)
        {
            // create db object 
            Database db = new Database();

            // get action type from attemp to delete location from db 
            vm.NewLocation.ActionType = db.DeleteLocation(vm.Location.LocationID);

            return vm.NewLocation.ActionType;
        }

        private SaleSpecial.ActionTypes DeleteSpecialFromLocation(AdminVM vm)
        {
            try
            {
                // create db object
                Database db = new Database();

                // delete from table 
                vm.Special.ActionType = db.DeleteSpecialLocation(vm);

                return vm.Special.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private SaleSpecial.ActionTypes AddSpecialToLocation(AdminVM vm)
        {
            try
            {
                // create db object
                Database db = new Database();

                // add new special to tblSpecial first 
                vm.Special = db.InsertSpecial(vm);

                // then add special and location to tblSpecialLocation 
                vm.Special.ActionType = db.InsertSpecialLocation(vm);

                return vm.Special.ActionType;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        // -------------------------------------------------------------------------------------------------
        // RETRIEVING DATA FROM DATABASE 
        // -------------------------------------------------------------------------------------------------

        private int GetTotalRequests()
        {
            try
            {
                // create db object
                Database db = new Database();

                // create variable to hold total requests 
                int total = 0;

                // get total requests 
                total = db.GetTotalRequests();

                return total;
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
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<State> GetStatesList()
        {
            // create EditCompaniesVM object
            AdminVM vm = new AdminVM();

            // create database object 
            Database db = new Database();

            // get states from database 
            vm.States = db.GetStates();

            return vm.States;
        }

        private List<MainBanner> GetBannersList(AdminBannerViewModel vm)
        {
            // create new database object 
            Database db = new Database();

            // create VM banner list object 
            vm.MainBanners = new List<MainBanner>();

            // get list of banners
            vm.MainBanners = db.GetMainBanners();

            return vm.MainBanners;
        }

        private AdminVM GetNotCategories(AdminVM vm)
        {
            try
            {
                // create db object 
                Database db = new Database();

                // get category list 
                vm.Categories = db.GetNotCategories(vm);

                return vm;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private AdminVM GetCurrentCategories(AdminVM vm)
        {
            try
            {
                // create db object
                Database db = new Database();

                // get current category list
                vm.Categories = db.GetCurrentCategories(vm);

                return vm;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }


        private AdminVM GetLocations(AdminVM vm)
        {
            try
            {
                // create db object
                Database db = new Database();

                // get list of locations from db 
                vm.Locations = db.GetLocations(vm);

                return vm;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<ContactPerson> GetContactsByCompany(AdminVM vm)
        {
            // create db object
            Database db = new Database();

            // create contacts list object
            vm.Contacts = new List<ContactPerson>();

            // get list of contacts based on company selected
            vm.Contacts = db.GetContactsByCompany(vm);

            return vm.Contacts;
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

        private List<SaleSpecial> GetSpecials(int intLocationID)
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
            } catch (Exception ex) { throw new Exception(ex.Message); }
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
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private AdminVM GetCompanySession(AdminVM vm)
        {
            try
            {
                // create database object
                Database db = new Database();

                // get current companyID from session
                vm.Company = vm.Company.GetCompanySession();

                // get rest of current company information using companyID we get from session
                vm.Company = db.GetCompanyInfo(vm);

                return vm;
            } catch (Exception ex) { throw new Exception(ex.Message); }
        }

    }
}