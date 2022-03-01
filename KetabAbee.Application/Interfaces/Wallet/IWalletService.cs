using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Wallet;
using KetabAbee.Application.DTOs.Wallet;

namespace KetabAbee.Application.Interfaces.Wallet
{
    public interface IWalletService
    {
        IEnumerable<Domain.Models.Wallet.Wallet> GetWalletsByUserId(int userId);

        long BalanceUserWallet(int userId);

        bool ChargeWalletByUserId(int userId,ChargeWalletViewModel charge,bool isPay = true);

        Domain.Models.Wallet.Wallet GetWalletById(int walletId);

        bool UpdateWallet(Domain.Models.Wallet.Wallet wallet);

        WalletsWithPagingViewModel GetWalletsWithPagingByUserId(WalletsWithPagingViewModel walletsWithPagingViewModel);

        bool ChargeWalletFromAdmin(ChargeWalletFromAdminViewModel charge);

        bool WithDrawWalletFromAdmin(ChargeWalletFromAdminViewModel charge);

        bool AddWallet(Domain.Models.Wallet.Wallet wallet);

        bool UpdateUserWalletBalance(int userId);
    }
}
