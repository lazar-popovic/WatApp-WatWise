import json
import random
from datetime import datetime, timedelta

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
        # Append the consumption value to the list
        consumption_data.append({
            "datetime": start_date.strftime('%Y-%m-%d %H:%M:%S'),
            "consumption": consumption
        })
    # Increment the start_date to next day
    start_date += timedelta(days=1)

# Write the consumption data to a JSON file
with open('fridge_consumption.json', 'w') as outfile:
    json.dump(consumption_data, outfile)
