namespace API.Services.DeviceScheduling;

public static class CronMaker
{
    /*
    public static string ToCron(DateTime date) 
    {
        string mins = date.Minute.ToString();
        string hrs =  date.Hour.ToString();
        string dayOfMonth = date.Day.ToString();
        string month = date.Month.ToString();
        string dayOfWeek = date.DayOfWeek.ToString();
        string year = date.Year.ToString();
        
        return string.Format("{0} {1} {2} {3} {4} {5}", mins, hrs, dayOfMonth, month, dayOfWeek, year);
    }
    */
    public static string ToCron(DateTime dateTime)
    {
        return $"{dateTime.Minute} {dateTime.Hour} {dateTime.Day} {dateTime.Month} {(int)dateTime.DayOfWeek} {dateTime.Year}";
    }

}