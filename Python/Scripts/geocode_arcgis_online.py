from arcgis.geocoding import geocode
from arcgis.gis import GIS
from arcgis.gis import User
import pandas as pd

gis = GIS("https://www.arcgis.com/", 'winslows2', 'NuevoEstudiante#1')

item_search = gis.content.search(query='title:"append"', item_type="csv")
print(item_search)

for i in item_search:
    try:
        i.delete()
        print("item deleted: " + str(i))
    except Exception as err:
        print(err)

item_search = gis.content.search(query='title:"append"', item_type="Feature *")
print(item_search)

for i in item_search:
    try:
        i.delete()
        print("item deleted: " + str(i))
    except Exception as err:
        print(err)

item_search = gis.content.search(query='title:"Bakery"', item_type="Feature *")
print(item_search)

for i in item_search:
    try:
        i.delete()
        print("item deleted: " + str(i))
    except Exception as err:
        print(err)

Locations = "C://Users/winsl/OneDrive/Desktop/Locations/Bakery.csv"

location_data = pd.read_csv(Locations)
print(location_data)

bakeries = gis.content.get('425ac75b893547a89d51e18cb6658ae2')

newLocation_properties = {"title": "appendBakery"}

appendLocation = gis.content.add(data=Locations, item_properties=newLocation_properties)
location_item = appendLocation.publish()

layer2append = gis.content.search(query='title:"appendBakery"', item_type="Feature *")[0]
count = 0

user = User(gis, "winslows2")
folders = user.folders
for folder in folders:
    items = user.items(folder=folder["title"])
    for item in items:
        count += 1

gdbfile = 'Bakery' + str(count)

item_gdb = layer2append.export(title=gdbfile, export_format='File Geodatabase')
item_gdb.move(folder='Bakery_Locations')

# This is where the breakdown happens
status = bakeries.layers[0].append(item_id=item_gdb.id,
                                   upload_format='filegdb',
                                   source_table_name='appendBakery',
                                   upsert=False)
print(status)
