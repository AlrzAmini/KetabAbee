using System;
using KetabAbee.Domain.Models.Wallet;

namespace KetabAbee.Application.DTOs.Admin.User
{
    public class UserWalletViewModel
    {
        public int WalletId { get; set; }

        public string UserName { get; set; }

        public WalletType WalletType { get; set; }

        public long Amount { get; set; }

        public string Behalf { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsPay { get; set; }
    }
}
