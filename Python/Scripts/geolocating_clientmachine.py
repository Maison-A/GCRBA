import arcpy
import os
import pandas as pd

outputGdb = r"C:\Users\winsl\OneDrive\Desktop\Capstone\Python\anotherTestProject\anotherTestProject.gdb"

arcpy.SignInToPortal(arcpy.GetActivePortalURL(),
                     'winslows2', 'NuevoEstudiante#1')

arcpy.env.overwriteOutput = True

WildlifeOperators = r"C:\Users\winsl\OneDrive\Desktop\Capstone\Python\SampleLocation.csv"
data = pd.read_csv(WildlifeOperators, encoding='utf8')
print(data)
ArcGIS_World_Geocoding_Service = "https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/ArcGIS World Geocoding Service"
geocodeResult = os.path.join(outputGdb, "geocodeResult_fromScript")

# Process: Geocode Addresses (Geocode Addresses)
geocode_results = "r\\...\\anotherTestProject.gdb\\geocode_results"
arcpy.GeocodeAddresses_geocoding(WildlifeOperators, ArcGIS_World_Geocoding_Service,
                "Address Street VISIBLE NONE;" +
                "Address2 <None> VISIBLE NONE;" +
                "Address3 <None> VISIBLE NONE;" +
                "Neighborhood <None> VISIBLE NONE;" +
                "City City VISIBLE NONE;" +
                "County <None> VISIBLE NONE;" +
                "State State VISIBLE NONE;" +
                "ZIP ZIP VISIBLE NONE;" +
                "ZIP4 <None> VISIBLE NONE;" +
                "Country <None> VISIBLE NONE", geocodeResult, "STATIC")