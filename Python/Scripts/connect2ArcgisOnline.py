import arcpy
import os
import datetime as dt
from copy import deepcopy
from arcgis.gis import GIS
from arcgis.features import GeoAccessor
import pandas as pd
from arcgis.features import FeatureLayerCollection
from arcgis.features import FeatureLayer

gis = GIS(arcpy.GetActivePortalURL(), 'winslows2', 'NuevoEstudiante#1')

#Deletes the previous version of the file in the Content section of arcgis online
item_search = gis.content.search(query='title:"newLocation"', item_type="Shapefile")
print(item_search)

for i in item_search:
    try:
        i.delete()
        print("item deleted: " + str(i))
    except Exception as err:
        print(err)

bakeries = gis.content.get('cbed9565c6c74df6867d39118230fca9')

newLocation_properties = {"title": "newLocation",
                          "type": "Shapefile"}

addLocation = gis.content.add(data=r'C:\Users\winsl\OneDrive\Desktop\Capstone\MVC\Python\ZippedFiles\sampleFiles.zip',
                              item_properties=newLocation_properties)

status = bakeries.layers[0].append(item_id=addLocation.id,
                         upload_format='shapefile')

print(status)
