namespace API.Services.DeviceScheduling;

public static class CronMaker
{
    public static string ToCron(DateTime dateTime)
    {
        return $"{dateTime.Minute} {dateTime.Hour} {dateTime.Day} {dateTime.Month} {(int)dateTime.DayOfWeek}";
    }

}