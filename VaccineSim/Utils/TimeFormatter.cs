using System;

namespace VaccineSim.Utils
{
    public static class TimeFormatter
    {
        public static string SecondsToTime(double value)
        {
            return TimeSpan.FromSeconds(value).ToString(@"hh\:mm\:ss\.ff");
        }
        
        public static string SecondsToTime(int hours, double seconds)
        {
            return TimeSpan.FromSeconds(seconds).Add(TimeSpan.FromHours(hours)).ToString(@"hh\:mm\:ss");
        }
    }
}
