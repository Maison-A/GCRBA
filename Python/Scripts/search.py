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

item_search = gis.content.search(query='title:"newLocation"', item_type="Shapefile")
print(item_search)

for i in item_search:
    try:
        i.delete()
        print("item deleted: " + str(i))
    except Exception as err:
        print(err)