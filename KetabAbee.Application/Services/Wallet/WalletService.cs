using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Wallet;
using KetabAbee.Application.DTOs.Paging;
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

        public bool ChargeWalletByUserId(int userId, ChargeWalletViewModel charge, bool isPay = true)
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

                _walletRepository.AddWallet(wallet);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Domain.Models.Wallet.Wallet GetWalletById(int walletId)
        {
            return _walletRepository.GetWalletById(walletId);
        }

        public bool UpdateWallet(Domain.Models.Wallet.Wallet wallet)
        {
            return _walletRepository.UpdateWallet(wallet);
        }

        public WalletsWithPagingViewModel GetWalletsWithPagingByUserId(WalletsWithPagingViewModel walletsWithPagingViewModel)
        {
            var result = _walletRepository.GetWalletsByUserId(walletsWithPagingViewModel.UserId).AsQueryable();

            //paging
            var pager = Pager.Build(walletsWithPagingViewModel.PageNum,
                result.Count(),
                walletsWithPagingViewModel.Take,
                walletsWithPagingViewModel.PageCountAfterAndBefor);
            var wallets = result.Paging(pager).ToList();

            return walletsWithPagingViewModel.SetPaging(pager).SetWallets(wallets);
        }

        public bool ChargeWalletFromAdmin(ChargeWalletFromAdminViewModel charge)
        {
            try
            {
                var wallet = new Domain.Models.Wallet.Wallet
                {
                    Amount = charge.Amount,
                    Behalf = charge.Behalf,
                    CreateDate = DateTime.Now,
                    UserId = charge.UserId,
                    IsPay = true,
                    WalletType = WalletType.Deposit
                };

                _walletRepository.AddWallet(wallet);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WithDrawWalletFromAdmin(ChargeWalletFromAdminViewModel charge)
        {
            try
            {
                var wallet = new Domain.Models.Wallet.Wallet
                {
                    Amount = charge.Amount,
                    Behalf = charge.Behalf,
                    CreateDate = DateTime.Now,
                    UserId = charge.UserId,
                    IsPay = true,
                    WalletType = WalletType.Withdraw
                };

                _walletRepository.AddWallet(wallet);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddWallet(Domain.Models.Wallet.Wallet wallet)
        {
            var walletId = _walletRepository.AddWallet(wallet);
            return walletId != 0;
        }
    }
}
