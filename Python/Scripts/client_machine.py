import arcpy
import os
import pandas as pd
import glob

#delete old shapefiles in Output folder
files = glob.glob('C:/temp/Output/geocodeResult_fromScript.*')

for f in files:
    try:
        os.remove(f)
    except OSError as e:
        print("Error: %s : %s" % (f, e.strerror))

arcpy.env.workspace = "C:/Users/winsl/OneDrive/Desktop/Capstone/Locations/workspace/Client_Machine"
outputGdb = r"C:\Users\winsl\OneDrive\Desktop\Capstone\Locations\workspace\Client_Machine\Client_Machine.gdb"

arcpy.SignInToPortal(arcpy.GetActivePortalURL(),
                     'winslows2', 'NuevoEstudiante#1')

arcpy.env.overwriteOutput = True

Locations = r"C:\Users\winsl\OneDrive\Desktop\Capstone\MVC\CSV_Folder\LocationCSV.csv"
data = pd.read_csv(Locations, encoding='utf8')
print(data)
ArcGIS_World_Geocoding_Service = "https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/ArcGIS World Geocoding Service"
geocodeResult = os.path.join(outputGdb, "geocodeResult_fromScript")

# Process: Geocode Addresses (Geocode Addresses)
geocode_results = "r\\...\\Client_Machine.gdb\\geocode_results"
arcpy.GeocodeAddresses_geocoding(Locations, ArcGIS_World_Geocoding_Service,
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

outputDir = "C:/temp/Output"
arcpy.FeatureClassToShapefile_conversion([geocodeResult], outputDir)

print('Job Finished')

