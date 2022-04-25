using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

namespace GCRBA {
    public class SendLocationEmail {
        static void Main(Models.LocationList locationList) {
            SendEmail(locationList);
        }
        
        public static void SendEmail(Models.LocationList locationList) { 
            int i = 0;
            do {
                GCRBA.Models.User user = new Models.User();
                user = user.GetUserSession();
                GCRBA.Models.LocationMailModel locationMailModel = new Models.LocationMailModel();

                locationMailModel.UserEmail = user.Email;
                locationMailModel.UserFullName = user.LastName + ", " + user.FirstName;
                locationMailModel.UserTelephone = user.Phone;

                locationMailModel.Content = new Models.LocationList();
                locationMailModel.Content.lstLocations[i] = new Models.NewLocation();
                locationMailModel.Content.lstLocations[i].CompanyName =     locationList.lstLocations[i].CompanyName;
                locationMailModel.Content.lstLocations[i].LocationName =    locationList.lstLocations[i].LocationName;
                locationMailModel.Content.lstLocations[i].StreetAddress =   locationList.lstLocations[i].StreetAddress;
                locationMailModel.Content.lstLocations[i].City =            locationList.lstLocations[i].City;
                locationMailModel.Content.lstLocations[i].State =           locationList.lstLocations[i].State;
                locationMailModel.Content.lstLocations[i].Zip =             locationList.lstLocations[i].Zip;

                //Extra Business Information
                locationMailModel.Content.lstLocations[i].BizYear =                 locationList.lstLocations[i].BizYear;
                locationMailModel.Content.lstLocations[i].Bio =                     locationList.lstLocations[i].Bio;
                locationMailModel.Content.lstLocations[i].BusinessPhone =           locationList.lstLocations[i].BusinessPhone;
                locationMailModel.Content.lstLocations[i].BusinessPhone.AreaCode =  locationList.lstLocations[i].BusinessPhone.AreaCode;
                locationMailModel.Content.lstLocations[i].BusinessPhone.Prefix =    locationList.lstLocations[i].BusinessPhone.Prefix;
                locationMailModel.Content.lstLocations[i].BusinessPhone.Suffix =    locationList.lstLocations[i].BusinessPhone.Suffix;
                locationMailModel.Content.lstLocations[i].BusinessEmail =           locationList.lstLocations[i].BusinessEmail;

                locationMailModel.Content.lstLocations[i].Sunday =          locationList.lstLocations[i].Sunday;
                locationMailModel.Content.lstLocations[i].Monday =          locationList.lstLocations[i].Monday;
                locationMailModel.Content.lstLocations[i].Tuesday =         locationList.lstLocations[i].Tuesday;
                locationMailModel.Content.lstLocations[i].Wednesday =       locationList.lstLocations[i].Wednesday;
                locationMailModel.Content.lstLocations[i].Thursday =        locationList.lstLocations[i].Thursday;
                locationMailModel.Content.lstLocations[i].Friday =          locationList.lstLocations[i].Friday;
                locationMailModel.Content.lstLocations[i].Saturday =        locationList.lstLocations[i].Saturday;

                locationMailModel.Content.lstLocations[i].Donuts =          locationList.lstLocations[i].Donuts;
                locationMailModel.Content.lstLocations[i].Bagels =          locationList.lstLocations[i].Breads;
                locationMailModel.Content.lstLocations[i].Muffins =         locationList.lstLocations[i].Muffins;
                locationMailModel.Content.lstLocations[i].IceCream =        locationList.lstLocations[i].IceCream;
                locationMailModel.Content.lstLocations[i].FineCandies =     locationList.lstLocations[i].FineCandies;
                locationMailModel.Content.lstLocations[i].WeddingCakes =    locationList.lstLocations[i].WeddingCakes;
                locationMailModel.Content.lstLocations[i].Breads =          locationList.lstLocations[i].Breads;
                locationMailModel.Content.lstLocations[i].DecoratedCakes =  locationList.lstLocations[i].DecoratedCakes;
                locationMailModel.Content.lstLocations[i].Cupcakes =        locationList.lstLocations[i].Cupcakes;
                locationMailModel.Content.lstLocations[i].Cookies =         locationList.lstLocations[i].Cookies;
                locationMailModel.Content.lstLocations[i].Desserts =        locationList.lstLocations[i].Desserts;
                locationMailModel.Content.lstLocations[i].Full =            locationList.lstLocations[i].Full;
                locationMailModel.Content.lstLocations[i].Deli =            locationList.lstLocations[i].Deli;
                locationMailModel.Content.lstLocations[i].Other =           locationList.lstLocations[i].Other;
                locationMailModel.Content.lstLocations[i].Wholesale =       locationList.lstLocations[i].Wholesale;
                locationMailModel.Content.lstLocations[i].Delivery =        locationList.lstLocations[i].Delivery;
                locationMailModel.Content.lstLocations[i].Shipping =        locationList.lstLocations[i].Shipping;
                locationMailModel.Content.lstLocations[i].Online =          locationList.lstLocations[i].Online;

                //Member Only Variables
                //Contact Person Information
                locationMailModel.Content.lstLocations[i].LocationContact                       = new Models.ContactPerson();
                locationMailModel.Content.lstLocations[i].LocationContact.strContactFirstName   = locationList.lstLocations[i].LocationContact.strContactFirstName;
                locationMailModel.Content.lstLocations[i].LocationContact.strContactLastName    = locationList.lstLocations[i].LocationContact.strContactLastName;
                locationMailModel.Content.lstLocations[i].LocationContact.contactPhone          = new Models.PhoneNumber();
                locationMailModel.Content.lstLocations[i].LocationContact.contactPhone.AreaCode = locationList.lstLocations[i].LocationContact.contactPhone.AreaCode;
                locationMailModel.Content.lstLocations[i].LocationContact.contactPhone.Prefix   = locationList.lstLocations[i].LocationContact.contactPhone.Prefix;
                locationMailModel.Content.lstLocations[i].LocationContact.contactPhone.Suffix   = locationList.lstLocations[i].LocationContact.contactPhone.Suffix;
                locationMailModel.Content.lstLocations[i].LocationContact.strContactEmail       = locationList.lstLocations[i].LocationContact.strContactEmail;
                locationMailModel.Content.lstLocations[i].LocationContact.intContactTypeID      = (short)Models.ContactPerson.ContactTypes.LocationContact;
                locationMailModel.Content.lstLocations[i].LocationContact.ContactType           = Models.ContactPerson.ContactTypes.LocationContact;

                if(!String.IsNullOrEmpty(locationList.lstLocations[i].LocationContact.strContactFirstName) && !String.IsNullOrEmpty(locationList.lstLocations[i].LocationContact.strContactLastName)) {
                    locationMailModel.Content.lstLocations[i].LocationContact.strFullName = locationList.lstLocations[i].LocationContact.strContactLastName + ", " + locationList.lstLocations[i].LocationContact.strContactFirstName;
                }

                if (!String.IsNullOrEmpty(locationList.lstLocations[i].LocationContact.contactPhone.AreaCode) && !String.IsNullOrEmpty(locationList.lstLocations[i].LocationContact.contactPhone.Prefix) && !String.IsNullOrEmpty(locationList.lstLocations[i].LocationContact.contactPhone.Suffix)) {
                    locationMailModel.Content.lstLocations[i].LocationContact.strFullPhone = '(' + locationList.lstLocations[i].LocationContact.contactPhone.AreaCode + ") " + locationList.lstLocations[i].LocationContact.contactPhone.Prefix + '-' + locationList.lstLocations[i].LocationContact.contactPhone.Suffix;    
                }

                //Web Admin contact information
                locationMailModel.Content.lstLocations[i].WebAdmin                          = new Models.ContactPerson();
                locationMailModel.Content.lstLocations[i].WebAdmin.strContactFirstName      = locationList.lstLocations[i].WebAdmin.strContactFirstName;
                locationMailModel.Content.lstLocations[i].WebAdmin.strContactLastName       = locationList.lstLocations[i].WebAdmin.strContactLastName;
                locationMailModel.Content.lstLocations[i].WebAdmin.contactPhone             = new Models.PhoneNumber();
                locationMailModel.Content.lstLocations[i].WebAdmin.contactPhone.AreaCode    = locationList.lstLocations[i].WebAdmin.contactPhone.AreaCode;
                locationMailModel.Content.lstLocations[i].WebAdmin.contactPhone.Prefix      = locationList.lstLocations[i].WebAdmin.contactPhone.Prefix;
                locationMailModel.Content.lstLocations[i].WebAdmin.contactPhone.Suffix      = locationList.lstLocations[i].WebAdmin.contactPhone.Suffix;
                locationMailModel.Content.lstLocations[i].WebAdmin.strContactEmail          = locationList.lstLocations[i].WebAdmin.strContactEmail;
                locationMailModel.Content.lstLocations[i].WebAdmin.intContactTypeID         = (short)Models.ContactPerson.ContactTypes.WebAdmin;
                locationMailModel.Content.lstLocations[i].WebAdmin.ContactType              = Models.ContactPerson.ContactTypes.WebAdmin;

                if (!String.IsNullOrEmpty(locationList.lstLocations[i].WebAdmin.strContactFirstName) && !String.IsNullOrEmpty(locationList.lstLocations[i].WebAdmin.strContactLastName)) {
                    locationMailModel.Content.lstLocations[i].WebAdmin.strFullName = locationList.lstLocations[i].WebAdmin.strContactLastName + ", " + locationList.lstLocations[i].WebAdmin.strContactFirstName;
                }

                if (!String.IsNullOrEmpty(locationList.lstLocations[i].WebAdmin.contactPhone.AreaCode) && !String.IsNullOrEmpty(locationList.lstLocations[i].WebAdmin.contactPhone.Prefix) && !String.IsNullOrEmpty(locationList.lstLocations[i].WebAdmin.contactPhone.Suffix)) {
                    locationMailModel.Content.lstLocations[i].WebAdmin.strFullPhone = '(' + locationList.lstLocations[i].WebAdmin.contactPhone.AreaCode + ") " + locationList.lstLocations[i].WebAdmin.contactPhone.Prefix + '-' + locationList.lstLocations[i].WebAdmin.contactPhone.Suffix;
                }

                //Customer Service Contact Information
                locationMailModel.Content.lstLocations[i].CustService                       = new Models.ContactPerson();
                locationMailModel.Content.lstLocations[i].CustService.strContactFirstName   = locationList.lstLocations[i].CustService.strContactFirstName;
                locationMailModel.Content.lstLocations[i].CustService.strContactLastName    = locationList.lstLocations[i].CustService.strContactLastName;
                locationMailModel.Content.lstLocations[i].CustService.contactPhone          = new Models.PhoneNumber();
                locationMailModel.Content.lstLocations[i].CustService.contactPhone.AreaCode = locationList.lstLocations[i].CustService.contactPhone.AreaCode;
                locationMailModel.Content.lstLocations[i].CustService.contactPhone.Prefix   = locationList.lstLocations[i].CustService.contactPhone.Prefix;
                locationMailModel.Content.lstLocations[i].CustService.contactPhone.Suffix   = locationList.lstLocations[i].CustService.contactPhone.Suffix;
                locationMailModel.Content.lstLocations[i].CustService.strContactEmail       = locationList.lstLocations[i].CustService.strContactEmail;
                locationMailModel.Content.lstLocations[i].CustService.intContactTypeID      = (short)Models.ContactPerson.ContactTypes.CustomerService;
                locationMailModel.Content.lstLocations[i].CustService.ContactType           = Models.ContactPerson.ContactTypes.CustomerService;

                if (!String.IsNullOrEmpty(locationList.lstLocations[i].CustService.strContactFirstName) && !String.IsNullOrEmpty(locationList.lstLocations[i].CustService.strContactLastName)) {
                    locationMailModel.Content.lstLocations[i].CustService.strFullName = locationList.lstLocations[i].CustService.strContactLastName + ", " + locationList.lstLocations[i].CustService.strContactFirstName;
                }

                if (!String.IsNullOrEmpty(locationList.lstLocations[i].CustService.contactPhone.AreaCode) && !String.IsNullOrEmpty(locationList.lstLocations[i].CustService.contactPhone.Prefix) && !String.IsNullOrEmpty(locationList.lstLocations[i].CustService.contactPhone.Suffix)) {
                    locationMailModel.Content.lstLocations[i].CustService.strFullPhone = '(' + locationList.lstLocations[i].CustService.contactPhone.AreaCode + ") " + locationList.lstLocations[i].CustService.contactPhone.Prefix + '-' + locationList.lstLocations[i].CustService.contactPhone.Suffix;
                }

                //Web Portal Information
                locationMailModel.Content.lstLocations[i].MainWeb                              = new Models.Website();
                locationMailModel.Content.lstLocations[i].MainWeb.intWebsiteTypeID             = (short)Models.Website.WebsiteTypes.MainPage;
                locationMailModel.Content.lstLocations[i].MainWeb.WebsiteType                  = Models.Website.WebsiteTypes.MainPage;
                locationMailModel.Content.lstLocations[i].MainWeb.strURL                       = locationList.lstLocations[i].MainWeb.strURL;
                
                locationMailModel.Content.lstLocations[i].OrderingWeb                          = new Models.Website();
                locationMailModel.Content.lstLocations[i].OrderingWeb.intWebsiteTypeID         = (short)Models.Website.WebsiteTypes.OrderingPage;
                locationMailModel.Content.lstLocations[i].OrderingWeb.WebsiteType              = Models.Website.WebsiteTypes.OrderingPage;
                locationMailModel.Content.lstLocations[i].OrderingWeb.strURL                   = locationList.lstLocations[i].OrderingWeb.strURL;

                locationMailModel.Content.lstLocations[i].KettleWeb                            = new Models.Website();
                locationMailModel.Content.lstLocations[i].KettleWeb.intWebsiteTypeID           = (short)Models.Website.WebsiteTypes.DonationPage;
                locationMailModel.Content.lstLocations[i].KettleWeb.WebsiteType                = Models.Website.WebsiteTypes.DonationPage;
                locationMailModel.Content.lstLocations[i].KettleWeb.strURL                     = locationList.lstLocations[i].KettleWeb.strURL;

                locationMailModel.Content.lstLocations[i].Facebook      = locationList.lstLocations[i].Facebook;
                locationMailModel.Content.lstLocations[i].Twitter       = locationList.lstLocations[i].Twitter;
                locationMailModel.Content.lstLocations[i].Snapchat      = locationList.lstLocations[i].Snapchat;
                locationMailModel.Content.lstLocations[i].TikTok        = locationList.lstLocations[i].TikTok;
                locationMailModel.Content.lstLocations[i].Instagram     = locationList.lstLocations[i].Instagram;
                locationMailModel.Content.lstLocations[i].Yelp          = locationList.lstLocations[i].Yelp;
               

                string BizPhone = '(' + locationMailModel.Content.lstLocations[i].BusinessPhone.AreaCode + ") " + locationMailModel.Content.lstLocations[i].BusinessPhone.Prefix + '-' + locationMailModel.Content.lstLocations[i].BusinessPhone.Suffix;

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~\\Views\\Shared\\LocEmailTemplate.html"))) {
                    //"C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\MVC\\Views\\Shared\\LocEmailTemplate.html"
                    //"E:\\Web-Folders\\Students\\Spring\\CPDM-290-200\\CPDM-WinslowS\\Views\\Shared\\LocEmailTemplate.html"
                    //Server.MapPath("~\\Views\\Shared\\LocEmailTemplate.html")
                    body = reader.ReadToEnd();
                }
                
                body = body.Replace("{CompanyName}", locationMailModel.Content.lstLocations[i].CompanyName);
                body = body.Replace("{LocationName}", locationMailModel.Content.lstLocations[i].LocationName);
                body = body.Replace("{Address}", locationMailModel.Content.lstLocations[i].StreetAddress);
                body = body.Replace("{City}", locationMailModel.Content.lstLocations[i].City);
                body = body.Replace("{State}", locationMailModel.Content.lstLocations[i].State);
                body = body.Replace("{Zip}", locationMailModel.Content.lstLocations[i].Zip);

                body = body.Replace("{BizPhone}", BizPhone);
                body = body.Replace("{BizEmail}", locationMailModel.Content.lstLocations[i].BusinessEmail);
                body = body.Replace("{BizYear}", locationMailModel.Content.lstLocations[i].BizYear);
                body = body.Replace("{BizBio}", locationMailModel.Content.lstLocations[i].Bio);

                body = body.Replace("{SundayStart}", locationMailModel.Content.lstLocations[i].Sunday.strOpenTime);
                body = body.Replace("{SundayEnd}", locationMailModel.Content.lstLocations[i].Sunday.strClosedTime);
                body = body.Replace("{MondayStart}", locationMailModel.Content.lstLocations[i].Monday.strOpenTime);
                body = body.Replace("{MondayEnd}", locationMailModel.Content.lstLocations[i].Monday.strClosedTime);
                body = body.Replace("{TuesdayStart}", locationMailModel.Content.lstLocations[i].Tuesday.strOpenTime);
                body = body.Replace("{TuesdayEnd}", locationMailModel.Content.lstLocations[i].Tuesday.strClosedTime);
                body = body.Replace("{WednesdayStart}", locationMailModel.Content.lstLocations[i].Wednesday.strOpenTime);
                body = body.Replace("{WednesdayEnd}", locationMailModel.Content.lstLocations[i].Wednesday.strClosedTime);
                body = body.Replace("{ThursdayStart}", locationMailModel.Content.lstLocations[i].Thursday.strOpenTime);
                body = body.Replace("{ThursdayEnd}", locationMailModel.Content.lstLocations[i].Thursday.strClosedTime);
                body = body.Replace("{FridayStart}", locationMailModel.Content.lstLocations[i].Friday.strOpenTime);
                body = body.Replace("{FridayEnd}", locationMailModel.Content.lstLocations[i].Friday.strClosedTime);
                body = body.Replace("{SaturdayStart}", locationMailModel.Content.lstLocations[i].Saturday.strOpenTime);
                body = body.Replace("{SaturdayEnd}", locationMailModel.Content.lstLocations[i].Saturday.strClosedTime);

                body = body.Replace("{DonutStatus}", locationMailModel.Content.lstLocations[i].Donuts.blnAvailable.ToString());
                body = body.Replace("{BagelStatus}", locationMailModel.Content.lstLocations[i].Bagels.blnAvailable.ToString());
                body = body.Replace("{MuffinStatus}", locationMailModel.Content.lstLocations[i].Muffins.blnAvailable.ToString());
                body = body.Replace("{IceCreamStatus}", locationMailModel.Content.lstLocations[i].IceCream.blnAvailable.ToString());
                body = body.Replace("{FineCandiesStatus}", locationMailModel.Content.lstLocations[i].FineCandies.blnAvailable.ToString());
                body = body.Replace("{WeddingCakeStatus}", locationMailModel.Content.lstLocations[i].WeddingCakes.blnAvailable.ToString());
                body = body.Replace("{BreadStatus}", locationMailModel.Content.lstLocations[i].Breads.blnAvailable.ToString());
                body = body.Replace("{DecoratedCakeStatus}", locationMailModel.Content.lstLocations[i].DecoratedCakes.blnAvailable.ToString());
                body = body.Replace("{CupcakeStatus}", locationMailModel.Content.lstLocations[i].Cupcakes.blnAvailable.ToString());
                body = body.Replace("{CookieStatus}", locationMailModel.Content.lstLocations[i].Cookies.blnAvailable.ToString());
                body = body.Replace("{DessertStatus}", locationMailModel.Content.lstLocations[i].Desserts.blnAvailable.ToString());
                body = body.Replace("{FullStatus}", locationMailModel.Content.lstLocations[i].Full.blnAvailable.ToString());
                body = body.Replace("{DeliStatus}", locationMailModel.Content.lstLocations[i].Deli.blnAvailable.ToString());
                body = body.Replace("{OtherStatus}", locationMailModel.Content.lstLocations[i].Other.blnAvailable.ToString());
                body = body.Replace("{WholesaleStatus}", locationMailModel.Content.lstLocations[i].Wholesale.blnAvailable.ToString());
                body = body.Replace("{DeliveryStatus}", locationMailModel.Content.lstLocations[i].Delivery.blnAvailable.ToString());
                body = body.Replace("{ShippingStatus}", locationMailModel.Content.lstLocations[i].Shipping.blnAvailable.ToString());
                body = body.Replace("{OnlineStatus}", locationMailModel.Content.lstLocations[i].Online.blnAvailable.ToString());

                body = body.Replace("{Username}", locationMailModel.UserName);
                body = body.Replace("{Title}", locationMailModel.Title);
                body = body.Replace("{Url}", locationMailModel.Url);
                body = body.Replace("{Description}", locationMailModel.Description);
                body = body.Replace("{UserFullName}", locationMailModel.UserFullName);
                body = body.Replace("{UserPhone}", locationMailModel.UserTelephone);
                body = body.Replace("{UserEmail}", locationMailModel.UserEmail);

                body = body.Replace("{LocationContactType}", locationMailModel.Content.lstLocations[i].LocationContact.ContactType.ToString());
                body = body.Replace("{LocationContactName}", locationMailModel.Content.lstLocations[i].LocationContact.strFullName);
                body = body.Replace("{LocationContactEmail}", locationMailModel.Content.lstLocations[i].LocationContact.strContactEmail);
                body = body.Replace("{LocationContactPhone}", locationMailModel.Content.lstLocations[i].LocationContact.strFullPhone);

                body = body.Replace("{CustServiceContactType}", locationMailModel.Content.lstLocations[i].CustService.ContactType.ToString());
                body = body.Replace("{CustServiceContactName}", locationMailModel.Content.lstLocations[i].CustService.strFullName);
                body = body.Replace("{CustServiceContactEmail}", locationMailModel.Content.lstLocations[i].CustService.strContactEmail);
                body = body.Replace("{CustServiceContactPhone}", locationMailModel.Content.lstLocations[i].CustService.strFullPhone);

                body = body.Replace("{WebAdminContactType}", locationMailModel.Content.lstLocations[i].WebAdmin.ContactType.ToString());
                body = body.Replace("{WebAdminContactName}", locationMailModel.Content.lstLocations[i].WebAdmin.strFullName);
                body = body.Replace("{WebAdminContactEmail}", locationMailModel.Content.lstLocations[i].WebAdmin.strContactEmail);
                body = body.Replace("{WebAdminContactPhone}", locationMailModel.Content.lstLocations[i].WebAdmin.strFullPhone);

                body = body.Replace("{FacebookURL}", locationMailModel.Content.lstLocations[i].Facebook.strSocialMediaLink);
                body = body.Replace("{FacebookAvailable}", locationMailModel.Content.lstLocations[i].Facebook.blnAvailable.ToString());
                body = body.Replace("{InstagramURL}", locationMailModel.Content.lstLocations[i].Instagram.strSocialMediaLink);
                body = body.Replace("{InstagramAvailable}", locationMailModel.Content.lstLocations[i].Instagram.blnAvailable.ToString());
                body = body.Replace("{TwitterURL}", locationMailModel.Content.lstLocations[i].Twitter.strSocialMediaLink);
                body = body.Replace("{TwitterAvailable}", locationMailModel.Content.lstLocations[i].Twitter.blnAvailable.ToString());
                body = body.Replace("{TikTokURL}", locationMailModel.Content.lstLocations[i].TikTok.strSocialMediaLink);
                body = body.Replace("{TikTokAvailable}", locationMailModel.Content.lstLocations[i].TikTok.blnAvailable.ToString());
                body = body.Replace("{YelpURL}", locationMailModel.Content.lstLocations[i].Yelp.strSocialMediaLink);
                body = body.Replace("{YelpAvailable}", locationMailModel.Content.lstLocations[i].Yelp.blnAvailable.ToString());
                body = body.Replace("{SnapchatURL}", locationMailModel.Content.lstLocations[i].Snapchat.strSocialMediaLink);
                body = body.Replace("{SnapchatAvailable}", locationMailModel.Content.lstLocations[i].Snapchat.blnAvailable.ToString());

                body = body.Replace("{MainWebType}", locationMailModel.Content.lstLocations[i].MainWeb.WebsiteType.ToString());
                body = body.Replace("{MainWebURL}", locationMailModel.Content.lstLocations[i].MainWeb.strURL);
                body = body.Replace("{OrderingWebType}", locationMailModel.Content.lstLocations[i].OrderingWeb.WebsiteType.ToString());
                body = body.Replace("{OrderingWebURL}", locationMailModel.Content.lstLocations[i].OrderingWeb.strURL);
                body = body.Replace("{KettleWebType}", locationMailModel.Content.lstLocations[i].KettleWeb.WebsiteType.ToString());
                body = body.Replace("{KettleWebURL}", locationMailModel.Content.lstLocations[i].KettleWeb.strURL);

                using (MailMessage mailMessage = new MailMessage()) {
                    mailMessage.From = new MailAddress(locationMailModel.UserName);
                    mailMessage.Subject = locationMailModel.Subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    LocateFile file = new LocateFile();
                    string file_path = file.GetFilePath(i);
                    //string file_path = "E:\\Web-Folders\\Students\\Spring\\CPDM-290-200\\CPDM-WinslowS\\CSV_Folder\\Bakery.csv"; //"C:/Users/winsl/OneDrive/Desktop/Capstone/MVC/Views/SendMailer/Bakery.csv"; //"E:/Web-Folders/Students/Spring/CPDM-290-200/CPDM-WinslowS/Views/SendMailer/Bakery.csv";
                    mailMessage.Attachments.Add(new Attachment(file_path));
                    mailMessage.To.Add(new MailAddress(locationMailModel.Recipient));
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("gcrbadata@gmail.com", "sazjptmmfdiyebom");
                    smtp.EnableSsl = true;
                    smtp.Send(mailMessage);
                }
                i += 1;
            } while (locationList.lstLocations[i] != null);
            delete deleteFiles = new delete();
            string folder_path = HttpContext.Current.Server.MapPath("\\CSV_Folder");
            deleteFiles.delete_files(folder_path);
        }

        public class LocateFile {
            public string GetFilePath(int i) {
                return HttpContext.Current.Server.MapPath("\\CSV_Folder\\Bakery" + i.ToString() + ".csv");
            }
        }

        public class delete {
            public void delete_files(string path) {
                System.IO.DirectoryInfo dinfo = null;
                try {
                    dinfo = new System.IO.DirectoryInfo(path);
                }
                catch { return; }
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
}

