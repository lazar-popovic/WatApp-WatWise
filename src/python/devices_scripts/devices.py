import pymongo
from datetime import datetime, timedelta
import random

client = pymongo.MongoClient("mongodb://localhost:27017")
db = client["database"]

def fridge():
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


    device_data = {
        "type": 1,
        "name": "Fridge",
        "usage": fridge_consumption_data
    }
    db.devices.insert_one(device_data)
    
def freezer():
    NUM_DAYS = 365
    NUM_HOURS_PER_DAY = 24
    FREEZER_MIN_CONSUMPTION = 0.04
    FREEZER_MAX_CONSUMPTION = 0.06
    freezer_consumption_data = []
    start_date = datetime(2023, 1, 1)
    for day in range(NUM_DAYS):
        for hour in range(NUM_HOURS_PER_DAY):
            freezer_consumption = round(random.uniform(FREEZER_MIN_CONSUMPTION, FREEZER_MAX_CONSUMPTION), 3)
            timestamp = start_date + timedelta(hours=hour)
            freezer_consumption_data.append({
                "timestamp": timestamp,
                "value": freezer_consumption
            })
        start_date += timedelta(days=1)


    # Create a new device with the freezer usage data
    device_data = {
        "type": 5,
        "name": "Freezer",
        "usage": freezer_consumption_data
    }
    db.devices.insert_one(device_data)

def solar_panel():
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


    device_data = {
        "type": 3,
        "name": "Solar panel",
        "usage": production_data
    }
    db.devices.insert_one(device_data)

def water_heater():
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


    device_data = {
        "type": 2,
        "name": "Water heater",
        "usage": consumption_data
    }
    db.devices.insert_one(device_data)

def desktop_computer():
    NUM_DAYS = 365
    NUM_HOURS_PER_DAY = 24
    COMPUTER_MIN_CONSUMPTION = 0.05
    COMPUTER_MAX_CONSUMPTION = 0.25

    consumption_data = []

    start_date = datetime(2023, 1, 1)

    for day in range(NUM_DAYS):
        for hour in range(NUM_HOURS_PER_DAY):
            if hour >= 8 and hour <= 17: # Assuming the computer is used for 8-10 hours between 8am and 6pm
                consumption = round(random.uniform(COMPUTER_MIN_CONSUMPTION, COMPUTER_MAX_CONSUMPTION), 3)
            else:
                consumption = 0.0 # Assuming the computer is not used outside of working hours
            timestamp = start_date + timedelta(hours=hour)
            consumption_data.append({
                "timestamp": timestamp,
                "value": consumption
            })
        start_date += timedelta(days=1)

    device_data = {
        "type": 4,
        "name": "Desktop computer",
        "usage": consumption_data
    }
    
    db.devices.insert_one(device_data)

fridge()
water_heater()
solar_panel()
desktop_computer()
freezer()

client.close()  