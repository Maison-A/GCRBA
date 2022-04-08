using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;


namespace GCRBA.Controllers
{
    public class SendMailerController : Controller
    {
        public ActionResult Index(Models.NewLocation passedLocation) {
            Models.NewLocation loc = (Models.NewLocation)TempData["location"];
            Models.MailModel mailModel = new Models.MailModel();
            mailModel.Content = loc;
            return View(mailModel);
        }

        [HttpPost]
        public ActionResult SendEmail(Models.MailModel email, FormCollection col) {
            email.Content.LocationName = col["Content.LocationName"];
            email.Content.LocationName = col["Content.LocationName"];
            email.Content.StreetAddress = col["Content.StreetAddress"];
            email.Content.City = col["Content.City"];
            email.Content.State = col["Content.State"];
            email.Content.Zip = col["Content.Zip"];

            //Extra Business Information
            email.Content.BizYear = col["Content.BizYear"];
            email.Content.Bio = col["Content.Bio"];
            email.Content.BusinessPhone = new Models.PhoneNumber();
            email.Content.BusinessPhone.AreaCode = col["Content.BusinessPhone.AreaCode"];
            email.Content.BusinessPhone.Prefix = col["Content.BusinessPhone.Prefix"];
            email.Content.BusinessPhone.Suffix = col["Content.BusinessPhone.Suffix"];
            email.Content.BusinessEmail = col["Content.BusinessEmail"];

            /*
            //Hours of Operation
            email.Content.Sunday = new Models.Days() { strDay = "Sunday", intDayID = 1, blnOperational = Convert.ToBoolean(col["Sunday.blnOperational"].Split(',')[0]), strOpenTime = col["Sunday.strOpenTime"], strClosedTime = col["Sunday.strClosedTime"] };
            email.Content.Monday = new Models.Days() { strDay = "Monday", intDayID = 2, blnOperational = Convert.ToBoolean(col["Monday.blnOperational"].Split(',')[0]), strOpenTime = col["Monday.strOpenTime"], strClosedTime = col["Monday.strClosedTime"] };
            email.Content.Tuesday = new Models.Days() { strDay = "Tuesday", intDayID = 3, blnOperational = Convert.ToBoolean(col["Tuesday.blnOperational"].Split(',')[0]), strOpenTime = col["Tuesday.strOpenTime"], strClosedTime = col["Tuesday.strClosedTime"] };
            email.Content.Wednesday = new Models.Days() { strDay = "Wednesday", intDayID = 4, blnOperational = Convert.ToBoolean(col["Wednesday.blnOperational"].Split(',')[0]), strOpenTime = col["Wednesday.strOpenTime"], strClosedTime = col["Wednesday.strClosedTime"] };
            email.Content.Thursday = new Models.Days() { strDay = "Thursday", intDayID = 5, blnOperational = Convert.ToBoolean(col["Thursday.blnOperational"].Split(',')[0]), strOpenTime = col["Thursday.strOpenTime"], strClosedTime = col["Thursday.strClosedTime"] };
            email.Content.Friday = new Models.Days() { strDay = "Friday", intDayID = 6, blnOperational = Convert.ToBoolean(col["Friday.blnOperational"].Split(',')[0]), strOpenTime = col["Friday.strOpenTime"], strClosedTime = col["Friday.strClosedTime"] };
            email.Content.Saturday = new Models.Days() { strDay = "Saturday", intDayID = 7, blnOperational = Convert.ToBoolean(col["Saturday.blnOperational"].Split(',')[0]), strOpenTime = col["Saturday.strOpenTime"], strClosedTime = col["Saturday.strClosedTime"] };

            
            //Product Categories
            email.Content.Donuts = new Models.CategoryItem() { ItemID = 1, ItemDesc = "Donuts", blnAvailable = Convert.ToBoolean(col["Donuts.blnAvailable"].Split(',')[0]) };
            email.Content.Bagels = new Models.CategoryItem() { ItemID = 2, ItemDesc = "Bagels", blnAvailable = Convert.ToBoolean(col["Bagels.blnAvailable"].Split(',')[0]) };
            email.Content.Muffins = new Models.CategoryItem() { ItemID = 3, ItemDesc = "Muffins", blnAvailable = Convert.ToBoolean(col["Muffins.blnAvailable"].Split(',')[0]) };
            email.Content.IceCream = new Models.CategoryItem() { ItemID = 4, ItemDesc = "Ice Cream", blnAvailable = Convert.ToBoolean(col["IceCream.blnAvailable"].Split(',')[0]) };
            email.Content.FineCandies = new Models.CategoryItem() { ItemID = 5, ItemDesc = "Fine Candies & Chocolates", blnAvailable = Convert.ToBoolean(col["FineCandies.blnAvailable"].Split(',')[0]) };
            email.Content.WeddingCakes = new Models.CategoryItem() { ItemID = 6, ItemDesc = "Wedding Cakes", blnAvailable = Convert.ToBoolean(col["WeddingCakes.blnAvailable"].Split(',')[0]) };
            email.Content.Breads = new Models.CategoryItem() { ItemID = 7, ItemDesc = "Breads", blnAvailable = Convert.ToBoolean(col["Breads.blnAvailable"].Split(',')[0]) };
            email.Content.DecoratedCakes = new Models.CategoryItem() { ItemID = 8, ItemDesc = "Decorated Cakes", blnAvailable = Convert.ToBoolean(col["DecoratedCakes.blnAvailable"].Split(',')[0]) };
            email.Content.Cupcakes = new Models.CategoryItem() { ItemID = 9, ItemDesc = "Cupcakes", blnAvailable = Convert.ToBoolean(col["Cupcakes.blnAvailable"].Split(',')[0]) };
            email.Content.Cookies = new Models.CategoryItem() { ItemID = 10, ItemDesc = "Cookies", blnAvailable = Convert.ToBoolean(col["Cookies.blnAvailable"].Split(',')[0]) };
            email.Content.Desserts = new Models.CategoryItem() { ItemID = 11, ItemDesc = "Desserts/Tortes", blnAvailable = Convert.ToBoolean(col["Desserts.blnAvailable"].Split(',')[0]) };
            email.Content.Full = new Models.CategoryItem() { ItemID = 12, ItemDesc = "Full-line Bakery", blnAvailable = Convert.ToBoolean(col["Full.blnAvailable"].Split(',')[0]) };
            email.Content.Deli = new Models.CategoryItem() { ItemID = 13, ItemDesc = "Deli/Catering", blnAvailable = Convert.ToBoolean(col["Deli.blnAvailable"].Split(',')[0]) };
            email.Content.Other = new Models.CategoryItem() { ItemID = 14, ItemDesc = "Other Carryout Deli", blnAvailable = Convert.ToBoolean(col["Other.blnAvailable"].Split(',')[0]) };
            email.Content.Wholesale = new Models.CategoryItem() { ItemID = 15, ItemDesc = "Wholesale", blnAvailable = Convert.ToBoolean(col["Wholesale.blnAvailable"].Split(',')[0]) };
            email.Content.Delivery = new Models.CategoryItem() { ItemID = 16, ItemDesc = "Delivery (3rd Party)", blnAvailable = Convert.ToBoolean(col["Delivery.blnAvailable"].Split(',')[0]) };
            email.Content.Shipping = new Models.CategoryItem() { ItemID = 17, ItemDesc = "Shipping", blnAvailable = Convert.ToBoolean(col["Shipping.blnAvailable"].Split(',')[0]) };
            email.Content.Online = new Models.CategoryItem() { ItemID = 18, ItemDesc = "Online Ordering", blnAvailable = Convert.ToBoolean(col["Online.blnAvailable"].Split(',')[0]) };

            

            //Member Only Variables
            //Contact Person Information
            email.Content.LocationContact = new Models.ContactPerson();
            email.Content.LocationContact.strContactFirstName = col["LocationContact.strContactFirstName"];
            email.Content.LocationContact.strContactLastName = col["LocationContact.strContactLastName"];
            email.Content.LocationContact.contactPhone = new Models.PhoneNumber();
            email.Content.LocationContact.contactPhone.AreaCode = col["LocationContact.contactPhone.AreaCode"];
            email.Content.LocationContact.contactPhone.Prefix = col["LocationContact.contactPhone.Prefix"];
            email.Content.LocationContact.contactPhone.Suffix = col["LocationContact.contactPhone.Suffix"];
            email.Content.LocationContact.strContactEmail = col["LocationContact.strContactEmail"];
            email.Content.LocationContact.intContactTypeID = 1;

            //Web Admin contact information
            email.Content.WebAdmin = new Models.ContactPerson();
            email.Content.WebAdmin.strContactFirstName = col["WebAdmin.strContactFirstName"];
            email.Content.WebAdmin.strContactLastName = col["WebAdmin.strContactLastName"];
            email.Content.WebAdmin.contactPhone = new Models.PhoneNumber();
            email.Content.WebAdmin.contactPhone.AreaCode = col["WebAdmin.contactPhone.AreaCode"];
            email.Content.WebAdmin.contactPhone.Prefix = col["WebAdmin.contactPhone.Prefix"];
            email.Content.WebAdmin.contactPhone.Suffix = col["WebAdmin.contactPhone.Suffix"];
            email.Content.WebAdmin.strContactEmail = col["WebAdmin.strContactEmail"];
            email.Content.WebAdmin.intContactTypeID = 2;

            //Customer Service Contact Information
            email.Content.CustService = new Models.ContactPerson();
            email.Content.CustService.strContactFirstName = col["CustService.strContactFirstName"];
            email.Content.CustService.strContactLastName = col["CustService.strContactLastName"];
            email.Content.CustService.contactPhone = new Models.PhoneNumber();
            email.Content.CustService.contactPhone.AreaCode = col["CustService.contactPhone.AreaCode"];
            email.Content.CustService.contactPhone.Prefix = col["CustService.contactPhone.Prefix"];
            email.Content.CustService.contactPhone.Suffix = col["CustService.contactPhone.Suffix"];
            email.Content.CustService.strContactEmail = col["CustService.strContactEmail"];
            email.Content.CustService.intContactTypeID = 3;

            //Web Portal Information
            email.Content.MainWeb = new Models.Website();
            email.Content.MainWeb.intWebsiteTypeID = 1;
            email.Content.MainWeb.strURL = col["MainWeb.strURL"];

            email.Content.OrderingWeb = new Models.Website();
            email.Content.OrderingWeb.intWebsiteTypeID = 2;
            email.Content.OrderingWeb.strURL = col["OrderingWeb.strURL"];

            email.Content.KettleWeb = new Models.Website();
            email.Content.KettleWeb.intWebsiteTypeID = 3;
            email.Content.KettleWeb.strURL = col["KettleWeb.strURL"];

            //Social Media Information
            email.Content.Facebook = new Models.SocialMedia() { strSocialMediaLink = col["Facebook.strSocialMediaLink"], intSocialMediaID = 1, blnAvailable = Convert.ToBoolean(col["Facebook.blnAvailable"].Split(',')[0]) };
            email.Content.Instagram = new Models.SocialMedia() { strSocialMediaLink = col["Instagram.strSocialMediaLink"], intSocialMediaID = 2, blnAvailable = Convert.ToBoolean(col["Instagram.blnAvailable"].Split(',')[0]) };
            email.Content.Snapchat = new Models.SocialMedia() { strSocialMediaLink = col["Snapchat.strSocialMediaLink"], intSocialMediaID = 3, blnAvailable = Convert.ToBoolean(col["Snapchat.blnAvailable"].Split(',')[0]) };
            email.Content.TikTok = new Models.SocialMedia() { strSocialMediaLink = col["TikTok.strSocialMediaLink"], intSocialMediaID = 4, blnAvailable = Convert.ToBoolean(col["TikTok.blnAvailable"].Split(',')[0]) };
            email.Content.Twitter = new Models.SocialMedia() { strSocialMediaLink = col["Twitter.strSocialMediaLink"], intSocialMediaID = 5, blnAvailable = Convert.ToBoolean(col["Twitter.blnAvailable"].Split(',')[0]) };
            email.Content.Yelp = new Models.SocialMedia() { strSocialMediaLink = col["Yelp.strSocialMediaLink"], intSocialMediaID = 6, blnAvailable = Convert.ToBoolean(col["Yelp.blnAvailable"].Split(',')[0]) };

            
            */

            string BizPhone = '(' + email.Content.BusinessPhone.AreaCode + ") " + email.Content.BusinessPhone.Prefix + '-' + email.Content.BusinessPhone.Suffix;

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Shared/LocEmailTemplate.html"))) {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{LocationName}", email.Content.LocationName);
            body = body.Replace("{Address}", email.Content.StreetAddress);
            body = body.Replace("{City}", email.Content.City);
            body = body.Replace("{State}", email.Content.State);
            body = body.Replace("{Zip}", email.Content.Zip);

            body = body.Replace("{BizPhone}", BizPhone);
            body = body.Replace("{BizEmail}", email.Content.BusinessEmail);
            body = body.Replace("{BizYear}", email.Content.BizYear);
            body = body.Replace("{BizBio}", email.Content.Bio);

            body = body.Replace("{Username}", email.UserName);
            body = body.Replace("{Title}", email.Title);
            body = body.Replace("{Url}", email.Url);
            body = body.Replace("{Description}", email.Description);

            using (MailMessage mailMessage = new MailMessage()) {
                mailMessage.From = new MailAddress(email.UserName);
                mailMessage.Subject = email.Subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                LocateFile file = new LocateFile();
                string file_path = file.GetFilePath();
                mailMessage.Attachments.Add(new Attachment(file_path));
                mailMessage.To.Add(new MailAddress(email.Recipient));
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("gcrbadata@gmail.com", "sazjptmmfdiyebom");
                smtp.EnableSsl = true;
                smtp.Send(mailMessage);
                return RedirectToAction("Index", "Bakery");
            }
            return View();
        }
    }

    public class LocateFile {
        public string GetFilePath() {
            return HttpContext.Current.Server.MapPath("/Views/SendMailer/Bakery.csv");
        }
    }
}