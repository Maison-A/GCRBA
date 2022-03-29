from arcgis.geocoding import geocode
from arcgis.gis import GIS
import pandas as pd

gis = GIS("https://www.arcgis.com/", 'winslows2', 'NuevoEstudiante#1')

item_search = gis.content.search(query='title:"appendLocation"', item_type="csv")
print(item_search)

for i in item_search:
    try:
        i.delete()
        print("item deleted: " + str(i))
    except Exception as err:
        print(err)

item_search = gis.content.search(query='title:"appendLocation"', item_type="Feature *")
print(item_search)

for i in item_search:
    try:
        i.delete()
        print("item deleted: " + str(i))
    except Exception as err:
        print(err)

Locations = "C://Users/winsl/OneDrive/Desktop/Capstone/MVC/CSV_Folder/LocationCSV.csv"

location_data = pd.read_csv(Locations)
print(location_data)

bakeries = gis.content.get('bdbbad03f14c47baa6402aa4ac95f1bd')

newLocation_properties = {"title": "appendLocation"}

appendLocation = gis.content.add(data=Locations, item_properties=newLocation_properties)
location_item = appendLocation.publish()

layer2append = gis.content.search(query='title:"appendLocation"', item_type="Feature *")[0]

item_shp = layer2append.export(title='item_shapefile', export_format='Shapefile')

status = bakeries.layers[0].append(item_id=item_shp.id,
                         upload_format='shapefile')
print(status)






