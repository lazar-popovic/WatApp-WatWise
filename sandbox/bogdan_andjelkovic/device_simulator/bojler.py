import json
import random
from datetime import datetime, timedelta

# Define constants for the script
NUM_DAYS = 365
NUM_HOURS_PER_DAY = 24
WATER_HEATER_MIN_CONSUMPTION = 3.0
WATER_HEATER_MAX_CONSUMPTION = 5.0
DAYTIME_ACTIVE_HOURS = 5
DAYTIME_ACTIVE_CONSUMPTION_FACTOR = 1
OTHER_HOURS_CONSUMPTION_FACTOR = 0.05

# Define a dictionary to store the consumption data
consumption_data = {}

# Define start date for the year
start_date = datetime(2023, 1, 1)

# Loop over each day in 2022
for day in range(NUM_DAYS):
    # Define a list to store the consumption data for the day
    day_consumption = dict()
    # Loop over each hour of the day
    for hour in range(NUM_HOURS_PER_DAY):
        # Determine the current time of day (day or night)
        if hour >= 6 and hour <= 20:
            time_of_day = "day"
        else:
            time_of_day = "night"
        # Determine the factor to apply to the consumption range based on the time of day and active hours
        if time_of_day == "day" and hour < 6 + DAYTIME_ACTIVE_HOURS:
            factor = DAYTIME_ACTIVE_CONSUMPTION_FACTOR
        else:
            factor = OTHER_HOURS_CONSUMPTION_FACTOR
        # Generate a random consumption value between the minimum and maximum, with a factor based on the time of day and active hours
        consumption = round(random.uniform(WATER_HEATER_MIN_CONSUMPTION * factor, WATER_HEATER_MAX_CONSUMPTION * factor), 2)
        # Append the consumption value to the day's list
        hour_str = str(hour).zfill(2)
        day_consumption[hour_str+":00"]=consumption
    # Add the day's consumption data to the dictionary with key as date string in mm/dd/yyyy format
    consumption_data[start_date.strftime('%m/%d/%Y')] = day_consumption
    # Increment the start_date to next day
    start_date += timedelta(days=1)

# Write the consumption data to a JSON file
with open('water_heater_consumption.json', 'w') as outfile:
    json.dump(consumption_data, outfile)
