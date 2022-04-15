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
        public ActionResult Index(Models.LocationList passedLocationList) {
            Models.LocationList locList = (Models.LocationList)TempData["location"];
            Models.MailModel mailModel = new Models.MailModel();
            mailModel.Content = locList;
            return View(mailModel);
        }

        [HttpPost]
        public ActionResult SendEmail(Models.MailModel email, FormCollection col) {
            int i = 0;
            do {
                if(col["Content.lstLocations[" + i + "].LocationName"] != null) { 
                    email.Content.lstLocations[i] = new Models.NewLocation();
                    email.Content.lstLocations[i].LocationName = col["Content.lstLocations[" + i + "].LocationName"];
                    email.Content.lstLocations[i].LocationName = col["Content.lstLocations[" + i + "].LocationName"];
                    email.Content.lstLocations[i].StreetAddress = col["Content.lstLocations[" + i + "].StreetAddress"];
                    email.Content.lstLocations[i].City = col["Content.lstLocations[" + i + "].City"];
                    email.Content.lstLocations[i].State = col["Content.lstLocations[" + i + "].State"];
                    email.Content.lstLocations[i].Zip = col["Content.lstLocations[" + i + "].Zip"];

                    //Extra Business Information
                    email.Content.lstLocations[i].BizYear = col["Content.lstLocations[" + i + "].BizYear"];
                    email.Content.lstLocations[i].Bio = col["Content.lstLocations[" + i + "].Bio"];
                    email.Content.lstLocations[i].BusinessPhone = new Models.PhoneNumber();
                    email.Content.lstLocations[i].BusinessPhone.AreaCode = col["Content.lstLocations[" + i.ToString() + "].BusinessPhone.AreaCode"];
                    email.Content.lstLocations[i].BusinessPhone.Prefix = col["Content.lstLocations[" + i.ToString() + "].BusinessPhone.Prefix"];
                    email.Content.lstLocations[i].BusinessPhone.Suffix = col["Content.lstLocations[" + i.ToString() + "].BusinessPhone.Suffix"];
                    email.Content.lstLocations[i].BusinessEmail = col["Content.lstLocations[" + i.ToString() + "].BusinessEmail"];

                    //Hours of Operation
                    if (col["lstLocations[" + i + "].Sunday.blnOperational"] == null) email.Content.lstLocations[i].Sunday = new Models.Days() { strDay = "Sunday", intDayID = 1, blnOperational = false, strOpenTime = col["lstLocations[" + i + "].Sunday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Sunday.strClosedTime"] };
                    else email.Content.lstLocations[i].Sunday = new Models.Days() { strDay = "Sunday", intDayID = 1, blnOperational = Convert.ToBoolean(col["lstLocations[" + i + "].Sunday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + i + "].Sunday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Sunday.strClosedTime"] };

                    if (col["lstLocations[" + i + "].Monday.blnOperational"] == null) email.Content.lstLocations[i].Monday = new Models.Days() { strDay = "Monday", intDayID = 2, blnOperational = false, strOpenTime = col["lstLocations[" + i + "].Monday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Monday.strClosedTime"] };
                    else email.Content.lstLocations[i].Monday = new Models.Days() { strDay = "Monday", intDayID = 2, blnOperational = Convert.ToBoolean(col["lstLocations[" + i + "].Monday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + i + "].Monday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Monday.strClosedTime"] };

                    if (col["lstLocations[" + i + "].Tuesday.blnOperational"] == null) email.Content.lstLocations[i].Tuesday = new Models.Days() { strDay = "Tuesday", intDayID = 3, blnOperational = false, strOpenTime = col["lstLocations[" + i + "].Tuesday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Tuesday.strClosedTime"] };
                    else email.Content.lstLocations[i].Tuesday = new Models.Days() { strDay = "Tuesday", intDayID = 3, blnOperational = Convert.ToBoolean(col["lstLocations[" + i + "].Tuesday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + i + "].Tuesday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Tuesday.strClosedTime"] };

                    if (col["lstLocations[" + i + "].Wednesday.blnOperational"] == null) email.Content.lstLocations[i].Wednesday = new Models.Days() { strDay = "Wednesday", intDayID = 4, blnOperational = false, strOpenTime = col["lstLocations[" + i + "].Wednesday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Wednesday.strClosedTime"] };
                    else email.Content.lstLocations[i].Wednesday = new Models.Days() { strDay = "Wednesday", intDayID = 4, blnOperational = Convert.ToBoolean(col["lstLocations[" + i + "].Wednesday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + i + "].Wednesday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Wednesday.strClosedTime"] };

                    if (col["lstLocations[" + i + "].Thursday.blnOperational"] == null) email.Content.lstLocations[i].Thursday = new Models.Days() { strDay = "Thursday", intDayID = 5, blnOperational = false, strOpenTime = col["lstLocations[" + i + "].Thursday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Thursday.strClosedTime"] };
                    else email.Content.lstLocations[i].Thursday = new Models.Days() { strDay = "Thursday", intDayID = 5, blnOperational = Convert.ToBoolean(col["lstLocations[" + i + "].Thursday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + i + "].Thursday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Thursday.strClosedTime"] };

                    if (col["lstLocations[" + i + "].Friday.blnOperational"] == null) email.Content.lstLocations[i].Friday = new Models.Days() { strDay = "Friday", intDayID = 6, blnOperational = false, strOpenTime = col["lstLocations[" + i + "].Friday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Friday.strClosedTime"] };
                    else email.Content.lstLocations[i].Friday = new Models.Days() { strDay = "Friday", intDayID = 6, blnOperational = Convert.ToBoolean(col["lstLocations[" + i + "].Friday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + i + "].Friday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Friday.strClosedTime"] };

                    if (col["lstLocations[" + i + "].Saturday.blnOperational"] == null) email.Content.lstLocations[i].Saturday = new Models.Days() { strDay = "Saturday", intDayID = 7, blnOperational = false, strOpenTime = col["lstLocations[" + i + "].Saturday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Saturday.strClosedTime"] };
                    else email.Content.lstLocations[i].Saturday = new Models.Days() { strDay = "Saturday", intDayID = 7, blnOperational = Convert.ToBoolean(col["lstLocations[" + i + "].Saturday.blnOperational"].Split(',')[0]), strOpenTime = col["lstLocations[" + i + "].Saturday.strOpenTime"], strClosedTime = col["lstLocations[" + i + "].Saturday.strClosedTime"] };



                    //Product Categories
                    if (col["lstLocations[" + i + "].Donuts.blnAvailable"] == null) email.Content.lstLocations[i].Donuts = new Models.CategoryItem() { ItemID = 1, ItemDesc = "Donuts", blnAvailable = false };
                    else email.Content.lstLocations[i].Donuts = new Models.CategoryItem() { ItemID = 1, ItemDesc = "Donuts", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Donuts.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Bagels.blnAvailable"] == null) email.Content.lstLocations[i].Bagels = new Models.CategoryItem() { ItemID = 2, ItemDesc = "Bagels", blnAvailable = false };
                    else email.Content.lstLocations[i].Bagels = new Models.CategoryItem() { ItemID = 2, ItemDesc = "Bagels", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Bagels.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Muffins.blnAvailable"] == null) email.Content.lstLocations[i].Muffins = new Models.CategoryItem() { ItemID = 3, ItemDesc = "Muffins", blnAvailable = false };
                    else email.Content.lstLocations[i].Muffins = new Models.CategoryItem() { ItemID = 3, ItemDesc = "Muffins", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Muffins.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].IceCream.blnAvailable"] == null) email.Content.lstLocations[i].IceCream = new Models.CategoryItem() { ItemID = 4, ItemDesc = "IceCream", blnAvailable = false };
                    else email.Content.lstLocations[i].IceCream = new Models.CategoryItem() { ItemID = 4, ItemDesc = "Ice Cream", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].IceCream.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].FineCandies.blnAvailable"] == null) email.Content.lstLocations[i].FineCandies = new Models.CategoryItem() { ItemID = 5, ItemDesc = "FineCandies", blnAvailable = false };
                    else email.Content.lstLocations[i].FineCandies = new Models.CategoryItem() { ItemID = 5, ItemDesc = "Fine Candies & Chocolates", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].FineCandies.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].WeddingCakes.blnAvailable"] == null) email.Content.lstLocations[i].WeddingCakes = new Models.CategoryItem() { ItemID = 6, ItemDesc = "WeddingCakes", blnAvailable = false };
                    else email.Content.lstLocations[i].WeddingCakes = new Models.CategoryItem() { ItemID = 6, ItemDesc = "Wedding Cakes", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].WeddingCakes.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Breads.blnAvailable"] == null) email.Content.lstLocations[i].Breads = new Models.CategoryItem() { ItemID = 7, ItemDesc = "Breads", blnAvailable = false };
                    else email.Content.lstLocations[i].Breads = new Models.CategoryItem() { ItemID = 7, ItemDesc = "Breads", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Breads.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].DecoratedCakes.blnAvailable"] == null) email.Content.lstLocations[i].DecoratedCakes = new Models.CategoryItem() { ItemID = 8, ItemDesc = "DecoratedCakes", blnAvailable = false };
                    else email.Content.lstLocations[i].DecoratedCakes = new Models.CategoryItem() { ItemID = 8, ItemDesc = "Decorated Cakes", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].DecoratedCakes.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Cupcakes.blnAvailable"] == null) email.Content.lstLocations[i].Cupcakes = new Models.CategoryItem() { ItemID = 9, ItemDesc = "Cupcakes", blnAvailable = false };
                    else email.Content.lstLocations[i].Cupcakes = new Models.CategoryItem() { ItemID = 9, ItemDesc = "Cupcakes", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Cupcakes.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Cookies.blnAvailable"] == null) email.Content.lstLocations[i].Cookies = new Models.CategoryItem() { ItemID = 10, ItemDesc = "Cookies", blnAvailable = false };
                    else email.Content.lstLocations[i].Cookies = new Models.CategoryItem() { ItemID = 10, ItemDesc = "Cookies", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Cookies.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Desserts.blnAvailable"] == null) email.Content.lstLocations[i].Desserts = new Models.CategoryItem() { ItemID = 11, ItemDesc = "Desserts", blnAvailable = false };
                    else email.Content.lstLocations[i].Desserts = new Models.CategoryItem() { ItemID = 11, ItemDesc = "Desserts/Tortes", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Desserts.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Full.blnAvailable"] == null) email.Content.lstLocations[i].Full = new Models.CategoryItem() { ItemID = 12, ItemDesc = "Full", blnAvailable = false };
                    else email.Content.lstLocations[i].Full = new Models.CategoryItem() { ItemID = 12, ItemDesc = "Full-line Bakery", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Full.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Deli.blnAvailable"] == null) email.Content.lstLocations[i].Deli = new Models.CategoryItem() { ItemID = 13, ItemDesc = "Deli", blnAvailable = false };
                    else email.Content.lstLocations[i].Deli = new Models.CategoryItem() { ItemID = 13, ItemDesc = "Deli/Catering", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Deli.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Other.blnAvailable"] == null) email.Content.lstLocations[i].Other = new Models.CategoryItem() { ItemID = 14, ItemDesc = "Other", blnAvailable = false };
                    else email.Content.lstLocations[i].Other = new Models.CategoryItem() { ItemID = 14, ItemDesc = "Other Carryout Deli", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Other.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Wholesale.blnAvailable"] == null) email.Content.lstLocations[i].Wholesale = new Models.CategoryItem() { ItemID = 15, ItemDesc = "Wholesale", blnAvailable = false };
                    else email.Content.lstLocations[i].Wholesale = new Models.CategoryItem() { ItemID = 15, ItemDesc = "Wholesale", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Wholesale.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Delivery.blnAvailable"] == null) email.Content.lstLocations[i].Delivery = new Models.CategoryItem() { ItemID = 16, ItemDesc = "Delivery", blnAvailable = false };
                    else email.Content.lstLocations[i].Delivery = new Models.CategoryItem() { ItemID = 16, ItemDesc = "Delivery (3rd Party)", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Delivery.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Shipping.blnAvailable"] == null) email.Content.lstLocations[i].Shipping = new Models.CategoryItem() { ItemID = 17, ItemDesc = "Shipping", blnAvailable = false };
                    else email.Content.lstLocations[i].Shipping = new Models.CategoryItem() { ItemID = 17, ItemDesc = "Shipping", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Shipping.blnAvailable"].Split(',')[0]) };

                    if (col["lstLocations[" + i + "].Online.blnAvailable"] == null) email.Content.lstLocations[i].Online = new Models.CategoryItem() { ItemID = 18, ItemDesc = "Online", blnAvailable = false };
                    else email.Content.lstLocations[i].Online = new Models.CategoryItem() { ItemID = 18, ItemDesc = "Online Ordering", blnAvailable = Convert.ToBoolean(col["lstLocations[" + i + "].Online.blnAvailable"].Split(',')[0]) };


                    //Business Contact Information
                    email.Content.lstLocations[i].BusinessPhone = new Models.PhoneNumber();
                    email.Content.lstLocations[i].BusinessPhone.AreaCode = col["lstLocations[" + i + "].BusinessPhone.AreaCode"];
                    email.Content.lstLocations[i].BusinessPhone.Prefix = col["lstLocations[" + i + "].BusinessPhone.Prefix"];
                    email.Content.lstLocations[i].BusinessPhone.Suffix = col["lstLocations[" + i + "].BusinessPhone.Suffix"];
                    email.Content.lstLocations[i].BusinessEmail = col["lstLocations[" + i + "].BusinessEmail"];

                    //Member Only Variables
                    //Contact Person Information
                    email.Content.lstLocations[i].LocationContact = new Models.ContactPerson();
                    email.Content.lstLocations[i].LocationContact.strContactFirstName = col["lstLocations[" + i + "].LocationContact.strContactFirstName"];
                    email.Content.lstLocations[i].LocationContact.strContactLastName = col["lstLocations[" + i + "].LocationContact.strContactLastName"];
                    email.Content.lstLocations[i].LocationContact.contactPhone = new Models.PhoneNumber();
                    email.Content.lstLocations[i].LocationContact.contactPhone.AreaCode = col["lstLocations[" + i + "].LocationContact.contactPhone.AreaCode"];
                    email.Content.lstLocations[i].LocationContact.contactPhone.Prefix = col["lstLocations[" + i + "].LocationContact.contactPhone.Prefix"];
                    email.Content.lstLocations[i].LocationContact.contactPhone.Suffix = col["lstLocations[" + i + "].LocationContact.contactPhone.Suffix"];
                    email.Content.lstLocations[i].LocationContact.strContactEmail = col["lstLocations[" + i + "].LocationContact.strContactEmail"];
                    email.Content.lstLocations[i].LocationContact.intContactTypeID = 1;

                    //Web Admin contact information
                    email.Content.lstLocations[i].WebAdmin = new Models.ContactPerson();
                    email.Content.lstLocations[i].WebAdmin.strContactFirstName = col["lstLocations[" + i + "].WebAdmin.strContactFirstName"];
                    email.Content.lstLocations[i].WebAdmin.strContactLastName = col["lstLocations[" + i + "].WebAdmin.strContactLastName"];
                    email.Content.lstLocations[i].WebAdmin.contactPhone = new Models.PhoneNumber();
                    email.Content.lstLocations[i].WebAdmin.contactPhone.AreaCode = col["lstLocations[" + i + "].WebAdmin.contactPhone.AreaCode"];
                    email.Content.lstLocations[i].WebAdmin.contactPhone.Prefix = col["lstLocations[" + i + "].WebAdmin.contactPhone.Prefix"];
                    email.Content.lstLocations[i].WebAdmin.contactPhone.Suffix = col["lstLocations[" + i + "].WebAdmin.contactPhone.Suffix"];
                    email.Content.lstLocations[i].WebAdmin.strContactEmail = col["lstLocations[" + i + "].WebAdmin.strContactEmail"];
                    email.Content.lstLocations[i].WebAdmin.intContactTypeID = 2;

                    //Customer Service Contact Information
                    email.Content.lstLocations[i].CustService = new Models.ContactPerson();
                    email.Content.lstLocations[i].CustService.strContactFirstName = col["lstLocations[" + i + "].CustService.strContactFirstName"];
                    email.Content.lstLocations[i].CustService.strContactLastName = col["lstLocations[" + i + "].CustService.strContactLastName"];
                    email.Content.lstLocations[i].CustService.contactPhone = new Models.PhoneNumber();
                    email.Content.lstLocations[i].CustService.contactPhone.AreaCode = col["lstLocations[" + i + "].CustService.contactPhone.AreaCode"];
                    email.Content.lstLocations[i].CustService.contactPhone.Prefix = col["lstLocations[" + i + "].CustService.contactPhone.Prefix"];
                    email.Content.lstLocations[i].CustService.contactPhone.Suffix = col["lstLocations[" + i + "].CustService.contactPhone.Suffix"];
                    email.Content.lstLocations[i].CustService.strContactEmail = col["lstLocations[" + i + "].CustService.strContactEmail"];
                    email.Content.lstLocations[i].CustService.intContactTypeID = 3;

                    //Web Portal Information
                    email.Content.lstLocations[i].MainWeb = new Models.Website();
                    email.Content.lstLocations[i].MainWeb.intWebsiteTypeID = 1;
                    email.Content.lstLocations[i].MainWeb.strURL = col["lstLocations[" + i + "].MainWeb.strURL"];

                    email.Content.lstLocations[i].OrderingWeb = new Models.Website();
                    email.Content.lstLocations[i].OrderingWeb.intWebsiteTypeID = 2;
                    email.Content.lstLocations[i].OrderingWeb.strURL = col["lstLocations[" + i + "].OrderingWeb.strURL"];

                    email.Content.lstLocations[i].KettleWeb = new Models.Website();
                    email.Content.lstLocations[i].KettleWeb.intWebsiteTypeID = 3;
                    email.Content.lstLocations[i].KettleWeb.strURL = col["lstLocations[" + i + "].KettleWeb.strURL"];

                    string BizPhone = '(' + email.Content.lstLocations[i].BusinessPhone.AreaCode + ") " + email.Content.lstLocations[i].BusinessPhone.Prefix + '-' + email.Content.lstLocations[i].BusinessPhone.Suffix;

                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(Server.MapPath("~\\Views\\Shared\\LocEmailTemplate.html"))) {
                        //"C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\MVC\\Views\\Shared\\LocEmailTemplate.html"
                        //"E:\\Web-Folders\\Students\\Spring\\CPDM-290-200\\CPDM-WinslowS\\Views\\Shared\\LocEmailTemplate.html"
                        //Server.MapPath("~\\Views\\Shared\\LocEmailTemplate.html")
                        body = reader.ReadToEnd();
                    }

                    body = body.Replace("{LocationName}", email.Content.lstLocations[i].LocationName);
                    body = body.Replace("{Address}", email.Content.lstLocations[i].StreetAddress);
                    body = body.Replace("{City}", email.Content.lstLocations[i].City);
                    body = body.Replace("{State}", email.Content.lstLocations[i].State);
                    body = body.Replace("{Zip}", email.Content.lstLocations[i].Zip);

                    body = body.Replace("{BizPhone}", BizPhone);
                    body = body.Replace("{BizEmail}", email.Content.lstLocations[i].BusinessEmail);
                    body = body.Replace("{BizYear}", email.Content.lstLocations[i].BizYear);
                    body = body.Replace("{BizBio}", email.Content.lstLocations[i].Bio);

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
                        string file_path = file.GetFilePath(i);
                        //string file_path = "E:\\Web-Folders\\Students\\Spring\\CPDM-290-200\\CPDM-WinslowS\\CSV_Folder\\Bakery.csv"; //"C:/Users/winsl/OneDrive/Desktop/Capstone/MVC/Views/SendMailer/Bakery.csv"; //"E:/Web-Folders/Students/Spring/CPDM-290-200/CPDM-WinslowS/Views/SendMailer/Bakery.csv";
                        mailMessage.Attachments.Add(new Attachment(file_path));
                        mailMessage.To.Add(new MailAddress(email.Recipient));
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("gcrbadata@gmail.com", "sazjptmmfdiyebom");
                        smtp.EnableSsl = true;
                        smtp.Send(mailMessage);
                    }
                    i += 1;
                }
            } while (col["Content.lstLocations[" + i + "]"] != null);
            delete deleteFiles = new delete();
            string folder_path = Server.MapPath("\\CSV_Folder");
            deleteFiles.delete_files(folder_path);
            return RedirectToAction("Index", "Bakery");
        }
    }

    public class LocateFile {
        public string GetFilePath(int i) {
            return HttpContext.Current.Server.MapPath("\\Views\\SendMailer\\CSV_Folder\\Bakery" + i.ToString() + ".csv");
        }
    }

    public class delete {
        public void delete_files(string path) {
            System.IO.DirectoryInfo dinfo = null;
			try {
                dinfo = new System.IO.DirectoryInfo(path);
			}
            catch { return;}
            System.IO.FileInfo[] dinfo_files = dinfo.GetFiles();
            for (int i = 0; i < dinfo_files.Length; i++) {
                if (dinfo_files[i].Name.EndsWith(".csv")) {
                    try {
                        System.IO.File.Delete(dinfo_files[i].FullName);
					}
                    catch { return; }
				}
			}
        }
    }
}