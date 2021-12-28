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

        public bool AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Wallet> GetWallets()
        {
            return _context.Wallets;
        }

        public bool UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
            return true;
        }
    }
}
