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
        List<Models.NewLocation> lstLocations = new List<Models.NewLocation>();

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
                results.lstCompanyMembers = db.GetCompanyMembers();

                foreach(Models.CompanyMember compMem in results.lstCompanyMembers) {
                    foreach(Models.NewLocation loc in results.lstLocations) {
                        if(loc.lngCompanyID == compMem.lngCompanyID) {
                            loc.isMember = true;
						}
					}
				}

                return View(results);
			}

            try {
                long lngSelectedLocID = Convert.ToInt64(col["btnSubmit"]);
                return RedirectToAction("LandingPage", new { id = lngSelectedLocID});
                //return RedirectToAction("TestDynamicPage");
            }
            catch {
                return View();
            }
        }

        public ActionResult TestDynamicPage() {
            return View();
		}

        public ActionResult LandingPage(long Id) {
            Models.Database db = new Models.Database();
            Models.SearchResults results = new Models.SearchResults();
            Models.NewLocation loc = new Models.NewLocation();
            
            results.landingLocation = db.GetLandingLocation(Id);
            results.lngLocationID = results.landingLocation.lngLocationID;
            results.landingLocation.memberStatus = db.CheckMemberStatus(results.landingLocation.lngLocationID);
            results.landingCategories = db.GetLandingCategories(Id);
            results.landingAwards = db.GetLandingAwards(Id);
            results.landingSpecials = db.GetLandingSpecials(Id);
            results.landingContacts = db.GetLandingContacts(Id);
            results.landingSocialMedia = db.GetLandingSocialMedia(Id);
            results.landingWebsite = db.GetLandingWebsite(Id);
            results.landingDays = db.GetLandingHours(Id);
            results.landingImages = db.GetLocationImages(Id);

            results.lstCompanyMembers = db.GetCompanyMembers();

            results.MainURL = results.landingWebsite.Where(r => r.intWebsiteTypeID.Equals(1)).Select(name => name.strURL).FirstOrDefault();
            results.OrderingURL = results.landingWebsite.Where(r => r.intWebsiteTypeID.Equals(2)).Select(name => name.strURL).FirstOrDefault();
            results.KettleURL = results.landingWebsite.Where(r => r.intWebsiteTypeID.Equals(3)).Select(name => name.strURL).FirstOrDefault();

            results.FacebookURL = results.landingSocialMedia.Where(s => s.intSocialMediaID.Equals(1)).Select(name => name.strSocialMediaLink).FirstOrDefault();
            results.InstagramURL = results.landingSocialMedia.Where(s => s.intSocialMediaID.Equals(2)).Select(name => name.strSocialMediaLink).FirstOrDefault();
            results.SnapchatURL = results.landingSocialMedia.Where(s => s.intSocialMediaID.Equals(3)).Select(name => name.strSocialMediaLink).FirstOrDefault();
            results.TikTokURL = results.landingSocialMedia.Where(s => s.intSocialMediaID.Equals(4)).Select(name => name.strSocialMediaLink).FirstOrDefault();
            results.TwitterURL = results.landingSocialMedia.Where(s => s.intSocialMediaID.Equals(5)).Select(name => name.strSocialMediaLink).FirstOrDefault();
            results.YelpURL = results.landingSocialMedia.Where(s => s.intSocialMediaID.Equals(6)).Select(name => name.strSocialMediaLink).FirstOrDefault();

            results.strSpecialStartDisplay = results.landingSpecials.Where(m=>m.dtmStart != null).Select(dt=>dt.dtmStart).FirstOrDefault().ToShortDateString();
            results.strSpecialEndDisplay = results.landingSpecials.Where(m=>m.dtmEnd != null).Select(dt=>dt.dtmEnd).FirstOrDefault().ToShortDateString();

            return View(results);
        }

        [HttpPost]
        public ActionResult LandingPage(FormCollection col) {
            if (col["btnSubmit"] == "deleteLocation") {
                Models.NewLocation loc = new Models.NewLocation();
                loc.lngLocationID = Convert.ToInt64(col["lngLocationID"]);
                Models.Database db = new Models.Database();
                loc.ActionType = Models.NewLocation.ActionTypes.NoType;
                loc.ActionType = db.DeleteLocation(loc.lngLocationID, loc.lngCompanyID);
                return RedirectToAction("Index", "Bakery");
            }
            return View();
		}

        public ActionResult MemberBakery(long Id) {
            return View();
		}

        public ActionResult AddNewLocation() {
            Models.Database db = new Models.Database();
            Models.LocationList locList = new Models.LocationList();


            Models.NewLocation loc = new Models.NewLocation();

            loc.lstStates = db.GetStates();

            loc.lstCompanies = db.GetCompanies();
            Models.Company nonValue = new Models.Company { CompanyID = 0, Name = "Select Existing Company" };
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

            locList.lstLocations = new Models.NewLocation[] { loc };

			return View(locList);
        }

        [HttpPost]
        public ActionResult AddNewLocation(FormCollection col) {
            try {
                int indexPopulated = 0;
                List<string>[] arrLocInfo = new List<string>[10];
                List<string>[] arrBizInfo = new List<string>[10];
                List<Models.ContactPerson>[] arrContactInfo = new List<Models.ContactPerson>[10];
                List<Models.SocialMedia>[] arrSocialMediaInfo = new List<Models.SocialMedia>[10];
                List<Models.CategoryItem>[] arrCategoryInfo = new List<Models.CategoryItem>[10];
                List<Models.Days>[] arrLocHours = new List<Models.Days>[10];
                List<Models.Website>[] arrWebsites = new List<Models.Website>[10];

                Models.LocationList locList = new Models.LocationList();
                
                for (int index = 0; index <= 99; index++) {
                    if(col["lstLocations[" + index + "].LocationName"] != null) {
                        //Add this information in case error and send back to view - company and state list need to be populated
                        Models.NewLocation loc = new Models.NewLocation();
                        Models.Database db = new Models.Database();
                        loc.lstStates = db.GetStates();
                        loc.lstCompanies = db.GetCompanies();
                        Models.Company nonValue = new Models.Company { CompanyID = 0, Name = "Select Existing Company" };
                        loc.lstCompanies.Add(nonValue);

                        //Check to see if preselected dropdown was selected
                        if (Convert.ToInt64(col["lstLocations[0].lngCompanyID"]) != 0) {
                            loc.lngCompanyID = Convert.ToInt64(col["lstLocations[0].lngCompanyID"]);
                        }

                        if (loc.lngCompanyID == 0) {
                            loc.CompanyName = col["lstLocations[0].CompanyName"];
                        }
                        else {
                            loc.CompanyName = string.Empty;
						}
                        
                        loc.LocationName = col["lstLocations[" + index + "].LocationName"];
                        loc.StreetAddress = col["lstLocations[" + index + "].StreetAddress"];
                        loc.City = col["lstLocations[" + index + "].City"];
                        loc.intState = Convert.ToInt16(col["lstLocations[" + index + "].intState"]);
                        loc.Zip = col["lstLocations[" + index + "].Zip"];

                        //Hours of Operation
                        if (col["lstLocations[" + index + "].Sunday.blnOperational"] == null) loc.Sunday = new Models.Days() { strDay = "Sunday", intDayID = 1, blnOperational = false, strOpenTime = col["lstLocations[" + index + "].Sunday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Sunday.strClosedTime"] };
                        else loc.Sunday = new Models.Days() { strDay = "Sunday", intDayID = 1, blnOperational = Convert.ToBoolean(col["lstLocations[" + index + "].Sunday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + index + "].Sunday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Sunday.strClosedTime"] };

                        if (col["lstLocations[" + index + "].Monday.blnOperational"] == null) loc.Monday = new Models.Days() { strDay = "Monday", intDayID = 2, blnOperational = false, strOpenTime = col["lstLocations[" + index + "].Monday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Monday.strClosedTime"] };
                        else loc.Monday = new Models.Days() { strDay = "Monday", intDayID = 2, blnOperational = Convert.ToBoolean(col["lstLocations[" + index + "].Monday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + index + "].Monday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Monday.strClosedTime"] };

                        if (col["lstLocations[" + index + "].Tuesday.blnOperational"] == null) loc.Tuesday = new Models.Days() { strDay = "Tuesday", intDayID = 3, blnOperational = false, strOpenTime = col["lstLocations[" + index + "].Tuesday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Tuesday.strClosedTime"] };
                        else loc.Tuesday = new Models.Days() { strDay = "Tuesday", intDayID = 3, blnOperational = Convert.ToBoolean(col["lstLocations[" + index + "].Tuesday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + index + "].Tuesday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Tuesday.strClosedTime"] };

                        if (col["lstLocations[" + index + "].Wednesday.blnOperational"] == null) loc.Wednesday = new Models.Days() { strDay = "Wednesday", intDayID = 4, blnOperational = false, strOpenTime = col["lstLocations[" + index + "].Wednesday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Wednesday.strClosedTime"] };
                        else loc.Wednesday = new Models.Days() { strDay = "Wednesday", intDayID = 4, blnOperational = Convert.ToBoolean(col["lstLocations[" + index + "].Wednesday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + index + "].Wednesday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Wednesday.strClosedTime"] };

                        if (col["lstLocations[" + index + "].Thursday.blnOperational"] == null) loc.Thursday = new Models.Days() { strDay = "Thursday", intDayID = 5, blnOperational = false, strOpenTime = col["lstLocations[" + index + "].Thursday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Thursday.strClosedTime"] };
                        else loc.Thursday = new Models.Days() { strDay = "Thursday", intDayID = 5, blnOperational = Convert.ToBoolean(col["lstLocations[" + index + "].Thursday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + index + "].Thursday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Thursday.strClosedTime"] };

                        if (col["lstLocations[" + index + "].Friday.blnOperational"] == null) loc.Friday = new Models.Days() { strDay = "Friday", intDayID = 6, blnOperational = false, strOpenTime = col["lstLocations[" + index + "].Friday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Friday.strClosedTime"] };
                        else loc.Friday = new Models.Days() { strDay = "Friday", intDayID = 6, blnOperational = Convert.ToBoolean(col["lstLocations[" + index + "].Friday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + index + "].Friday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Friday.strClosedTime"] };

                        if (col["lstLocations[" + index + "].Saturday.blnOperational"] == null) loc.Saturday = new Models.Days() { strDay = "Saturday", intDayID = 7, blnOperational = false, strOpenTime = col["lstLocations[" + index + "].Saturday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Saturday.strClosedTime"] };
                        else loc.Saturday = new Models.Days() { strDay = "Saturday", intDayID = 7, blnOperational = Convert.ToBoolean(col["lstLocations[" + index + "].Saturday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + index + "].Saturday.strOpenTime"], strClosedTime = col["lstLocations[" + index + "].Saturday.strClosedTime"] };

                        //Product Categories
                        if (col["lstLocations[" + index + "].Donuts.blnAvailable"] == null) loc.Donuts = new Models.CategoryItem() { ItemID = 1, ItemDesc = "Donuts", blnAvailable = false };
                        else loc.Donuts = new Models.CategoryItem() { ItemID = 1, ItemDesc = "Donuts", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Donuts.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Bagels.blnAvailable"] == null) loc.Bagels = new Models.CategoryItem() { ItemID = 2, ItemDesc = "Bagels", blnAvailable = false };
                        else loc.Bagels = new Models.CategoryItem() { ItemID = 2, ItemDesc = "Bagels", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Bagels.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Muffins.blnAvailable"] == null) loc.Muffins = new Models.CategoryItem() { ItemID = 3, ItemDesc = "Muffins", blnAvailable = false };
                        else loc.Muffins = new Models.CategoryItem() { ItemID = 3, ItemDesc = "Muffins", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Muffins.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].IceCream.blnAvailable"] == null) loc.IceCream = new Models.CategoryItem() { ItemID = 4, ItemDesc = "IceCream", blnAvailable = false };
                        else loc.IceCream = new Models.CategoryItem() { ItemID = 4, ItemDesc = "Ice Cream", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].IceCream.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].FineCandies.blnAvailable"] == null) loc.FineCandies = new Models.CategoryItem() { ItemID = 5, ItemDesc = "FineCandies", blnAvailable = false };
                        else loc.FineCandies = new Models.CategoryItem() { ItemID = 5, ItemDesc = "Fine Candies & Chocolates", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].FineCandies.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].WeddingCakes.blnAvailable"] == null) loc.WeddingCakes = new Models.CategoryItem() { ItemID = 6, ItemDesc = "WeddingCakes", blnAvailable = false };
                        else loc.WeddingCakes = new Models.CategoryItem() { ItemID = 6, ItemDesc = "Wedding Cakes", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].WeddingCakes.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Breads.blnAvailable"] == null) loc.Breads = new Models.CategoryItem() { ItemID = 7, ItemDesc = "Breads", blnAvailable = false };
                        else loc.Breads = new Models.CategoryItem() { ItemID = 7, ItemDesc = "Breads", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Breads.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].DecoratedCakes.blnAvailable"] == null) loc.DecoratedCakes = new Models.CategoryItem() { ItemID = 8, ItemDesc = "DecoratedCakes", blnAvailable = false };
                        else loc.DecoratedCakes = new Models.CategoryItem() { ItemID = 8, ItemDesc = "Decorated Cakes", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].DecoratedCakes.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Cupcakes.blnAvailable"] == null) loc.Cupcakes = new Models.CategoryItem() { ItemID = 9, ItemDesc = "Cupcakes", blnAvailable = false };
                        else loc.Cupcakes = new Models.CategoryItem() { ItemID = 9, ItemDesc = "Cupcakes", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Cupcakes.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Cookies.blnAvailable"] == null) loc.Cookies = new Models.CategoryItem() { ItemID = 10, ItemDesc = "Cookies", blnAvailable = false };
                        else loc.Cookies = new Models.CategoryItem() { ItemID = 10, ItemDesc = "Cookies", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Cookies.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Desserts.blnAvailable"] == null) loc.Desserts = new Models.CategoryItem() { ItemID = 11, ItemDesc = "Desserts", blnAvailable = false };
                        else loc.Desserts = new Models.CategoryItem() { ItemID = 11, ItemDesc = "Desserts/Tortes", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Desserts.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Full.blnAvailable"] == null) loc.Full = new Models.CategoryItem() { ItemID = 12, ItemDesc = "Full", blnAvailable = false };
                        else loc.Full = new Models.CategoryItem() { ItemID = 12, ItemDesc = "Full-line Bakery", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Full.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Deli.blnAvailable"] == null) loc.Deli = new Models.CategoryItem() { ItemID = 13, ItemDesc = "Deli", blnAvailable = false };
                        else loc.Deli = new Models.CategoryItem() { ItemID = 13, ItemDesc = "Deli/Catering", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Deli.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Other.blnAvailable"] == null) loc.Other = new Models.CategoryItem() { ItemID = 14, ItemDesc = "Other", blnAvailable = false };
                        else loc.Other = new Models.CategoryItem() { ItemID = 14, ItemDesc = "Other Carryout Deli", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Other.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Wholesale.blnAvailable"] == null) loc.Wholesale = new Models.CategoryItem() { ItemID = 15, ItemDesc = "Wholesale", blnAvailable = false };
                        else loc.Wholesale = new Models.CategoryItem() { ItemID = 15, ItemDesc = "Wholesale", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Wholesale.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Delivery.blnAvailable"] == null) loc.Delivery = new Models.CategoryItem() { ItemID = 16, ItemDesc = "Delivery", blnAvailable = false };
                        else loc.Delivery = new Models.CategoryItem() { ItemID = 16, ItemDesc = "Delivery (3rd Party)", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Delivery.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Shipping.blnAvailable"] == null) loc.Shipping = new Models.CategoryItem() { ItemID = 17, ItemDesc = "Shipping", blnAvailable = false };
                        else loc.Shipping = new Models.CategoryItem() { ItemID = 17, ItemDesc = "Shipping", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Shipping.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Online.blnAvailable"] == null) loc.Online = new Models.CategoryItem() { ItemID = 18, ItemDesc = "Online", blnAvailable = false };
                        else loc.Online = new Models.CategoryItem() { ItemID = 18, ItemDesc = "Online Ordering", blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Online.blnAvailable"].Split(',')[0]) };

                        //Business Contact Information
                        loc.BusinessPhone = new Models.PhoneNumber();
                        loc.BusinessPhone.AreaCode = col["lstLocations[" + index + "].BusinessPhone.AreaCode"];
                        loc.BusinessPhone.Prefix = col["lstLocations[" + index + "].BusinessPhone.Prefix"];
                        loc.BusinessPhone.Suffix = col["lstLocations[" + index + "].BusinessPhone.Suffix"];
                        loc.BusinessEmail = col["lstLocations[" + index + "].BusinessEmail"];

                        //Member Only Variables
                        //Contact Person Information
                        loc.LocationContact = new Models.ContactPerson();
                        loc.LocationContact.strContactFirstName = col["lstLocations[" + index + "].LocationContact.strContactFirstName"];
                        loc.LocationContact.strContactLastName = col["lstLocations[" + index + "].LocationContact.strContactLastName"];
                        loc.LocationContact.contactPhone = new Models.PhoneNumber();
                        loc.LocationContact.contactPhone.AreaCode = col["lstLocations[" + index + "].LocationContact.contactPhone.AreaCode"];
                        loc.LocationContact.contactPhone.Prefix = col["lstLocations[" + index + "].LocationContact.contactPhone.Prefix"];
                        loc.LocationContact.contactPhone.Suffix = col["lstLocations[" + index + "].LocationContact.contactPhone.Suffix"];
                        loc.LocationContact.strContactEmail = col["lstLocations[" + index + "].LocationContact.strContactEmail"];
                        loc.LocationContact.intContactTypeID = (short)Models.ContactPerson.ContactTypes.LocationContact;

                        //Web Admin contact information
                        loc.WebAdmin = new Models.ContactPerson();
                        loc.WebAdmin.strContactFirstName = col["lstLocations[" + index + "].WebAdmin.strContactFirstName"];
                        loc.WebAdmin.strContactLastName = col["lstLocations[" + index + "].WebAdmin.strContactLastName"];
                        loc.WebAdmin.contactPhone = new Models.PhoneNumber();
                        loc.WebAdmin.contactPhone.AreaCode = col["lstLocations[" + index + "].WebAdmin.contactPhone.AreaCode"];
                        loc.WebAdmin.contactPhone.Prefix = col["lstLocations[" + index + "].WebAdmin.contactPhone.Prefix"];
                        loc.WebAdmin.contactPhone.Suffix = col["lstLocations[" + index + "].WebAdmin.contactPhone.Suffix"];
                        loc.WebAdmin.strContactEmail = col["lstLocations[" + index + "].WebAdmin.strContactEmail"];
                        loc.WebAdmin.intContactTypeID = (short)Models.ContactPerson.ContactTypes.WebAdmin;

                        //Customer Service Contact Information
                        loc.CustService = new Models.ContactPerson();
                        loc.CustService.strContactFirstName = col["lstLocations[" + index + "].CustService.strContactFirstName"];
                        loc.CustService.strContactLastName = col["lstLocations[" + index + "].CustService.strContactLastName"];
                        loc.CustService.contactPhone = new Models.PhoneNumber();
                        loc.CustService.contactPhone.AreaCode = col["lstLocations[" + index + "].CustService.contactPhone.AreaCode"];
                        loc.CustService.contactPhone.Prefix = col["lstLocations[" + index + "].CustService.contactPhone.Prefix"];
                        loc.CustService.contactPhone.Suffix = col["lstLocations[" + index + "].CustService.contactPhone.Suffix"];
                        loc.CustService.strContactEmail = col["lstLocations[" + index + "].CustService.strContactEmail"];
                        loc.CustService.intContactTypeID = (short)Models.ContactPerson.ContactTypes.CustomerService;

                        //Web Portal Information
                        loc.MainWeb = new Models.Website();
                        loc.MainWeb.intWebsiteTypeID = 1;
                        loc.MainWeb.strURL = col["lstLocations[" + index + "].MainWeb.strURL"];

                        loc.OrderingWeb = new Models.Website();
                        loc.OrderingWeb.intWebsiteTypeID = 2;
                        loc.OrderingWeb.strURL = col["lstLocations[" + index + "].OrderingWeb.strURL"];

                        loc.KettleWeb = new Models.Website();
                        loc.KettleWeb.intWebsiteTypeID = 3;
                        loc.KettleWeb.strURL = col["lstLocations[" + index + "].KettleWeb.strURL"];

                        //Social Media Information
                        if (col["lstLocations[" + index + "].Facebook.blnAvailable"] == null) loc.Facebook = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].Facebook.strSocialMediaLink"], intSocialMediaID = 1, blnAvailable = false };
                        else loc.Facebook = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].Facebook.strSocialMediaLink"], intSocialMediaID = 1, blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Facebook.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Instagram.blnAvailable"] == null) loc.Instagram = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].Instagram.strSocialMediaLink"], intSocialMediaID = 2, blnAvailable = false };
                        else loc.Instagram = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].Instagram.strSocialMediaLink"], intSocialMediaID = 2, blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Instagram.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Snapchat.blnAvailable"] == null) loc.Snapchat = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].Snapchat.strSocialMediaLink"], intSocialMediaID = 3, blnAvailable = false };
                        else loc.Snapchat = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].Snapchat.strSocialMediaLink"], intSocialMediaID = 3, blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Snapchat.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].TikTok.blnAvailable"] == null) loc.TikTok = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].TikTok.strSocialMediaLink"], intSocialMediaID = 4, blnAvailable = false };
                        else loc.TikTok = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].TikTok.strSocialMediaLink"], intSocialMediaID = 4, blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].TikTok.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Twitter.blnAvailable"] == null) loc.Twitter = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].Twitter.strSocialMediaLink"], intSocialMediaID = 5, blnAvailable = false };
                        else loc.Twitter = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].Twitter.strSocialMediaLink"], intSocialMediaID = 5, blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Twitter.blnAvailable"].Split(',')[0]) };

                        if (col["lstLocations[" + index + "].Yelp.blnAvailable"] == null) loc.Yelp = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].Yelp.strSocialMediaLink"], intSocialMediaID = 6, blnAvailable = false };
                        else loc.Yelp = new Models.SocialMedia() { strSocialMediaLink = col["lstLocations[" + index + "].Yelp.strSocialMediaLink"], intSocialMediaID = 6, blnAvailable = Convert.ToBoolean(col["lstLocations[" + index + "].Yelp.blnAvailable"].Split(',')[0]) };

                        //Extra Business Information
                        loc.BizYear = col["lstLocations[" + index + "].BizYear"];
                        loc.Bio = col["lstLocations[" + index + "].Bio"];


                        if (loc.CompanyName.Length == 0 && loc.lngCompanyID == 0) {
                            loc.lstStates = db.GetStates();
                            loc.lstCompanies = db.GetCompanies();
                            loc.lstCompanies.Add(nonValue);
                            locList.lstLocations = new Models.NewLocation[] { loc };
                            locList.ActionType = Models.LocationList.ActionTypes.CompanyFieldsMissing;
                            return View(locList);
                        }

                        if (loc.LocationName.Length == 0 || loc.StreetAddress.Length == 0 || loc.City.Length == 0 || loc.intState == 0 || loc.Zip.Length == 0) {
                            loc.lstStates = db.GetStates();
                            loc.lstCompanies = db.GetCompanies();
                            loc.lstCompanies.Add(nonValue);
                            locList.lstLocations = new Models.NewLocation[] { loc };
                            locList.ActionType = Models.LocationList.ActionTypes.RequiredFieldsMissing;
                            return View(locList);
                        }

                        //Store Categories in List for Category Check
                        var categories = new List<Models.CategoryItem>() {
                        loc.Donuts, loc.Bagels, loc.Muffins, loc.IceCream, loc.FineCandies, loc.WeddingCakes, loc.Breads, loc.DecoratedCakes, loc.Cupcakes, loc.Cookies, loc.Desserts, loc.Full,
                        loc.Deli, loc.Other, loc.Wholesale, loc.Delivery, loc.Shipping, loc.Online
                        };

                        bool CategoryCheck = false;
                        foreach (Models.CategoryItem item in categories) {
                            if (item.blnAvailable == false) continue;
                            else CategoryCheck = true;
						}
                        if(CategoryCheck == false) {
                            loc.lstStates = db.GetStates();
                            loc.lstCompanies = db.GetCompanies();
                            loc.lstCompanies.Add(nonValue);
                            locList.lstLocations = new Models.NewLocation[] { loc };
                            locList.ActionType = Models.LocationList.ActionTypes.CategoryFieldsMissing;
                            return View(locList);
						}
                        //If good, store information in array
                        arrCategoryInfo[indexPopulated] = categories;

                        //Store Location Hours in List for Hours Check
                        var LocationHours = new List<Models.Days>() {
                        loc.Sunday, loc.Monday, loc.Tuesday, loc.Wednesday, loc.Thursday, loc.Friday, loc.Saturday
                        };

                        bool HoursCheck = false;
                        foreach (Models.Days item in LocationHours) {
                            if (item.blnOperational == false) continue;
                            else HoursCheck = true;
                        }
                        if (HoursCheck == false) {
                            loc.lstStates = db.GetStates();
                            loc.lstCompanies = db.GetCompanies();
                            loc.lstCompanies.Add(nonValue);
                            locList.lstLocations = new Models.NewLocation[] { loc };
                            locList.ActionType = Models.LocationList.ActionTypes.HoursFieldsMissing;
                            return View(locList);
                        }
                        //If good, store information in array
                        arrLocHours[indexPopulated] = LocationHours;

                        //ADDRESS MOST IMPORTANT TO GEOCODE
                        var location = new List<string>() {
                        loc.LocationName, loc.StreetAddress + ' ' + loc.City + ' ' + loc.State + ' ' + loc.Zip
                        };
                        arrLocInfo[indexPopulated] = location;

                        var businessInfo = new List<string>() {
                        loc.BusinessPhone.AreaCode + loc.BusinessPhone.Prefix + loc.BusinessPhone.Suffix,
                        loc.BusinessEmail,
                        loc.BizYear,
                        loc.Bio
                        };
                        arrBizInfo[indexPopulated] = businessInfo;

                        var contacts = new List<Models.ContactPerson>()
                        {
                        loc.LocationContact, loc.WebAdmin, loc.CustService
                    };
                        arrContactInfo[indexPopulated] = contacts;

                        var socialmedia = new List<Models.SocialMedia>() {
                        loc.Facebook, loc.Twitter, loc.Instagram, loc.Snapchat, loc.TikTok, loc.Yelp
                    };
                        arrSocialMediaInfo[indexPopulated] = socialmedia;

                        var Websites = new List<Models.Website>() {
                        loc.MainWeb, loc.OrderingWeb, loc.KettleWeb
                    };
                        arrWebsites[indexPopulated] = Websites;

                        //Store location inside list of locations
                        locList.lstLocations[indexPopulated] = loc;
                        indexPopulated += 1;
                    }
				}
                

                if (col["btnSubmit"].ToString() == "SubmitLocation") {
                    
                    //Store temp information to be sent to the email sender
                    TempData["location"] = new List<Models.NewLocation>();
                    TempData["location"] = locList;

                    Models.LocationList.ActionTypes at = Models.LocationList.ActionTypes.NoType;
                    at = locList.StoreTempNewLocation(arrCategoryInfo, arrLocHours, arrSocialMediaInfo, arrWebsites, arrContactInfo);
                    switch (at) {
                        case Models.LocationList.ActionTypes.InsertSuccessful:
                            
                            ExportToCsv.StartExport(locList, arrLocInfo, arrBizInfo, arrCategoryInfo, arrLocHours, arrContactInfo, arrSocialMediaInfo, arrWebsites);
                            SendLocationEmail.SendEmail(locList);
                            return RedirectToAction("Index", "Bakery");
							

                        case Models.LocationList.ActionTypes.DeleteSuccessful:
                            return RedirectToAction("Index");
                        default:
                            Models.Database db = new Models.Database();                           

                            Models.NewLocation loc = new Models.NewLocation();

                            loc.lstStates = db.GetStates();

                            loc.lstCompanies = db.GetCompanies();
                            Models.Company nonValue = new Models.Company { CompanyID = 0, Name = "Select Existing Company" };
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

                            locList.lstLocations = new Models.NewLocation[] { loc };
                            locList.ActionType = at;
                            return View(locList);
                    }
                }

                return View(locList);
            }
            catch (Exception) {
                Models.LocationList locList = new Models.LocationList();
                Models.NewLocation loc = new Models.NewLocation();
                Models.Database db = new Models.Database();
                loc.lstStates = db.GetStates();

                loc.lstCompanies = db.GetCompanies();
                Models.Company nonValue = new Models.Company { CompanyID = 0, Name = "Select Existing Company" };
                loc.lstCompanies.Add(nonValue);
                locList.lstLocations = new Models.NewLocation[] { loc };
                return View(locList);
            }
        }
        
        public ActionResult Map() {
            return View();
		} 
    }
}