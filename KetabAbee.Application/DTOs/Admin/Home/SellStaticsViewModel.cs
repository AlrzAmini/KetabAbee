using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Home
{
    public class SellStaticsViewModel
    {
        #region order count

        public int LastDayOrdersCount { get; set; }
        public int LastWeekOrdersCount { get; set; }
        public int LastMonthOrdersCount { get; set; }
        public int LastYearOrdersCount { get; set; }

        #endregion

        #region income

        public float LastDayOrdersIncome { get; set; }
        public float LastWeekOrdersIncome { get; set; }
        public float LastMonthOrdersIncome { get; set; }
        public float LastYearOrdersIncome { get; set; }

        #endregion

        #region most selling book

        public List<string> MostSellingBook { get; set; }

        #endregion

        #region most selling book categoris

        public List<string> MostSellingBookCategories { get; set; }

        #endregion

        #region order status percentage

        public int IsSendPercent { get; set; }

        public int IsNotSendPercent { get; set; }

        #endregion
    }
}
