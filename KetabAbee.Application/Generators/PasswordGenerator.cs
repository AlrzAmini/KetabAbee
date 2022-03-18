using System;
using System.Web;

namespace KetabAbee.Application.Generators
{
    public class PasswordGenerator
    {
        public static string GenerateStrongPassword()
        {
            char[] chars = {
                    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
                    'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                    'u', 'v', 'w', 'x', 'y', 'z','A', 'B', 'C', 'D',
                    'E', 'F', 'G', 'H','I', 'J', 'K', 'L', 'M', 'N',
                    'O', 'P','Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
                    'Y', 'Z','0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                    '!','@','$','%','^','&','*','-',
                    '~',')','(','+','=','{',']','}','[',
                    ':',';','<','>','.','?','/','|'
            };
            var password = string.Empty;
            var random = new Random();

            for (byte a = 0; a < 10; a++)
            {
                password += chars[random.Next(0, chars.Length)];
            }

            return password;
        }
    }
}
