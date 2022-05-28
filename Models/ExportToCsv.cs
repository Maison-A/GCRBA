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
    /* This class is specially designed to store information collected from the new location form
     * DO NOT CHANGE THIS CLASS. THE ORDER AND NAMES OF THE VARIABLES IN THIS CLASS ARE VERY IMPORTANT.
     * IT WILL CAUSE MAJOR DISRUPTION TO THE AUTOMATION OF GIS AND MAPPING!!!!!!!!!!!!!
     */
	public class ExportToCsv {
        static void Main(Models.LocationList locList, List<string>[] Location, List<string>[] businessInfo, List<Models.CategoryItem>[] specialties, List<Models.Days>[] operations, List<Models.ContactPerson>[] Contact, List<Models.SocialMedia>[] socialMedias, List<Models.Website>[] websites) {
            StartExport(locList, Location, businessInfo, specialties, operations, Contact, socialMedias, websites);            
        }

        public static void StartExport(Models.LocationList locList, List<string>[] Location, List<string>[] businessInfo, List<Models.CategoryItem>[] Specialties, List<Models.Days>[] operations, List<Models.ContactPerson>[] Contact, List<Models.SocialMedia>[] socialMedias, List<Models.Website>[] websites) {
            int i = 0;
            List<string>[] formatSchedule = new List<string>[100];
            List<string>[] formatLinks = new List<string>[100];
            List<string>[] formatSocialMedia = new List<string>[100];
            List<string>[] formatContacts = new List<string>[100];
            List<string>[] formatSpecialty = new List<string>[100];

            foreach (Models.NewLocation ls in locList.lstLocations) {
                if (Location[i] != null) {
                    try {
                        var websiteURL = new List<string>();
                        foreach (Models.Website item in websites[i]) {
                            websiteURL.Add(item.strURL);
                        }
                        formatLinks[i] = websiteURL;

                        var hourOperations = new List<string>();
                        foreach (Models.Days item in operations[i]) {
                            //if (item.strOpenTime != string.Empty) item.dtOpenTime = Convert.ToDateTime(item.strOpenTime);
                            //if (item.strClosedTime != string.Empty) item.dtClosedTime = Convert.ToDateTime(item.strClosedTime);
                            string schedule = string.Empty;
                            if (item.strOpenTime == "Closed" || item.strClosedTime == "Closed") schedule = "Closed";
                            else schedule = item.dtOpenTime.ToShortTimeString() + '-' + item.dtClosedTime.ToShortTimeString();
                            hourOperations.Add(schedule);
                        }
                        formatSchedule[i] = hourOperations;

                        var specialties = new List<string>();
                        foreach (Models.CategoryItem item in Specialties[i]) {
                            string bakedgood = Convert.ToString(item.blnAvailable);
                            specialties.Add(bakedgood);
                        }
                        formatSpecialty[i] = specialties;

                        var socialMediaLinks = new List<string>();
                        foreach (Models.SocialMedia item in socialMedias[i]) {
                            socialMediaLinks.Add(item.strSocialMediaLink);
                        }
                        formatSocialMedia[i] = socialMediaLinks;

                        var contacts = new List<string>();
                        foreach (Models.ContactPerson item in Contact[i]) {
                            string emptyContactName = string.Empty;
                            string emptyPhone = string.Empty;

                            if (item.LastName != string.Empty && item.FirstName != string.Empty) contacts.Add(item.LastName + ',' + item.FirstName);
                            else contacts.Add(emptyContactName);

                            if (item.ContactPhone.AreaCode != string.Empty && item.ContactPhone.Prefix != string.Empty && item.ContactPhone.Suffix != string.Empty) {
                                contacts.Add('(' + item.ContactPhone.AreaCode + ") " + item.ContactPhone.Prefix + '-' + item.ContactPhone.Suffix);
                            }
                            else contacts.Add(emptyPhone);
                            contacts.Add(item.Email);
                        }
                        formatContacts[i] = contacts;

                    }
                    catch (Exception ex) {
                        throw new Exception(ex.Message);
                    }
                    i += 1;
                }
                else break;
            }
            Export(locList, Location, businessInfo, formatSpecialty, formatSchedule, formatContacts, formatLinks, formatSocialMedia);
        }
        
        public static void Export(Models.LocationList locList, List<string>[] Location, List<string>[] businessInfo, List<string>[] SpecialtiesList, List<string>[] hourOperations, List<string>[] Contact, List<string>[] websiteLinks, List<string>[] socialMediaLinks) {
            int i = 0;
            foreach (Models.NewLocation ls in locList.lstLocations) {
                if (Location[i] != null) {
                    try {
                        int Index = 0;
                        LocateFile file = new LocateFile();
                        string file_path = file.GetFilePath();
                        //string file_path = "E:\\Web-Folders\\Students\\Spring\\CPDM-290-200\\CPDM-WinslowS\\CSV_Folder\\Bakery.csv"; //"C:/Users/winsl/OneDrive/Desktop/Capstone/MVC/Views/SendMailer/Bakery.csv"; //"E:/Web-Folders/Students/Spring/CPDM-290-200/CPDM-WinslowS/Views/SendMailer/Bakery.csv";
                        using (StreamWriter streamWriter = new StreamWriter(file_path)) {
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

                                //NOTE THIS ORDER IS VERY IMPORTANT AS IT CORRESPONDS TO THE STATIC HEADERS ABOVE!
                                foreach (string item in Location[i]) {
                                    csvWriter.WriteField(item);
                                }
                               
                                foreach (string item in businessInfo[i]) {
                                    csvWriter.WriteField(item);
								}

                                foreach(string item in SpecialtiesList[i]) {
                                    csvWriter.WriteField(item);
								}

                                foreach(string item in hourOperations[i]) {
                                    csvWriter.WriteField(item);
								}

                                foreach(string item in Contact[i]) {
                                    csvWriter.WriteField(item);
								}

                                foreach(string item in websiteLinks[i]) {
                                    csvWriter.WriteField(item);
								}

                                foreach(string item in socialMediaLinks[i]) {
                                    csvWriter.WriteField(item);
								}

                                //End CsvWriter Session
                                csvWriter.Flush();

                            }
                            //End StreamWriter Session
                            streamWriter.Close();
                        }
                    }
                    catch (Exception ex) {
                        throw new Exception(ex.Message);
                    }
                    i += 1;
                }
                else break;
            }
        }  
	}
    public class LocateFile {
        public string GetFilePath() {
            try {
                int fCount = 0;
                string folder_path = HttpContext.Current.Server.MapPath("\\CSV_Folder");
                fCount = Directory.GetFiles(folder_path, "*", SearchOption.AllDirectories).Length;
                return HttpContext.Current.Server.MapPath("\\CSV_Folder\\Bakery" + fCount.ToString() + ".csv");
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}