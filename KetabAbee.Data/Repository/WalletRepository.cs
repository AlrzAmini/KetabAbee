using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Wallet;

namespace KetabAbee.Data.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly KetabAbeeDBContext _context;

        public WalletRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Wallet> GetWallets()
        {
            return _context.Wallets;
        }

    }
}
