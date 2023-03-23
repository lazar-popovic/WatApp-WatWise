import pymongo
import json
from datetime import datetime, timedelta
import random

NUM_DAYS = 365
NUM_HOURS_PER_DAY = 24
FRIDGE_MIN_CONSUMPTION = 0.04
FRIDGE_MAX_CONSUMPTION = 0.05

consumption_data = []

start_date = datetime(2023, 1, 1)

for day in range(NUM_DAYS):
    for hour in range(NUM_HOURS_PER_DAY):
        consumption = round(random.uniform(FRIDGE_MIN_CONSUMPTION, FRIDGE_MAX_CONSUMPTION), 3)
        timestamp = start_date + timedelta(hours=hour)
        consumption_data.append({
            "timestamp": timestamp,
            "value": consumption
        })
    start_date += timedelta(days=1)

client = pymongo.MongoClient("mongodb://localhost:27017")
db = client["devices"]

db.drop_collection("fridge")
db.create_collection("fridge", timeseries={"timeField": "timestamp"})

db.fridge.insert_many(consumption_data)

client.close()
