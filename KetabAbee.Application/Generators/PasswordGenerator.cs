using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.Generators
{
    public class PasswordGenerator
    {
        public static string GenerateStrongPassword()
        {
            return Guid.NewGuid().ToString("d").Substring(1, 8);
        }
    }
}
