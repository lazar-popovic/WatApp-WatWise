namespace API.Services.DeviceScheduling;

public static class CronMaker
{
    public static string ToCron(DateTime dateTime)
    {
        //{dateTime.Day} {dateTime.Month} {(int)dateTime.DayOfWeek}
        return $"{dateTime.Minute} {dateTime.Hour} * * *";
    }

}