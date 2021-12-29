using System;

namespace KetabAbee.Application.Security
{
    public static class NameGenerator
    {
        public static string WalletNumberGenerator(int walletId)
        {
            //var number = (walletId + walletId) * (walletId + (walletId + walletId) + 7) / walletId * (walletId * 3);
            //var res = (int)Math.Ceiling((decimal)number);

            var random = new Random();
            var number = random.Next(walletId, 9999);

            return "KTA - " + number;
        }

    }
}
