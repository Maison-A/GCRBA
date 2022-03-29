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
        static void Main(List<string> Location, List<string> Contact, List<string> Specialties) {

            Export(Location, Contact, Specialties);
        }

        public static void Export(List<string> Location, List<string> Contact, List<string> Specialties) {
            int LocationIndex = 0;
            int SpecialtyIndex = 0;
            int ContactIndex = 0;
            using (StreamWriter streamWriter = new StreamWriter("C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\MVC\\CSV_Folder\\LocationCSV.csv")) {
                using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.CurrentCulture)) {
                    //Write Location Information Headers
                    csvWriter.WriteField("LocationName");
                    csvWriter.WriteField("Address");

                    //Write Contact Information Headers
                    csvWriter.WriteField("Contact_Name");
                    csvWriter.WriteField("Contact_Phone");
                    csvWriter.WriteField("Business_Phone");
                    csvWriter.WriteField("Business_Email");
                    csvWriter.WriteField("Business_Website");

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
                    
                    //End CSV Row
                    csvWriter.NextRecord();

                    while(LocationIndex < Location.Count()) {
                        csvWriter.WriteField(Location[LocationIndex]);
                        LocationIndex += 1;
                    }

                    while(ContactIndex < Contact.Count()) {
                        csvWriter.WriteField(Contact[ContactIndex]);
                        ContactIndex += 1;
					}

                    while(SpecialtyIndex < Specialties.Count()) {
                        csvWriter.WriteField(Specialties[SpecialtyIndex]);
                        SpecialtyIndex += 1;
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