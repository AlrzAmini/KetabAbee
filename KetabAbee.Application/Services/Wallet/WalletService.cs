using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Wallet;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.DTOs.Wallet;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Wallet;

namespace KetabAbee.Application.Services.Wallet
{
    public class WalletService : IWalletService
    {

        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;

        public WalletService(IWalletRepository walletRepository, IUserRepository userRepository)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
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

        public bool ChargeWalletByUserId(int userId, ChargeWalletViewModel charge)
        {
            try
            {
                if (charge.Amount <= 0)
                {
                    return false;
                }

                var wallet = new Domain.Models.Wallet.Wallet
                {
                    Amount = charge.Amount,
                    Behalf = charge.Behalf.Sanitizer(),
                    CreateDate = DateTime.Now,
                    UserId = userId,
                    IsPay = charge.IsPay,
                    WalletType = WalletType.Deposit
                };

                AddWallet(wallet);
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
                10,
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

                AddWallet(wallet);
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

                AddWallet(wallet);
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
            if (UpdateUserWalletBalance(wallet.UserId)) return walletId != 0;

            _walletRepository.RemoveWallet(walletId);
            return false;

        }

        public bool UpdateUserWalletBalance(int userId)
        {
            var user = _userRepository.GetUserById(userId);

            var userBalance = user.WalletBalance;
            var newBalance = BalanceUserWallet(userId);

            if (userBalance == newBalance) return false;

            user.WalletBalance = newBalance;
            return _userRepository.UpdateUser(user);
        }
    }
}
