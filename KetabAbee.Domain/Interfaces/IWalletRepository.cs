using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Wallet;

namespace KetabAbee.Domain.Interfaces
{
    public interface IWalletRepository
    {
        IEnumerable<Wallet> GetWallets();
    }
}
