using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Wallet;
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
                .Where(w => w.UserId == userId && w.WalletType == WalletType.Deposit && w.IsPay)
                .Select(w => w.Amount);

            var withdraw = _walletRepository.GetWallets()
                .Where(w => w.UserId == userId && w.WalletType == WalletType.Withdraw && w.IsPay)
                .Select(w => w.Amount);

            return deposit.Sum() - withdraw.Sum();
        }

        public int ChargeWalletByUserId(int userId, ChargeWalletViewModel charge, bool isPay = false)
        {
            try
            {
                var wallet = new Domain.Models.Wallet.Wallet
                {
                    Amount = charge.Amount,
                    Behalf = charge.Behalf,
                    CreateDate = DateTime.Now,
                    UserId = userId,
                    IsPay = isPay,
                    WalletType = WalletType.Deposit
                };

                return _walletRepository.AddWallet(wallet);
            }
            catch
            {
                return 0;
            }
        }

        public Domain.Models.Wallet.Wallet GetWalletById(int walletId)
        {
           return _walletRepository.GetWallets().SingleOrDefault(w => w.WalletId == walletId);
        }

        public bool UpdateWallet(Domain.Models.Wallet.Wallet wallet)
        {
            return _walletRepository.UpdateWallet(wallet);
        }
    }
}
