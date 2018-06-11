using StudyApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace StudyApp.Helper
{
    public class DateHelper
    {

        public static List<string> DaysOfWeek = new List<string>
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"
        };

        public static TimeSpan GetTime()
        {
            return DateTime.Now.TimeOfDay;
        }

        public async static Task<int> GetWeek()
        {
            SemesterModel currSemester = await SemesterModel.GetLastAsync();
            int week = 0;

            if (currSemester != null)
            {
                Calendar c = new GregorianCalendar();
                int currWeek = c.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int firstWeek = c.GetWeekOfYear(currSemester.StartDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                week = ((currWeek - firstWeek) % 2) + 1;
            }

            return week;
        }

        public static int GetDayOfWeek()
        {
            return DaysOfWeek.IndexOf(DateTime.Now.DayOfWeek.ToString());
        }
    }
}
