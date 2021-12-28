using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Wallet;

namespace KetabAbee.Application.Services.Wallet
{
    public class WalletService : IWalletService
    {

        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public IEnumerable<Domain.Models.Wallet.Wallet> GetWalletsByUserId(int userId)
        {
            return _walletRepository.GetWallets().Where(w => w.UserId == userId);
        }

        public long BalanceUserWallet(int userId)
        {
            var deposit = _walletRepository.GetWallets()
                .Where(w => w.UserId == userId && w.WalletType == WalletType.Deposit)
                .Select(w => w.Amount);

            var withdraw = _walletRepository.GetWallets()
                .Where(w => w.UserId == userId && w.WalletType == WalletType.Withdraw)
                .Select(w => w.Amount);

            return deposit.Sum() - withdraw.Sum();
        }

        

    }
}
