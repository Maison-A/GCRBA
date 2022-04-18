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
        

        public ActionResult Index()
        {
            // get current user session so we know who is logged in (member, nonmember, admin)  
            User u = new User();
            u = u.GetUserSession();

            return View(u);
        }

        [HttpPost]
        public ActionResult Index(FormCollection col)
        {
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
            EditCompaniesViewModel vm = InitEditCompanies();

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
            EditCompaniesViewModel vm = InitEditCompanies();
            vm.CurrentCompany = new Company();
            Database db = new Database();

            // get input from form 
            if (col["btnSubmit"].ToString() == "submit")
            {
                vm.CurrentCompany.Name = col["CurrentCompany.Name"];
                vm.CurrentCompany.About = col["CurrentCompany.About"];
                vm.CurrentCompany.Year = col["CurrentCompany.Year"];
            }

            // add to database
            vm.CurrentCompany.ActionType = vm.CurrentCompany.SaveInsert();

            return View(vm);
        }

        public ActionResult DeleteCompany()
        {
            // create VM object
            EditCompaniesViewModel vm = InitEditCompanies();

            vm.Companies = GetCompaniesList(vm);

            // return view 
            return View(vm);
        }

        [HttpPost]
        public ActionResult DeleteCompany(FormCollection col)
        {
            // set initial flag value to 0
            ViewBag.Flag = 0;

            // create VM object
            EditCompaniesViewModel vm = InitEditCompanies();

            // get list of companies
            vm.Companies = GetCompaniesList(vm);

            // get selection 
            vm.CurrentCompany.CompanyID = Convert.ToInt16(col["companies"].ToString());

            // delete button pressed
            if (col["btnSubmit"].ToString() == "delete")
            {
                // save action type correlating to success of deletion from database
                vm.CurrentCompany.ActionType = vm.CurrentCompany.SaveDelete();

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
            EditCompaniesViewModel vm = new EditCompaniesViewModel();

            // create VM user object
            vm.CurrentUser = new User();

            // get current user session
            vm.CurrentUser = vm.CurrentUser.GetUserSession();

            // get companies list 
            vm.Companies = GetCompaniesList(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditExistingCompany(FormCollection col)
        {
            // create EditCompaniesVM object 
            EditCompaniesViewModel vm = InitEditCompanies();

            // get companies list 
            vm.Companies = GetCompaniesList(vm);

            // get companyID from company selected from dropdown
            vm.CurrentCompany.CompanyID = Convert.ToInt16(col["companies"]);

            // save current  ID so we can access it in other view 
            vm.CurrentCompany.SaveCompanySession();

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
            EditCompaniesViewModel vm = InitEditCompanies();

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
                EditCompaniesViewModel vm = InitEditCompanies();

                // initial location objects 
                vm = InitLocationInfo(vm);

                // get current company session
                vm = GetCompanySession(vm);

                // get list of locations 
                vm = GetLocations(vm);

                vm.NewLocation.lngCompanyID = vm.CurrentCompany.CompanyID;

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
            EditCompaniesViewModel vm = InitEditCompanies();

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
                EditCompaniesViewModel vm = InitEditCompanies();

                // get current company session so we know which company we are making edits to
                vm = GetCompanySession(vm);

                // create locations list object
                vm.Locations = new List<Location>();

                // get list of locations to display in dropdown
                vm = GetLocations(vm);

                // create Location object to hold location selected in dropdown to be deleted 
                vm.CurrentLocation = new Location();

                // create NewLocation object 
                vm.NewLocation = new NewLocation();

                // if submit button pressed 
                if (col["btnSubmit"].ToString() == "submit")
                {
                    // get ID of location selected to be deleted 
                    vm.CurrentLocation.LocationID = Convert.ToInt16(col["locations"]);

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
            EditCompaniesViewModel vm = InitEditCompanies();

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
            EditCompaniesViewModel vm = InitEditCompanies();

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
            EditCompaniesViewModel vm = InitEditCompanies();

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

            EditCompaniesViewModel vm = InitEditCompanies();

            vm = GetCompanySession(vm);

            vm.Contacts = new List<ContactPerson>();

            vm.Contacts = GetContactsByCompany(vm);

            vm.ContactPerson = new ContactPerson();

            vm.Locations = new List<Location>();

            vm.CurrentLocation = new Location();

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
            EditCompaniesViewModel vm = InitEditCompanies();

            // get current company session 
            vm = InitEditCategories(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditCategories(FormCollection col)
        {
            EditCompaniesViewModel vm = InitEditCompanies();

            vm = InitEditCategories(vm);
           
            if (col["btnSubmit"].ToString() == "addLocation")
            {
                return RedirectToAction("AddNewLocation", "AdminPortal");
            }

            if (col["btnSubmit"].ToString() == "addCategories")
            {
                // get current LocationID
                vm.CurrentLocation.LocationID = Convert.ToInt16(col["locations"]);

                // get list of categories not current applied to location
                vm = GetNotCategories(vm);
            }

            if (col["btnSubmit"].ToString() == "deleteCategories")
            {

            }

            if (col["btnSubmit"].ToString() == "submit")
            {
                vm.CurrentLocation.LocationID = Convert.ToInt16(col["locations"]);

                // get category(s) selected (by ID)
                string categoryIDs = col["categories"];

                // submit to db 
                vm.Category.ActionType = AddCategoriesToDB(vm, categoryIDs);

                // reset LocationID to 0 to reset form
                vm.CurrentLocation.LocationID = 0;

                return View(vm);
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
            EditCompaniesViewModel vm = InitEditCompanies();

            // get companyID that was selected from  dropdown on previous page and saved in company session
            vm.CurrentCompany = vm.CurrentCompany.GetCompanySession();

            // create database object
            Database db = new Database();

            // get current company info based on selected company from previous page
            vm.CurrentCompany = db.GetCompanyInfo(vm);

            // get locations list
            vm.Locations = db.GetLocations(vm);

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

        private List<ContactPerson> GetContactsByCompany(EditCompaniesViewModel vm)
        {
            // create db object
            Database db = new Database();

            // create contacts list object
            vm.Contacts = new List<ContactPerson>();

            // get list of contacts based on company selected
            vm.Contacts = db.GetContactsByCompany(vm);

            return vm.Contacts;
        }

        private List<Location> GetLocationWhereNotContact(EditCompaniesViewModel vm)
        {
            // create db object
            Database db = new Database();

            // create location list objects 
            vm.Locations = new List<Location>();

            // get list of locations where selected contact is not a contact
            vm.Locations = db.GetLocationsNotContact(vm);

            return vm.Locations;
        }

        public List<Company> GetCompaniesList(EditCompaniesViewModel vm)
        {
            // create database object
            Database db = new Database();

            // create VM company list object 
            vm.Companies = new List<Company>();

            // get list of companies
            vm.Companies = db.GetCompanies();

            return vm.Companies;
        }

        private EditCompaniesViewModel GetCompanySession(EditCompaniesViewModel vm)
        {
            try
            {
                // create database object
                Database db = new Database();

                // get current companyID from session
                vm.CurrentCompany = vm.CurrentCompany.GetCompanySession();

                // get rest of current company information using companyID we get from session
                vm.CurrentCompany = db.GetCompanyInfo(vm);

                return vm;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private EditCompaniesViewModel GetNotCategories(EditCompaniesViewModel vm)
        {
            try
            {
                // create db object 
                Database db = new Database();

                // get category list 
                vm.Categories = db.GetNotCategories(vm);

                return vm;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private EditCompaniesViewModel GetLocations(EditCompaniesViewModel vm)
        {
            try
            {
                // create db object
                Database db = new Database();

                // get list of locations from db 
                vm.Locations = db.GetLocations(vm);            

                return vm;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private NewLocation.ActionTypes DeleteLocation(EditCompaniesViewModel vm)
        {
            // create db object 
            Database db = new Database();

            // get action type from attemp to delete location from db 
            vm.NewLocation.ActionType = db.DeleteLocation(vm.CurrentLocation.LocationID);

            return vm.NewLocation.ActionType;
        }

        private EditCompaniesViewModel InitEditCategories(EditCompaniesViewModel vm)
        {
            // get current company session 
            vm = GetCompanySession(vm);

            // get list of locations for current company 
            vm = GetLocations(vm);

            // create Location object that will hold selected location 
            vm.CurrentLocation = new Location();

            // create Category object
            vm.Category = new CategoryItem();

            // create list of categories
            vm.Categories = new List<CategoryItem>();

            return vm; 
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
            // create EditCompaniesVM object
            EditCompaniesViewModel vm = new EditCompaniesViewModel();

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

        private NewLocation.ActionTypes SubmitLocationToDB(EditCompaniesViewModel vm)
        {
            try
            {
                Database db = new Database();

                // submit to db 
                vm.NewLocation.ActionType = db.AddNewLocation(vm);

                return vm.NewLocation.ActionType;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private CategoryItem.ActionTypes AddCategoriesToDB(EditCompaniesViewModel vm, string categoryIDs)
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
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private EditCompaniesViewModel InitEditCompanies()
        {
            // create EditCompaniesVM object 
            EditCompaniesViewModel vm = new EditCompaniesViewModel();

            // create VM user object
            vm.CurrentUser = new User();

            // get current user session
            vm.CurrentUser = vm.CurrentUser.GetUserSession();

            // create new VM company object 
            vm.CurrentCompany = new Company();

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

        private EditCompaniesViewModel InitLocationInfo(EditCompaniesViewModel vm)
        {
            // create VM location objects 
            vm.NewLocation = new NewLocation();
            vm.Locations = new List<Location>();
            vm.States = new List<State>();

            // get states to display in drop down 
            vm.States = GetStatesList();

            return vm;
        }

    }
}