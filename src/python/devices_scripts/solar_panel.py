import json
import random
from datetime import datetime, timedelta
import pymongo

NUM_DAYS = 365
NUM_HOURS_PER_DAY = 24
SOLAR_PANEL_MIN_PRODUCTION = 0.25
SOLAR_PANEL_MAX_PRODUCTION = 0.4
PRODUCTION_FACTORS = {
    "night": 0,
    "6am-10am": 0.5,
    "10am-5pm": 1,
    "6pm-8pm": 0.5
}

production_data = []

start_date = datetime(2023, 1, 1)

for day in range(NUM_DAYS):
    for hour in range(NUM_HOURS_PER_DAY):
        if hour < 6 or hour >= 20:
            time_of_day = "night"
        elif hour >= 6 and hour < 10:
            time_of_day = "6am-10am"
        elif hour >= 10 and hour < 18:
            time_of_day = "10am-5pm"
        else:
            time_of_day = "6pm-8pm"
        factor = PRODUCTION_FACTORS[time_of_day]
        production = round(random.uniform(SOLAR_PANEL_MIN_PRODUCTION, SOLAR_PANEL_MAX_PRODUCTION) * factor, 3)
        timestamp = start_date + timedelta(hours=hour)
        production_data.append({
            "timestamp": timestamp,
            "value": production
        })
    start_date += timedelta(days=1)

client = pymongo.MongoClient("mongodb://localhost:27017")
db = client["database"]

device_data = {
    "type": "solar_panel",
    "usage": production_data
}
db.devices.insert_one(device_data)

client.close()