using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.Convertors
{
    public static class PriceConvertor
    {
        public static string ToToman(this int value)
        {
            return value.ToString("#,0") + " تومان ";
        }
    }
}
