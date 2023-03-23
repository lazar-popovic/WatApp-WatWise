import json
import random
from datetime import datetime, timedelta, timezone

# Define constants for the script
NUM_DAYS = 365
NUM_HOURS_PER_DAY = 24
SOLAR_PANEL_MIN_PRODUCTION = 6.0
SOLAR_PANEL_MAX_PRODUCTION = 10.0
PRODUCTION_FACTORS = {
    "night": 0,
    "6am-10am": 0.5,
    "10am-5pm": 1,
    "6pm-8pm": 0.5
}

# Define a list to store the production data
production_data = []

# Define start date for the year
start_date = datetime(2023, 1, 1)

# Loop over each day in 2023
for day in range(NUM_DAYS):
    # Loop over each hour of the day
    for hour in range(NUM_HOURS_PER_DAY):
        # Determine the current time of day (night, morning, afternoon, or evening)
        if hour < 6 or hour >= 20:
            time_of_day = "night"
        elif hour >= 6 and hour < 10:
            time_of_day = "6am-10am"
        elif hour >= 10 and hour < 18:
            time_of_day = "10am-5pm"
        else:
            time_of_day = "6pm-8pm"
        # Determine the factor to apply to the production range based on the time of day
        factor = PRODUCTION_FACTORS[time_of_day]
        # Generate a random production value between the minimum and maximum, with a factor based on the time of day
        production = round(random.uniform(SOLAR_PANEL_MIN_PRODUCTION, SOLAR_PANEL_MAX_PRODUCTION) * factor, 2)
        # Convert the datetime to epoch milliseconds
        dt_epoch_milliseconds = int(start_date.replace(hour=hour, minute=0, second=0, microsecond=0, tzinfo=timezone.utc).timestamp() * 1000)
        # Append the production value to the list
        production_data.append({
            "datetime": start_date.replace(hour=hour, minute=0, second=0, microsecond=0).isoformat(),
            "consumption": production
        })
    # Increment the start_date to next day
    start_date += timedelta(days=1)

# Write the production data to a JSON file
with open('solar_panel_production.json', 'w') as outfile:
    json.dump(production_data, outfile)
