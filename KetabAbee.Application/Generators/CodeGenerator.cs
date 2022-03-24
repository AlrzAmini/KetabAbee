using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.Generators
{
    public class CodeGenerator
    {
        public static string GenerateUniqCode()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string ActivationCode()
        {
            return new Random().Next(10000, 99999).ToString();
        }
    }
}
