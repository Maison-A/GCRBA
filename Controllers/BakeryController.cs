using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
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
        private Process firstProcess;
        //private Process secondProcess;
        //private Process thirdProcess;

        // GET: Bakery
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection col) {
            if (col["btnSubmit"].ToString() == "addLocation") {
                return RedirectToAction("AddNewLocation");
            }
            else return View();
        }

        public ActionResult AddNewLocation() {

            Models.NewLocation loc = new Models.NewLocation();
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
                    loc.LocationName = col["LocationName"];
                    loc.StreetAddress = col["StreetAddress"];
                    loc.City = col["City"];
                    loc.State = col["State"];
                    loc.Zip = col["Zip"];

                    //Hours of Operation
                    loc.Sunday = new Models.Days() { strDay = "Sunday", intDayID = 1, blnOperational = Convert.ToBoolean(col["Sunday.blnOperational"].Split(',')[0]), strOpenTime = col["Sunday.strOpenTime"], strClosedTime = col["Sunday.strClosedTime"] };
                    loc.Monday = new Models.Days() { strDay = "Monday", intDayID = 2, blnOperational = Convert.ToBoolean(col["Monday.blnOperational"].Split(',')[0]), strOpenTime = col["Monday.strOpenTime"], strClosedTime = col["Monday.strClosedTime"] };
                    loc.Tuesday = new Models.Days() { strDay = "Tuesday", intDayID = 3, blnOperational = Convert.ToBoolean(col["Tuesday.blnOperational"].Split(',')[0]), strOpenTime = col["Tuesday.strOpenTime"], strClosedTime = col["Tuesday.strClosedTime"] };
                    loc.Wednesday = new Models.Days() { strDay = "Wednesday", intDayID = 4, blnOperational = Convert.ToBoolean(col["Wednesday.blnOperational"].Split(',')[0]), strOpenTime = col["Wednesday.strOpenTime"], strClosedTime = col["Wednesday.strClosedTime"] };
                    loc.Thursday = new Models.Days() { strDay = "Thursday", intDayID = 5, blnOperational = Convert.ToBoolean(col["Thursday.blnOperational"].Split(',')[0]), strOpenTime = col["Thursday.strOpenTime"], strClosedTime = col["Thursday.strClosedTime"] };
                    loc.Friday = new Models.Days() { strDay = "Friday", intDayID = 6, blnOperational = Convert.ToBoolean(col["Friday.blnOperational"].Split(',')[0]), strOpenTime = col["Friday.strOpenTime"], strClosedTime = col["Friday.strClosedTime"] };
                    loc.Saturday = new Models.Days() { strDay = "Saturday", intDayID = 7, blnOperational = Convert.ToBoolean(col["Saturday.blnOperational"].Split(',')[0]), strOpenTime = col["Saturday.strOpenTime"], strClosedTime = col["Saturday.strClosedTime"] };

                    loc.Donuts = new Models.CategoryItem() { ItemID = 1, ItemDesc = "Donuts", blnAvailable = Convert.ToBoolean(col["Donuts.blnAvailable"].Split(',')[0]) };
                    loc.Bagels = new Models.CategoryItem() { ItemID = 2, ItemDesc = "Bagels", blnAvailable = Convert.ToBoolean(col["Bagels.blnAvailable"].Split(',')[0]) };
                    loc.Muffins = new Models.CategoryItem() { ItemID = 3, ItemDesc = "Muffins", blnAvailable = Convert.ToBoolean(col["Muffins.blnAvailable"].Split(',')[0]) };
                    loc.IceCream = new Models.CategoryItem() { ItemID = 4, ItemDesc = "Ice Cream", blnAvailable = Convert.ToBoolean(col["IceCream.blnAvailable"].Split(',')[0]) };
                    loc.FineCandies = new Models.CategoryItem() { ItemID = 5, ItemDesc = "Fine Candies & Chocolates", blnAvailable = Convert.ToBoolean(col["Donuts.blnAvailable"].Split(',')[0]) };
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

                    loc.BusinessPhone = new Models.PhoneNumber();
                    loc.BusinessPhone.AreaCode = col["BusinessPhone.AreaCode"];
                    loc.BusinessPhone.Prefix = col["BusinessPhone.Prefix"];
                    loc.BusinessPhone.Suffix = col["BusinessPhone.Suffix"];
                    loc.BusinessEmail = col["BusinessEmail"];

                    //Member Only Variables
                    loc.ContactPerson.strContactFirstName = col["contactFirstName"];
                    loc.ContactPerson.strContactLastName = col["contactLastName"];
                    loc.ContactPerson.contactPhone = new Models.PhoneNumber();
                    loc.ContactPerson.contactPhone.AreaCode = col["ContactPhone.AreaCode"];
                    loc.ContactPerson.contactPhone.Prefix = col["ContactPhone.Prefix"];
                    loc.ContactPerson.contactPhone.Suffix = col["ContactPhone.Suffix"];
                    loc.ContactPerson.strContactEmail = col["ContactPerson.strContactEmail"];

                    loc.WebAdmin.strContactFirstName = col["WebAdmin.strContactFirstName"];
                    loc.WebAdmin.strContactLastName = col["WebAdmin.strContactLastName"];
                    loc.WebAdmin.contactPhone = new Models.PhoneNumber();
                    loc.WebAdmin.contactPhone.AreaCode = col["WebAdmin.AreaCode"];
                    loc.WebAdmin.contactPhone.Prefix = col["WebAdmin.Prefix"];
                    loc.WebAdmin.contactPhone.Suffix = col["WebAdmin.Suffix"];
                    loc.WebAdmin.strContactEmail = col["WebAdmin.strContactEmail"];

                    loc.CustService.strContactFirstName = col["CustService.strContactFirstName"];
                    loc.CustService.strContactLastName = col["CustService.strContactLastName"];
                    loc.CustService.contactPhone = new Models.PhoneNumber();
                    loc.CustService.contactPhone.AreaCode = col["CustService.AreaCode"];
                    loc.CustService.contactPhone.Prefix = col["CustService.Prefix"];
                    loc.CustService.contactPhone.Suffix = col["CustService.Suffix"];
                    loc.CustService.strContactEmail = col["CustService.strContactEmail"];

                    loc.MainWeb.strURL = col["MainWeb.strURL"];
                    loc.OrderingWeb.strURL = col["OrderingWeb.strURL"];
                    loc.KettleWeb.strURL = col["KettleWeb.strURL"];

                    loc.Facebook = new Models.SocialMedia() { strSocialMediaLink = col["Facebook.strSocialMediaLink"], intSocialMediaID = 1 };
                    loc.Instagram = new Models.SocialMedia() { strSocialMediaLink = col["Instagram.strSocialMediaLink"], intSocialMediaID = 2 };
                    loc.Snapchat = new Models.SocialMedia() { strSocialMediaLink = col["Snapchat.strSocialMediaLink"], intSocialMediaID = 3 };
                    loc.TikTok = new Models.SocialMedia() { strSocialMediaLink = col["TikTok.strSocialMediaLink"], intSocialMediaID = 4 };
                    loc.Twitter = new Models.SocialMedia() { strSocialMediaLink = col["Twitter.strSocialMediaLink"], intSocialMediaID = 5 };                   
                    loc.Yelp = new Models.SocialMedia() { strSocialMediaLink = col["Yelp.strSocialMediaLink"], intSocialMediaID = 6 };


                    loc.custServiceEmail = col["CustServEmail"];
                    loc.BizYear = col["BizYear"];
                    loc.Bio = col["Bio"];

                    var location = new List<string>()
                    {
                        loc.LocationName, loc.StreetAddress + ' ' + loc.City + ' ' + loc.State + ' ' + loc.Zip
                    };

                    //For GIS listing
                    var contact = new List<string>()
                    {
                        loc.ContactPerson.strContactLastName + ", " + loc.ContactPerson.strContactFirstName,
                        loc.ContactPerson.contactPhone.AreaCode + loc.ContactPerson.contactPhone.Prefix + loc.ContactPerson.contactPhone.Suffix,
                        loc.ContactPerson.strContactEmail                        
                    };

                    //Both GIS and Database listing
                    var socialmedia = new List<Models.SocialMedia>() {
                        loc.Facebook, loc.Twitter, loc.Instagram, loc.Snapchat, loc.TikTok, loc.Yelp
                    };

                    //For GIS listing
                    var specialties = new List<string>()
                    {
                        Convert.ToString(loc.Donuts.blnAvailable), Convert.ToString(loc.Bagels.blnAvailable), Convert.ToString(loc.Muffins.blnAvailable), Convert.ToString(loc.IceCream.blnAvailable),
                        Convert.ToString(loc.FineCandies.blnAvailable), Convert.ToString(loc.WeddingCakes.blnAvailable), Convert.ToString(loc.Breads.blnAvailable), Convert.ToString(loc.DecoratedCakes.blnAvailable),
                        Convert.ToString(loc.Cupcakes.blnAvailable), Convert.ToString(loc.Cookies.blnAvailable), Convert.ToString(loc.Desserts.blnAvailable), Convert.ToString(loc.Full.blnAvailable),
                        Convert.ToString(loc.Deli.blnAvailable), Convert.ToString(loc.Other.blnAvailable), Convert.ToString(loc.Wholesale.blnAvailable), Convert.ToString(loc.Delivery.blnAvailable),
                        Convert.ToString(loc.Shipping.blnAvailable), Convert.ToString(loc.Online.blnAvailable)
                    };

                    //For Database listing
                    var categories = new List<Models.CategoryItem>() 
                    {
                        loc.Donuts, loc.Bagels, loc.Muffins, loc.IceCream, loc.FineCandies, loc.WeddingCakes, loc.Breads, loc.DecoratedCakes, loc.Cupcakes, loc.Cookies, loc.Desserts, loc.Full,
                        loc.Deli, loc.Other, loc.Wholesale, loc.Delivery, loc.Shipping, loc.Online
                    };

                    //For GIS listing
                    var operations = new List<string>()
                    {
                        loc.Sunday.strOpenTime +'-' + loc.Sunday.strClosedTime, loc.Monday.strOpenTime + '-' + loc.Monday.strClosedTime,
                        loc.Tuesday.strOpenTime + '-' + loc.Tuesday.strClosedTime, loc.Wednesday.strOpenTime + '-' + loc.Wednesday.strClosedTime,
                        loc.Thursday.strOpenTime + '-' + loc.Thursday.strClosedTime, loc.Friday.strOpenTime + '-' + loc.Friday.strClosedTime,
                        loc.Saturday.strOpenTime + '-' + loc.Saturday.strClosedTime
                    };

                    //For Database listing
                    var LocationHours = new List<Models.Days>()
                    {
                        loc.Sunday, loc.Monday, loc.Tuesday, loc.Wednesday, loc.Thursday, loc.Friday, loc.Saturday
                    };

                    if (loc.LocationName.Length == 0 || loc.StreetAddress.Length == 0 || loc.City.Length == 0 || loc.State.Length == 0 || loc.Zip.Length == 0) {
                        loc.ActionType = Models.NewLocation.ActionTypes.RequiredFieldsMissing;
                        return View(loc);
                    }

                    Models.NewLocation.ActionTypes at = Models.NewLocation.ActionTypes.NoType;
                    at = loc.StoreNewLocation(categories, LocationHours, socialmedia);
                    switch (at) {
                        case Models.NewLocation.ActionTypes.InsertSuccessful:
                            loc.ActionType = Models.NewLocation.ActionTypes.InsertSuccessful;

                            ExportToCsv.Export(location, contact, specialties, socialmedia);

                            using (firstProcess = new Process()) {
                                firstProcess.StartInfo.FileName = "C:\\Users\\winsl\\AppData\\Local\\ESRI\\conda\\envs\\myenv-py3v2\\python.exe";
                                firstProcess.StartInfo.CreateNoWindow = true;
                                firstProcess.StartInfo.Arguments = "C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\MVC\\Python\\Scripts\\geocode_arcgis_online.py";
                                firstProcess.Start();
                                firstProcess.WaitForExit();
                            }

                            return RedirectToAction("Index");
                        case Models.NewLocation.ActionTypes.DeleteSuccessful:
                            loc.ActionType = Models.NewLocation.ActionTypes.DeleteSuccessful;
                            return RedirectToAction("Index");
                        default:
                            return View();

                    }


                    /*
                    using (secondProcess = new Process()) {
                        secondProcess.StartInfo.FileName = "C:\\Program Files\\ArcGIS\\Pro\\bin\\Python\\envs\\arcgispro-py3\\python.exe"; 
                        secondProcess.StartInfo.CreateNoWindow = true;
                        secondProcess.StartInfo.Arguments = "C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\MVC\\Python\\Scripts\\ZipFiles.py";
                        secondProcess.Start();
                        secondProcess.WaitForExit();
                    }
                    using (thirdProcess = new Process()) {
                        thirdProcess.StartInfo.FileName = "C:\\Program Files\\ArcGIS\\Pro\\bin\\Python\\envs\\arcgispro-py3\\python.exe";
                        thirdProcess.StartInfo.CreateNoWindow = true;
                        thirdProcess.StartInfo.Arguments = "C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\MVC\\Python\\Scripts\\connect2ArcgisOnline.py";
                        thirdProcess.Start();
                        thirdProcess.WaitForExit();
                    }
                    */
                }
                catch (Exception) {
                    Models.NewLocation loc = new Models.NewLocation();
                    return View(loc);
                }
            }
            else {
                return View();
            }
        }
    }
}