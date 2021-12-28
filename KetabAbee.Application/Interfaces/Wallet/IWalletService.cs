﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Wallet;

namespace KetabAbee.Application.Interfaces.Wallet
{
    public interface IWalletService
    {
        IEnumerable<Domain.Models.Wallet.Wallet> GetWalletsByUserId(int userId);

        long BalanceUserWallet(int userId);
    }
}
