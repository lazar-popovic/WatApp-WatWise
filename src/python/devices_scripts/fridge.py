import pymongo
import json
from datetime import datetime, timedelta
import random

NUM_DAYS = 365
NUM_HOURS_PER_DAY = 24
FRIDGE_MIN_CONSUMPTION = 0.04
FRIDGE_MAX_CONSUMPTION = 0.05

fridge_consumption_data = []

start_date = datetime(2023, 1, 1)

for day in range(NUM_DAYS):
    for hour in range(NUM_HOURS_PER_DAY):
        fridge_consumption = round(random.uniform(FRIDGE_MIN_CONSUMPTION, FRIDGE_MAX_CONSUMPTION), 3)
        timestamp = start_date + timedelta(hours=hour)
        fridge_consumption_data.append({
            "timestamp": timestamp,
            "value": fridge_consumption
        })
    start_date += timedelta(days=1)

client = pymongo.MongoClient("mongodb://localhost:27017")
db = client["database"]

device_data = {
    "type": "fridge",
    "usage": fridge_consumption_data
}
db.devices.insert_one(device_data)

client.close()