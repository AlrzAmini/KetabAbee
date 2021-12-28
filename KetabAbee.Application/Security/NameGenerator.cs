using System;

namespace KetabAbee.Application.Security
{
    public static class NameGenerator
    {
        public static string WalletNumberGenerator(int walletId)
        {
            var number = (walletId * walletId) * (walletId + 1) * (walletId * 3);

            var res = (int)Math.Ceiling((decimal)number);

            return "KTA - " + res;
        }

    }
}
