import json
import random
from datetime import datetime, timedelta
import pymongo

# Define constants for the script
NUM_DAYS = 365
NUM_HOURS_PER_DAY = 24
WATER_HEATER_MIN_CONSUMPTION = 1
WATER_HEATER_MAX_CONSUMPTION = 1.4
DAYTIME_ACTIVE_HOURS = 5
DAYTIME_ACTIVE_CONSUMPTION_FACTOR = 1
OTHER_HOURS_CONSUMPTION_FACTOR = 0
MIN_ACTIVE_HOURS = 3
MAX_ACTIVE_HOURS = 5

consumption_data = []

start_date = datetime(2023, 1, 1)

for day in range(NUM_DAYS):
    active_hours = random.randint(MIN_ACTIVE_HOURS, MAX_ACTIVE_HOURS)
    active_start_hour = random.randint(0, NUM_HOURS_PER_DAY - active_hours)
    for hour in range(NUM_HOURS_PER_DAY):
        if hour >= 6 and hour <= 20:
            time_of_day = "day"
        else:
            time_of_day = "night"
        if time_of_day == "day" and hour >= active_start_hour and hour < active_start_hour + active_hours:
            factor = DAYTIME_ACTIVE_CONSUMPTION_FACTOR
        else:
            factor = OTHER_HOURS_CONSUMPTION_FACTOR
        consumption = round(random.uniform(WATER_HEATER_MIN_CONSUMPTION * factor, WATER_HEATER_MAX_CONSUMPTION * factor), 2)
        timestamp = start_date + timedelta(hours=hour)
        consumption_data.append({
            "timestamp": timestamp,
            "value": consumption
        })
    start_date += timedelta(days=1)

client = pymongo.MongoClient("mongodb://localhost:27017")
db = client["database"]

device_data = {
    "type": "water_heater",
    "usage": consumption_data
}
db.devices.insert_one(device_data)

client.close()
