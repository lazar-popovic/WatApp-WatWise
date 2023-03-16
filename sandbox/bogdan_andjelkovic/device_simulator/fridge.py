import json
import random
from datetime import datetime, timedelta

# Define constants for the script
NUM_DAYS = 365
NUM_HOURS_PER_DAY = 24
FRIDGE_MIN_CONSUMPTION = 0.5
FRIDGE_MAX_CONSUMPTION = 1.5

# Define a dictionary to store the consumption data
consumption_data = {}

# Define start date for the year
start_date = datetime(2023, 1, 1)

# Loop over each day in 2022
for day in range(NUM_DAYS):
    # Define a list to store the consumption data for the day
    day_consumption = {}
    # Loop over each hour of the day
    for hour in range(NUM_HOURS_PER_DAY):
        # Determine the current time of day (day or night)
        if hour >= 6 and hour <= 20:
            time_of_day = "day"
        else:
            time_of_day = "night"
        # Set the factor to 1 for a fridge that works constantly
        factor = 1
        # Generate a random consumption value between the minimum and maximum, with a factor based on the time of day and active hours
        consumption = round(random.uniform(FRIDGE_MIN_CONSUMPTION * factor, FRIDGE_MAX_CONSUMPTION * factor), 2)
        # Format the hour value with leading zeroes
        hour_str = str(hour).zfill(2)
        # Append the consumption value to the day's list
        day_consumption[hour_str+":00"] = consumption
    # Add the day's consumption data to the dictionary with key as date string in mm/dd/yyyy format
    consumption_data[start_date.strftime('%m/%d/%Y')] = day_consumption
    # Increment the start_date to next day
    start_date += timedelta(days=1)

# Write the consumption data to a JSON file
with open('fridge_consumption.json', 'w') as outfile:
    json.dump(consumption_data, outfile)
