from arcgis.geocoding import geocode
from arcgis.gis import GIS
import pandas as pd


gis = GIS("https://www.arcgis.com/", 'winslows2', 'NuevoEstudiante#1')

Locations = "C://Users/winsl/OneDrive/Desktop/Capstone/MVC/CSV_Folder/LocationCSV2.csv"

newLocation_properties = {"title": "testLoc"}

appendLocation = gis.content.add(data=Locations, item_properties=newLocation_properties)
location_item = appendLocation.publish()
