import os
import glob

files = glob.glob('C:/temp/Output/geocodeResult_fromScript.*')

for f in files:
    try:
        os.remove(f)
    except OSError as e:
        print("Error: %s : %s" % (f, e.strerror))