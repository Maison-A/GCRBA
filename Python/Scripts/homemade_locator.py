import arcpy
import os
import pandas as pd

arcpy.env.workspace = "C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\Locations\\workspace\\Homemade_Locator_Locations"
outputGdb = r"Homemade_Locator_Locations.gdb"

arcpy.SignInToPortal(arcpy.GetActivePortalURL(),'winslows2', 'NuevoEstudiante#1')

arcpy.env.overwriteOutput = True

WildlifeOperators = r"C:\Users\winsl\OneDrive\Desktop\Capstone\Locations\SampleLocation1.csv"
data = pd.read_csv(WildlifeOperators, encoding='utf8')
print(data)
locator ="Kenton_Locator.loc"
geocodeResult = os.path.join(outputGdb, "geocodeResult_fromScript")

# Process: Geocode Addresses (Geocode Addresses)
geocode_results = "r\...\Homemade_Locator_Locations.gdb\geocode_results"
arcpy.GeocodeAddresses_geocoding(WildlifeOperators, locator,
                "Address StreetNumber VISIBLE NONE;" +
                "Address2 StreetName VISIBLE NONE;" +
                "Address3 <None> VISIBLE NONE;" +
                "Neighborhood <None> VISIBLE NONE;" +
                "City City VISIBLE NONE;" +
                "County <None> VISIBLE NONE;" +
                "State State VISIBLE NONE;" +
                "ZIP Zip VISIBLE NONE;" +
                "ZIP4 <None> VISIBLE NONE;" +
                "Country <None> VISIBLE NONE", geocodeResult, "STATIC")