import pymongo
import json
from datetime import datetime, timedelta
import random

NUM_DAYS = 365
NUM_HOURS_PER_DAY = 24
FREEZER_MIN_CONSUMPTION = 0.04
FREEZER_MAX_CONSUMPTION = 0.06

freezer_consumption_data = []

start_date = datetime(2023, 1, 1)

for day in range(NUM_DAYS):
    for hour in range(NUM_HOURS_PER_DAY):
        freezer_consumption = round(random.uniform(FREEZER_MIN_CONSUMPTION, FREEZER_MAX_CONSUMPTION), 3)
        timestamp = start_date + timedelta(hours=hour)
        freezer_consumption_data.append({
            "timestamp": timestamp,
            "value": freezer_consumption
        })
    start_date += timedelta(days=1)

client = pymongo.MongoClient("mongodb://localhost:27017")
db = client["database"]

# Create a new device with the freezer usage data
device_data = {
    "type": "freezer",
    "usage": freezer_consumption_data
}
db.devices.insert_one(device_data)

client.close()
