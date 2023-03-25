import pymongo
from datetime import datetime, timedelta
import random
import json

NUM_DAYS = 365
NUM_HOURS_PER_DAY = 24
COMPUTER_MIN_CONSUMPTION = 0.05
COMPUTER_MAX_CONSUMPTION = 0.25

consumption_data = []

start_date = datetime(2023, 1, 1)

for day in range(NUM_DAYS):
    daily_data = []
    for hour in range(NUM_HOURS_PER_DAY):
        if hour >= 8 and hour <= 17: # Assuming the computer is used for 8-10 hours between 8am and 6pm
            consumption = round(random.uniform(COMPUTER_MIN_CONSUMPTION, COMPUTER_MAX_CONSUMPTION), 3)
        else:
            consumption = 0.0 # Assuming the computer is not used outside of working hours
        timestamp = start_date + timedelta(hours=hour)
        daily_data.append({
            "timestamp": timestamp.isoformat(),
            "value": consumption
        })
    consumption_data.extend(daily_data)
    start_date += timedelta(days=1)

client = pymongo.MongoClient("mongodb://localhost:27017")
db = client["database"]

# Create a new device with the usage data
device_data = {
    "type": "desktop_computer",
    "usage": consumption_data
}
db.devices.insert_one(device_data)

client.close()
