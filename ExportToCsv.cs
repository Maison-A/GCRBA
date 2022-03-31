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

namespace GCRBA {
	public class ExportToCsv {
        static void Main(List<string> Location, List<string> businessInfo, List<Models.CategoryItem> specialties, List<Models.Days> operations, List<Models.ContactPerson> Contact, List<Models.SocialMedia> socialMedias, List<Models.Website> websites) {
            StartExport(Location, businessInfo, specialties, operations, Contact, socialMedias, websites);            
        }

        public static void StartExport(List<string> Location, List<string> businessInfo, List<Models.CategoryItem> Specialties, List<Models.Days> operations, List<Models.ContactPerson> Contact, List<Models.SocialMedia> socialMedias, List<Models.Website> websites) {
            var websiteLinks = new List<string>();
            foreach (Models.Website item in websites) {
                websiteLinks.Add(item.strURL);
            }

            var hourOperations = new List<string>();
            foreach (Models.Days item in operations) {
                string schedule = item.strOpenTime + '-' + item.strClosedTime;
                hourOperations.Add(schedule);
               };

            var specialties = new List<string>();
            foreach(Models.CategoryItem item in Specialties) {
                string bakedgood = Convert.ToString(item.blnAvailable);
                specialties.Add(bakedgood);
            };

            var socialMediaLinks = new List<string>();
            foreach(Models.SocialMedia item in socialMedias)
            {
                socialMediaLinks.Add(item.strSocialMediaLink);
            };

            var contacts = new List<string>();
            foreach(Models.ContactPerson item in Contact) {
                contacts.Add(item.strContactLastName + ',' + item.strContactFirstName);
                contacts.Add('(' + item.contactPhone.AreaCode + ") " + item.contactPhone.Prefix + '-' + item.contactPhone.Suffix);
                contacts.Add(item.strContactEmail);
			}

            Export(Location, businessInfo, specialties, hourOperations, contacts, websiteLinks, socialMediaLinks);
        }
        
        public static void Export(List<string> Location, List<string> businessInfo, List<string> SpecialtiesList, List<string> hourOperations, List<string> Contact, List<string> websiteLinks, List<string> socialMediaLinks) {
            int Index = 0;
            using (StreamWriter streamWriter = new StreamWriter("C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\MVC\\CSV_Folder\\LocationCSV.csv")) {
                using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.CurrentCulture)) {
                    //Write Location Information Headers
                    csvWriter.WriteField("Bakery_Name");
                    csvWriter.WriteField("Address");

                    //Write Business Information Headers
                    csvWriter.WriteField("Business_Phone");
                    csvWriter.WriteField("Business_Email");
                    csvWriter.WriteField("Business_Year");
                    csvWriter.WriteField("Business_Bio");
                    

                    //Write Specialty Information Headers
                    csvWriter.WriteField("Donuts");
                    csvWriter.WriteField("Bagels");
                    csvWriter.WriteField("Muffins");
                    csvWriter.WriteField("IceCream");
                    csvWriter.WriteField("Fine_Candies");
                    csvWriter.WriteField("Wedding_Cakes");
                    csvWriter.WriteField("Breads");
                    csvWriter.WriteField("Decorated_Cakes");
                    csvWriter.WriteField("Cupcakes");
                    csvWriter.WriteField("Cookies");
                    csvWriter.WriteField("Desserts_Tortes");
                    csvWriter.WriteField("Fullline_Bakery");
                    csvWriter.WriteField("Deli_Catering");
                    csvWriter.WriteField("Carryout_Deli");
                    csvWriter.WriteField("Wholesale");
                    csvWriter.WriteField("Delivery");
                    csvWriter.WriteField("Shipping");
                    csvWriter.WriteField("Online");

                    csvWriter.WriteField("Sunday");
                    csvWriter.WriteField("Monday");
                    csvWriter.WriteField("Tuesday");
                    csvWriter.WriteField("Wednesday");
                    csvWriter.WriteField("Thursday");
                    csvWriter.WriteField("Friday");
                    csvWriter.WriteField("Saturday");

                    //Write Contact Information Headers
                    csvWriter.WriteField("Location Contact Name");
                    csvWriter.WriteField("Location Contact Phone");
                    csvWriter.WriteField("Location Contact Email");

                    csvWriter.WriteField("Web Admin Contact Name");
                    csvWriter.WriteField("Web Admin Contact Phone");
                    csvWriter.WriteField("Web Admin Contact Email");

                    csvWriter.WriteField("Customer Service Contact Name");
                    csvWriter.WriteField("Customer Service Contact Phone");
                    csvWriter.WriteField("Customer Service Contact Email");

                    csvWriter.WriteField("Main Webpage");
                    csvWriter.WriteField("Ordering Webpage");
                    csvWriter.WriteField("Donation Kettle Website");

                    csvWriter.WriteField("Facebook");
                    csvWriter.WriteField("Twitter");
                    csvWriter.WriteField("Instagram");
                    csvWriter.WriteField("Snapchat");
                    csvWriter.WriteField("TikTok");
                    csvWriter.WriteField("Yelp");

                    //End CSV Row
                    csvWriter.NextRecord();

                    while(Index < Location.Count()) {
                        csvWriter.WriteField(Location[Index]);
                        Index += 1;
                    }

                    Index = 0;
                    while(Index < businessInfo.Count()) {
                        csvWriter.WriteField(businessInfo[Index]);
					}

                    Index = 0;
                    while (Index < SpecialtiesList.Count()) {
                        csvWriter.WriteField(SpecialtiesList[Index]);
                        Index += 1;
                    }

                    Index = 0;
                    while (Index < hourOperations.Count()) {
                        csvWriter.WriteField(hourOperations[Index]);
                        Index += 1;
                    }

                    Index = 0;
                    while (Index < Contact.Count()) {
                        csvWriter.WriteField(Contact[Index]);
                        Index += 1;
					}

                    Index = 0;
                    while (Index < websiteLinks.Count()) {
                        csvWriter.WriteField(websiteLinks[Index]);
                        Index += 1;
                    }

                    Index = 0;
                    while (Index < socialMediaLinks.Count()) {
                        csvWriter.WriteField(socialMediaLinks[Index]);
                        Index += 1;
                    }

                    //End CsvWriter Session
                    csvWriter.Flush();
					
                }
                //End StreamWriter Session
                streamWriter.Close();
		    }
           
        }  
	}
}