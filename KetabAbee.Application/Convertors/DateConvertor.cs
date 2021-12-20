using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.Convertors
{
  public static  class DateConvertor
    {
        public static string ToShamsi(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();

            return pc.GetYear(date) + "/" + pc.GetMonth(date).ToString("00") + "/" + pc.GetDayOfMonth(date).ToString("00");
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
    }
}
