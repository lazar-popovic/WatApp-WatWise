import pymongo
import json
from datetime import datetime, timedelta
import random

# Define constants for the script
NUM_DAYS = 365
NUM_HOURS_PER_DAY = 24
FRIDGE_MIN_CONSUMPTION = 0.5
FRIDGE_MAX_CONSUMPTION = 1.5

# Define a list to store the consumption data
consumption_data = []

# Define start date for the year
start_date = datetime(2023, 1, 1)

# Loop over each day in 2023
for day in range(NUM_DAYS):
    # Loop over each hour of the day
    for hour in range(NUM_HOURS_PER_DAY):
        # Generate a random consumption value between the minimum and maximum
        consumption = round(random.uniform(FRIDGE_MIN_CONSUMPTION, FRIDGE_MAX_CONSUMPTION), 2)
        # Get the timestamp value as a datetime object
        timestamp = start_date + timedelta(hours=hour)
        # Append the consumption value to the list
        consumption_data.append({
            "timestamp": timestamp,
            "value": consumption
        })
    # Increment the start_date to next day
    start_date += timedelta(days=1)

# Connect to MongoDB
client = pymongo.MongoClient("mongodb://localhost:27017")
db = client["devices"]

# Create the timeseries collection
db.create_collection("fridge", timeseries={"timeField": "timestamp"})

# Insert the data into the timeseries collection
db.fridge.insert_many(consumption_data)

# Close the connection to MongoDB
client.close()
