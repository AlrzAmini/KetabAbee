using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime date)
        {
            var pc = new PersianCalendar();

            return pc.GetYear(date) + "/" + pc.GetMonth(date).ToString("00") + "/" + pc.GetDayOfMonth(date).ToString("00");
        }

        public static string ToShamsi(this DateTime? date)
        {
            var pc = new PersianCalendar();

            if (date == null)
            {
                return default;
            }

            return pc.GetYear((DateTime)date) + "/" + pc.GetMonth((DateTime)date).ToString("00") + "/" + pc.GetDayOfMonth((DateTime)date).ToString("00");
        }

        public static DateTime ToMiladi(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, new PersianCalendar());


            //    static void Main()
            //    {
            //        PersianCalendar pc = new PersianCalendar();
            //        DateTime dt = new DateTime(1391, 4, 7, pc);
            //        Console.WriteLine(dt.ToString(CultureInfo.InvariantCulture));
            //    }
        }

        public static string GetHourAndMinutes(this DateTime value)
        {
            return value.ToString("HH:mm");
        }

        public static string ToIraniDate(this DateTime dt)
        {
            PersianCalendar PC = new PersianCalendar();
            int intYear = PC.GetYear(dt);
            int intMonth = PC.GetMonth(dt);
            int intDayOfMonth = PC.GetDayOfMonth(dt);
            DayOfWeek enDayOfWeek = PC.GetDayOfWeek(dt);
            string strMonthName = "";
            string strDayName = "";
            switch (intMonth)
            {
                case 1:
                    strMonthName = "فروردین";
                    break;
                case 2:
                    strMonthName = "اردیبهشت";
                    break;
                case 3:
                    strMonthName = "خرداد";
                    break;
                case 4:
                    strMonthName = "تیر";
                    break;
                case 5:
                    strMonthName = "مرداد";
                    break;
                case 6:
                    strMonthName = "شهریور";
                    break;
                case 7:
                    strMonthName = "مهر";
                    break;
                case 8:
                    strMonthName = "آبان";
                    break;
                case 9:
                    strMonthName = "آذر";
                    break;
                case 10:
                    strMonthName = "دی";
                    break;
                case 11:
                    strMonthName = "بهمن";
                    break;
                case 12:
                    strMonthName = "اسفند";
                    break;
                default:
                    strMonthName = "";
                    break;
            }

            switch (enDayOfWeek)
            {
                case DayOfWeek.Friday:
                    strDayName = "جمعه";
                    break;
                case DayOfWeek.Monday:
                    strDayName = "دوشنبه";
                    break;
                case DayOfWeek.Saturday:
                    strDayName = "شنبه";
                    break;
                case DayOfWeek.Sunday:
                    strDayName = "یکشنبه";
                    break;
                case DayOfWeek.Thursday:
                    strDayName = "پنجشنبه";
                    break;
                case DayOfWeek.Tuesday:
                    strDayName = "سه شنبه";
                    break;
                case DayOfWeek.Wednesday:
                    strDayName = "چهارشنبه";
                    break;
                default:
                    strDayName = "";
                    break;
            }

            return (string.Format("{0} {1} {2} {3}", strDayName, intDayOfMonth, strMonthName, intYear));
        }

        public static DateTime StringShamsiToMiladi(this string shamsi)
        {
            var pc = new PersianCalendar();

            var year = int.Parse(shamsi.Substring(6, 4));
            var month = int.Parse(shamsi.Substring(0, 2));
            var day = int.Parse(shamsi.Substring(3, 2));

            var dt = pc.ToDateTime(year, month, day, 0, 0, 0, 0);

            return dt;
        }

        public static string GetEnglishNumbers(this string s)
        {
            return s.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
        }

        public static string GetPersianNumbers(this string s)
        {
            return s.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴").Replace("5", "۵").Replace("6", "۶").Replace("7", "۷").Replace("8", "۸").Replace("9", "۹");
        }

        public static DateTime ToMiladiDateTime(this string ts)
        {
            var splitDate = ts.GetEnglishNumbers().Split('/');
            var year = int.Parse(splitDate[0]);
            var month = int.Parse(splitDate[1]);
            var day = int.Parse(splitDate[2]);
            var currentDate = new DateTime(year, month, day, new PersianCalendar());
            return currentDate;
        }

        public static string ShowAudioBookTime(this int time)
        {
            switch (time)
            {
                case < 60:
                    return time + " دقیقه ";
                case 60:
                    return 1 + " ساعت ";
                case > 60:
                    {
                        var hours = time / 60;
                        var minutes = time % 60;

                        if (minutes == 0)
                        {
                            return hours + " ساعت ";
                        }

                        return hours + " ساعت " + minutes + " دقیقه ";
                    }
            }
        }
    }
}
