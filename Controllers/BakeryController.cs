using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web.Routing;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CsvHelper;
using CsvHelper.Configuration;
using GCRBA;

namespace GCRBA.Views.Bakery {
    public class BakeryController : Controller {
        //private Process firstProcess;

        // GET: Bakery
        public ActionResult Index() {
            Models.SearchResults results = new Models.SearchResults();
            return View(results);
        }

        [HttpPost]
        public ActionResult Index(FormCollection col) {
            Models.SearchResults results = new Models.SearchResults();
            
            if (col["btnSubmit"].ToString() == "addLocation") {
                return RedirectToAction("AddNewLocation");
            }
            
            if(col["btnSubmit"].ToString() == "search") {
                results.Donuts = new Models.CategoryItem() { ItemID = 1, ItemDesc = "Donuts", blnAvailable = Convert.ToBoolean(col["Donuts.blnAvailable"].Split(',')[0]) };
                results.Bagels = new Models.CategoryItem() { ItemID = 2, ItemDesc = "Bagels", blnAvailable = Convert.ToBoolean(col["Bagels.blnAvailable"].Split(',')[0]) };
                results.Muffins = new Models.CategoryItem() { ItemID = 3, ItemDesc = "Muffins", blnAvailable = Convert.ToBoolean(col["Muffins.blnAvailable"].Split(',')[0]) };
                results.IceCream = new Models.CategoryItem() { ItemID = 4, ItemDesc = "Ice Cream", blnAvailable = Convert.ToBoolean(col["IceCream.blnAvailable"].Split(',')[0]) };
                results.FineCandies = new Models.CategoryItem() { ItemID = 5, ItemDesc = "Fine Candies & Chocolates", blnAvailable = Convert.ToBoolean(col["FineCandies.blnAvailable"].Split(',')[0]) };
                results.WeddingCakes = new Models.CategoryItem() { ItemID = 6, ItemDesc = "Wedding Cakes", blnAvailable = Convert.ToBoolean(col["WeddingCakes.blnAvailable"].Split(',')[0]) };
                results.Breads = new Models.CategoryItem() { ItemID = 7, ItemDesc = "Breads", blnAvailable = Convert.ToBoolean(col["Breads.blnAvailable"].Split(',')[0]) };
                results.DecoratedCakes = new Models.CategoryItem() { ItemID = 8, ItemDesc = "Decorated Cakes", blnAvailable = Convert.ToBoolean(col["DecoratedCakes.blnAvailable"].Split(',')[0]) };
                results.Cupcakes = new Models.CategoryItem() { ItemID = 9, ItemDesc = "Cupcakes", blnAvailable = Convert.ToBoolean(col["Cupcakes.blnAvailable"].Split(',')[0]) };
                results.Cookies = new Models.CategoryItem() { ItemID = 10, ItemDesc = "Cookies", blnAvailable = Convert.ToBoolean(col["Cookies.blnAvailable"].Split(',')[0]) };
                results.Desserts = new Models.CategoryItem() { ItemID = 11, ItemDesc = "Desserts/Tortes", blnAvailable = Convert.ToBoolean(col["Desserts.blnAvailable"].Split(',')[0]) };
                results.Full = new Models.CategoryItem() { ItemID = 12, ItemDesc = "Full-line Bakery", blnAvailable = Convert.ToBoolean(col["Full.blnAvailable"].Split(',')[0]) };
                results.Deli = new Models.CategoryItem() { ItemID = 13, ItemDesc = "Deli/Catering", blnAvailable = Convert.ToBoolean(col["Deli.blnAvailable"].Split(',')[0]) };
                results.Other = new Models.CategoryItem() { ItemID = 14, ItemDesc = "Other Carryout Deli", blnAvailable = Convert.ToBoolean(col["Other.blnAvailable"].Split(',')[0]) };
                results.Wholesale = new Models.CategoryItem() { ItemID = 15, ItemDesc = "Wholesale", blnAvailable = Convert.ToBoolean(col["Wholesale.blnAvailable"].Split(',')[0]) };
                results.Delivery = new Models.CategoryItem() { ItemID = 16, ItemDesc = "Delivery (3rd Party)", blnAvailable = Convert.ToBoolean(col["Delivery.blnAvailable"].Split(',')[0]) };
                results.Shipping = new Models.CategoryItem() { ItemID = 17, ItemDesc = "Shipping", blnAvailable = Convert.ToBoolean(col["Shipping.blnAvailable"].Split(',')[0]) };
                results.Online = new Models.CategoryItem() { ItemID = 18, ItemDesc = "Online Ordering", blnAvailable = Convert.ToBoolean(col["Online.blnAvailable"].Split(',')[0]) };

                List<Models.CategoryItem> lstSearchBySpecialty = new List<Models.CategoryItem>() {
                    results.Donuts, results.Bagels, results.Muffins, results.IceCream, results.FineCandies, results.WeddingCakes, results.Breads, results.DecoratedCakes, results.Cupcakes,
                    results.Cookies, results.Desserts, results.Full, results.Deli, results.Other, results.Wholesale, results.Delivery, results.Shipping, results.Online
                };

                Models.Database db = new Models.Database();
                results.lstLocations = db.GetLocations(lstSearchBySpecialty);
                return View(results);
			}

            try {
                long lngSelectedLocID = Convert.ToInt64(col["btnSubmit"]);
                return RedirectToAction("TestLandingPage", new { id = lngSelectedLocID});
            }
            catch {
                return View();
            }
        }

        public ActionResult TestLandingPage(long Id) {
            Models.Database db = new Models.Database();
            Models.SearchResults results = new Models.SearchResults();
            Models.NewLocation loc = new Models.NewLocation();
            results.landingLocation = db.GetLandingLocation(Id);
            loc.lngLocationID = Id;
            loc.LocationName = results.landingLocation.LocationName;
            loc.City = results.landingLocation.City;
            loc.StreetAddress = results.landingLocation.StreetAddress;
            loc.State = results.landingLocation.State;
            loc.Zip = results.landingLocation.Zip;
            return View(loc);
        }

        [HttpPost]
        public ActionResult TestLandingPage(FormCollection col) {
            Models.NewLocation loc = new Models.NewLocation();
            loc.lngLocationID = Convert.ToInt64(col["lngLocationID"]);
            Models.Database db = new Models.Database();
            loc.ActionType = Models.NewLocation.ActionTypes.NoType;
            loc.ActionType = db.DeleteLocation(loc.lngLocationID);
            return RedirectToAction("Index", "Bakery");
		}

        public ActionResult MemberBakery(long Id) {
            return View();
		}

        public ActionResult AddNewLocation() {
            Models.Database db = new Models.Database();
            Models.NewLocation loc = new Models.NewLocation();

            loc.lstStates = db.GetStates();

            loc.lstCompanies = db.GetCompanies();
            Models.Company nonValue = new Models.Company { intCompanyID = 0, strCompanyName = "Select Existing Company" };
            loc.lstCompanies.Add(nonValue);

            loc.Donuts = new Models.CategoryItem() { ItemID = 1, ItemDesc = "Donuts" };
            loc.Bagels = new Models.CategoryItem() { ItemID = 2, ItemDesc = "Bagels" };
            loc.Muffins = new Models.CategoryItem() { ItemID = 3, ItemDesc = "Muffins" };
            loc.IceCream = new Models.CategoryItem() { ItemID = 4, ItemDesc = "Ice Cream" };
            loc.FineCandies = new Models.CategoryItem() { ItemID = 5, ItemDesc = "Fine Candies & Chocolates" };
            loc.WeddingCakes = new Models.CategoryItem() { ItemID = 6, ItemDesc = "Wedding Cakes" };
            loc.Breads = new Models.CategoryItem() { ItemID = 7, ItemDesc = "Breads" };
            loc.DecoratedCakes = new Models.CategoryItem() { ItemID = 8, ItemDesc = "Decorated Cakes" };
            loc.Cupcakes = new Models.CategoryItem() { ItemID = 9, ItemDesc = "Cupcakes" };
            loc.Cookies = new Models.CategoryItem() { ItemID = 10, ItemDesc = "Cookies" };
            loc.Desserts = new Models.CategoryItem() { ItemID = 11, ItemDesc = "Desserts/Tortes" };
            loc.Full = new Models.CategoryItem() { ItemID = 12, ItemDesc = "Full-line Bakery" };
            loc.Deli = new Models.CategoryItem() { ItemID = 13, ItemDesc = "Deli/Catering" };
            loc.Other = new Models.CategoryItem() { ItemID = 14, ItemDesc = "Other Carryout Deli" };
            loc.Wholesale = new Models.CategoryItem() { ItemID = 15, ItemDesc = "Wholesale" };
            loc.Delivery = new Models.CategoryItem() { ItemID = 16, ItemDesc = "Delivery (3rd Party)" };
            loc.Shipping = new Models.CategoryItem() { ItemID = 17, ItemDesc = "Shipping" };
            loc.Online = new Models.CategoryItem() { ItemID = 18, ItemDesc = "Online Ordering" };

            //Hours of Operation
            loc.Sunday = new Models.Days() { strDay = "Sunday" };
            loc.Monday = new Models.Days() { strDay = "Monday" };
            loc.Tuesday = new Models.Days() { strDay = "Tuesday" };
            loc.Wednesday = new Models.Days() { strDay = "Wednesday" };
            loc.Thursday = new Models.Days() { strDay = "Thursday" };
            loc.Friday = new Models.Days() { strDay = "Friday" };
            loc.Saturday = new Models.Days() { strDay = "Saturday" };

            //Special Options

            //Website
            loc.MainWeb = new Models.Website() { strWebsiteType = "Main Website URL" };
            loc.OrderingWeb = new Models.Website() { strWebsiteType = "Ordering URL" };
            loc.KettleWeb = new Models.Website() { strWebsiteType = "Donation Kettle URL" };

            //SocialMedia
            loc.Facebook = new Models.SocialMedia() { strPlatform = "Facebook" };
            loc.Twitter = new Models.SocialMedia() { strPlatform = "Twitter" };
            loc.Instagram = new Models.SocialMedia() { strPlatform = "Instagram" };
            loc.Snapchat = new Models.SocialMedia() { strPlatform = "Snapchat" };
            loc.TikTok = new Models.SocialMedia() { strPlatform = "TikTok" };
            loc.Yelp = new Models.SocialMedia() { strPlatform = "Yelp" };
            

			return View(loc);
        }

        [HttpPost]
        public ActionResult AddNewLocation(FormCollection col) {
            if (col["btnSubmit"].ToString() == "NewLocation") {
                try {
                    Models.NewLocation loc = new Models.NewLocation();
                    if(Convert.ToInt64(col["lngCompanyID"]) != 0) {
						loc.lngCompanyID = Convert.ToInt64(col["lngCompanyID"]);
					}

					loc.CompanyName = col["CompanyName"];
                    loc.LocationName = col["LocationName"];
                    loc.StreetAddress = col["StreetAddress"];
                    loc.City = col["City"];
                    loc.intState = Convert.ToInt16(col["intState"]);
                    loc.Zip = col["Zip"];

                    //Hours of Operation
                    loc.Sunday = new Models.Days() { strDay = "Sunday", intDayID = 1, blnOperational = Convert.ToBoolean(col["Sunday.blnOperational"].Split(',')[0]), strOpenTime = col["Sunday.strOpenTime"], strClosedTime = col["Sunday.strClosedTime"] };
                    loc.Monday = new Models.Days() { strDay = "Monday", intDayID = 2, blnOperational = Convert.ToBoolean(col["Monday.blnOperational"].Split(',')[0]), strOpenTime = col["Monday.strOpenTime"], strClosedTime = col["Monday.strClosedTime"] };
                    loc.Tuesday = new Models.Days() { strDay = "Tuesday", intDayID = 3, blnOperational = Convert.ToBoolean(col["Tuesday.blnOperational"].Split(',')[0]), strOpenTime = col["Tuesday.strOpenTime"], strClosedTime = col["Tuesday.strClosedTime"] };
                    loc.Wednesday = new Models.Days() { strDay = "Wednesday", intDayID = 4, blnOperational = Convert.ToBoolean(col["Wednesday.blnOperational"].Split(',')[0]), strOpenTime = col["Wednesday.strOpenTime"], strClosedTime = col["Wednesday.strClosedTime"] };
                    loc.Thursday = new Models.Days() { strDay = "Thursday", intDayID = 5, blnOperational = Convert.ToBoolean(col["Thursday.blnOperational"].Split(',')[0]), strOpenTime = col["Thursday.strOpenTime"], strClosedTime = col["Thursday.strClosedTime"] };
                    loc.Friday = new Models.Days() { strDay = "Friday", intDayID = 6, blnOperational = Convert.ToBoolean(col["Friday.blnOperational"].Split(',')[0]), strOpenTime = col["Friday.strOpenTime"], strClosedTime = col["Friday.strClosedTime"] };
                    loc.Saturday = new Models.Days() { strDay = "Saturday", intDayID = 7, blnOperational = Convert.ToBoolean(col["Saturday.blnOperational"].Split(',')[0]), strOpenTime = col["Saturday.strOpenTime"], strClosedTime = col["Saturday.strClosedTime"] };

                    //Product Categories
                    loc.Donuts = new Models.CategoryItem() { ItemID = 1, ItemDesc = "Donuts", blnAvailable = Convert.ToBoolean(col["Donuts.blnAvailable"].Split(',')[0]) };
                    loc.Bagels = new Models.CategoryItem() { ItemID = 2, ItemDesc = "Bagels", blnAvailable = Convert.ToBoolean(col["Bagels.blnAvailable"].Split(',')[0]) };
                    loc.Muffins = new Models.CategoryItem() { ItemID = 3, ItemDesc = "Muffins", blnAvailable = Convert.ToBoolean(col["Muffins.blnAvailable"].Split(',')[0]) };
                    loc.IceCream = new Models.CategoryItem() { ItemID = 4, ItemDesc = "Ice Cream", blnAvailable = Convert.ToBoolean(col["IceCream.blnAvailable"].Split(',')[0]) };
                    loc.FineCandies = new Models.CategoryItem() { ItemID = 5, ItemDesc = "Fine Candies & Chocolates", blnAvailable = Convert.ToBoolean(col["FineCandies.blnAvailable"].Split(',')[0]) };
                    loc.WeddingCakes = new Models.CategoryItem() { ItemID = 6, ItemDesc = "Wedding Cakes", blnAvailable = Convert.ToBoolean(col["WeddingCakes.blnAvailable"].Split(',')[0]) };
                    loc.Breads = new Models.CategoryItem() { ItemID = 7, ItemDesc = "Breads", blnAvailable = Convert.ToBoolean(col["Breads.blnAvailable"].Split(',')[0]) };
                    loc.DecoratedCakes = new Models.CategoryItem() { ItemID = 8, ItemDesc = "Decorated Cakes", blnAvailable = Convert.ToBoolean(col["DecoratedCakes.blnAvailable"].Split(',')[0]) };
                    loc.Cupcakes = new Models.CategoryItem() { ItemID = 9, ItemDesc = "Cupcakes", blnAvailable = Convert.ToBoolean(col["Cupcakes.blnAvailable"].Split(',')[0]) };
                    loc.Cookies = new Models.CategoryItem() { ItemID = 10, ItemDesc = "Cookies", blnAvailable = Convert.ToBoolean(col["Cookies.blnAvailable"].Split(',')[0]) };
                    loc.Desserts = new Models.CategoryItem() { ItemID = 11, ItemDesc = "Desserts/Tortes", blnAvailable = Convert.ToBoolean(col["Desserts.blnAvailable"].Split(',')[0]) };
                    loc.Full = new Models.CategoryItem() { ItemID = 12, ItemDesc = "Full-line Bakery", blnAvailable = Convert.ToBoolean(col["Full.blnAvailable"].Split(',')[0]) };
                    loc.Deli = new Models.CategoryItem() { ItemID = 13, ItemDesc = "Deli/Catering", blnAvailable = Convert.ToBoolean(col["Deli.blnAvailable"].Split(',')[0]) };
                    loc.Other = new Models.CategoryItem() { ItemID = 14, ItemDesc = "Other Carryout Deli", blnAvailable = Convert.ToBoolean(col["Other.blnAvailable"].Split(',')[0]) };
                    loc.Wholesale = new Models.CategoryItem() { ItemID = 15, ItemDesc = "Wholesale", blnAvailable = Convert.ToBoolean(col["Wholesale.blnAvailable"].Split(',')[0]) };
                    loc.Delivery = new Models.CategoryItem() { ItemID = 16, ItemDesc = "Delivery (3rd Party)", blnAvailable = Convert.ToBoolean(col["Delivery.blnAvailable"].Split(',')[0]) };
                    loc.Shipping = new Models.CategoryItem() { ItemID = 17, ItemDesc = "Shipping", blnAvailable = Convert.ToBoolean(col["Shipping.blnAvailable"].Split(',')[0]) };
                    loc.Online = new Models.CategoryItem() { ItemID = 18, ItemDesc = "Online Ordering", blnAvailable = Convert.ToBoolean(col["Online.blnAvailable"].Split(',')[0]) };

                    //Business Contact Information
                    loc.BusinessPhone = new Models.PhoneNumber();
                    loc.BusinessPhone.AreaCode = col["BusinessPhone.AreaCode"];
                    loc.BusinessPhone.Prefix = col["BusinessPhone.Prefix"];
                    loc.BusinessPhone.Suffix = col["BusinessPhone.Suffix"];
                    loc.BusinessEmail = col["BusinessEmail"];

                    //Member Only Variables
                    //Contact Person Information
                    loc.LocationContact = new Models.ContactPerson();
                    loc.LocationContact.strContactFirstName = col["LocationContact.strContactFirstName"];
                    loc.LocationContact.strContactLastName = col["LocationContact.strContactLastName"];
                    loc.LocationContact.contactPhone = new Models.PhoneNumber();
                    loc.LocationContact.contactPhone.AreaCode = col["LocationContact.contactPhone.AreaCode"];
                    loc.LocationContact.contactPhone.Prefix = col["LocationContact.contactPhone.Prefix"];
                    loc.LocationContact.contactPhone.Suffix = col["LocationContact.contactPhone.Suffix"];
                    loc.LocationContact.strContactEmail = col["LocationContact.strContactEmail"];
                    loc.LocationContact.intContactTypeID = 1;

                    //Web Admin contact information
                    loc.WebAdmin = new Models.ContactPerson();
                    loc.WebAdmin.strContactFirstName = col["WebAdmin.strContactFirstName"];
                    loc.WebAdmin.strContactLastName = col["WebAdmin.strContactLastName"];
                    loc.WebAdmin.contactPhone = new Models.PhoneNumber();
                    loc.WebAdmin.contactPhone.AreaCode = col["WebAdmin.contactPhone.AreaCode"];
                    loc.WebAdmin.contactPhone.Prefix = col["WebAdmin.contactPhone.Prefix"];
                    loc.WebAdmin.contactPhone.Suffix = col["WebAdmin.contactPhone.Suffix"];
                    loc.WebAdmin.strContactEmail = col["WebAdmin.strContactEmail"];
                    loc.WebAdmin.intContactTypeID = 2;

                    //Customer Service Contact Information
                    loc.CustService = new Models.ContactPerson();
                    loc.CustService.strContactFirstName = col["CustService.strContactFirstName"];
                    loc.CustService.strContactLastName = col["CustService.strContactLastName"];
                    loc.CustService.contactPhone = new Models.PhoneNumber();
                    loc.CustService.contactPhone.AreaCode = col["CustService.contactPhone.AreaCode"];
                    loc.CustService.contactPhone.Prefix = col["CustService.contactPhone.Prefix"];
                    loc.CustService.contactPhone.Suffix = col["CustService.contactPhone.Suffix"];
                    loc.CustService.strContactEmail = col["CustService.strContactEmail"];
                    loc.CustService.intContactTypeID = 3;

                    //Web Portal Information
                    loc.MainWeb = new Models.Website();
                    loc.MainWeb.intWebsiteTypeID = 1;
                    loc.MainWeb.strURL = col["MainWeb.strURL"];

                    loc.OrderingWeb = new Models.Website();
                    loc.OrderingWeb.intWebsiteTypeID = 2;
                    loc.OrderingWeb.strURL = col["OrderingWeb.strURL"];

                    loc.KettleWeb = new Models.Website();
                    loc.KettleWeb.intWebsiteTypeID = 3;
                    loc.KettleWeb.strURL = col["KettleWeb.strURL"];

					//Social Media Information
					loc.Facebook = new Models.SocialMedia() { strSocialMediaLink = col["Facebook.strSocialMediaLink"], intSocialMediaID = 1, blnAvailable = Convert.ToBoolean(col["Facebook.blnAvailable"].Split(',')[0]) };
					loc.Instagram = new Models.SocialMedia() { strSocialMediaLink = col["Instagram.strSocialMediaLink"], intSocialMediaID = 2, blnAvailable = Convert.ToBoolean(col["Instagram.blnAvailable"].Split(',')[0]) };
                    loc.Snapchat = new Models.SocialMedia() { strSocialMediaLink = col["Snapchat.strSocialMediaLink"], intSocialMediaID = 3, blnAvailable = Convert.ToBoolean(col["Snapchat.blnAvailable"].Split(',')[0]) };
                    loc.TikTok = new Models.SocialMedia() { strSocialMediaLink = col["TikTok.strSocialMediaLink"], intSocialMediaID = 4, blnAvailable = Convert.ToBoolean(col["TikTok.blnAvailable"].Split(',')[0]) };
                    loc.Twitter = new Models.SocialMedia() { strSocialMediaLink = col["Twitter.strSocialMediaLink"], intSocialMediaID = 5, blnAvailable = Convert.ToBoolean(col["Twitter.blnAvailable"].Split(',')[0]) };                   
                    loc.Yelp = new Models.SocialMedia() { strSocialMediaLink = col["Yelp.strSocialMediaLink"], intSocialMediaID = 6, blnAvailable = Convert.ToBoolean(col["Yelp.blnAvailable"].Split(',')[0]) };

                    //Extra Business Information
                    loc.BizYear = col["BizYear"];
                    loc.Bio = col["Bio"];

                    TempData["location"] = new Models.NewLocation();
                    TempData["location"] = loc;

                    //LIST MOST IMPORTANT TO GEOCODE
                    var location = new List<string>()
                    {
                        loc.LocationName, loc.StreetAddress + ' ' + loc.City + ' ' + loc.State + ' ' + loc.Zip
                    };

                    var businessInfo = new List<string>() {
                        loc.BusinessPhone.AreaCode + loc.BusinessPhone.Prefix + loc.BusinessPhone.Suffix,
                        loc.BusinessEmail,
                        loc.BizYear,
                        loc.Bio
                    };

                    var contacts = new List<Models.ContactPerson>()
                    {
                        loc.LocationContact, loc.WebAdmin, loc.CustService                       
                    };
                    
                    var socialmedia = new List<Models.SocialMedia>() {
                        loc.Facebook, loc.Twitter, loc.Instagram, loc.Snapchat, loc.TikTok, loc.Yelp
                    };

                    var categories = new List<Models.CategoryItem>() 
                    {
                        loc.Donuts, loc.Bagels, loc.Muffins, loc.IceCream, loc.FineCandies, loc.WeddingCakes, loc.Breads, loc.DecoratedCakes, loc.Cupcakes, loc.Cookies, loc.Desserts, loc.Full,
                        loc.Deli, loc.Other, loc.Wholesale, loc.Delivery, loc.Shipping, loc.Online
                    };

                    var LocationHours = new List<Models.Days>()
                    {
                        loc.Sunday, loc.Monday, loc.Tuesday, loc.Wednesday, loc.Thursday, loc.Friday, loc.Saturday
                    };

                    var Websites = new List<Models.Website>() {
                        loc.MainWeb, loc.OrderingWeb, loc.KettleWeb
                    };

                    Models.Database db = new Models.Database();
                    loc.lstStates = db.GetStates();

                    loc.lstCompanies = db.GetCompanies();
                    Models.Company nonValue = new Models.Company { intCompanyID = 0, strCompanyName = "Select Existing Company" };
                    loc.lstCompanies.Add(nonValue);

                    if (loc.CompanyName.Length == 0 && loc.lngCompanyID == 0) {
                        loc.ActionType = Models.NewLocation.ActionTypes.RequiredFieldsMissing;
                        return View(loc);
                    }

                    if (loc.LocationName.Length == 0 || loc.StreetAddress.Length == 0 || loc.City.Length == 0 || loc.intState == 0 || loc.Zip.Length == 0 ) {
                        loc.ActionType = Models.NewLocation.ActionTypes.RequiredFieldsMissing;
                        return View(loc);
                    }                    

                    Models.NewLocation.ActionTypes at = Models.NewLocation.ActionTypes.NoType;
                    at = loc.StoreNewLocation(categories, LocationHours, socialmedia, Websites, contacts);
                    switch (at) {
                        case Models.NewLocation.ActionTypes.InsertSuccessful:

                            ExportToCsv.StartExport(location, businessInfo, categories, LocationHours, contacts, socialmedia, Websites);

                            /*
                            using (firstProcess = new Process()) {
                                firstProcess.StartInfo.FileName = "C:\\Users\\winsl\\AppData\\Local\\ESRI\\conda\\envs\\myenv-py3v2\\python.exe";
                                firstProcess.StartInfo.CreateNoWindow = true;
                                firstProcess.StartInfo.Arguments = "C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\MVC\\Python\\Scripts\\geocode_arcgis_online.py";
                                firstProcess.Start();
                                firstProcess.WaitForExit();
                            }
                            */

                            return RedirectToAction("Index", "SendMailer", "location");
                        case Models.NewLocation.ActionTypes.DeleteSuccessful:
                            loc.ActionType = Models.NewLocation.ActionTypes.DeleteSuccessful;
                            return RedirectToAction("Index");
                        default:
                            return View(loc);

                    }
                }
                catch (Exception) {
                    Models.NewLocation loc = new Models.NewLocation();
                    Models.Database db = new Models.Database();
                    loc.lstStates = db.GetStates();

                    loc.lstCompanies = db.GetCompanies();
                    Models.Company nonValue = new Models.Company { intCompanyID = 0, strCompanyName = "Select Existing Company" };
                    loc.lstCompanies.Add(nonValue);

                    return View(loc);
                }
            }
            else {
                return View();
            }
        }
        
        public ActionResult Map() {
            return View();
		} 
    }
}