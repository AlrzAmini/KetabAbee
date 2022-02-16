using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Paging;

namespace KetabAbee.Application.DTOs.Wallet
{
    public class WalletsWithPagingViewModel : BasePaging
    {
        public int UserId { get; set; }

        public ChargeWalletViewModel Charge { get; set; }

        public List<Domain.Models.Wallet.Wallet> Wallets { get; set; }

        #region Methods

        public WalletsWithPagingViewModel SetWallets(List<Domain.Models.Wallet.Wallet> wallets)
        {
            Wallets = wallets;
            return this;
        }

        public WalletsWithPagingViewModel SetPaging(BasePaging paging)
        {
            PageNum = paging.PageNum;
            TotalEntitiesCount = paging.TotalEntitiesCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            PageCountAfterAndBefor = paging.PageCountAfterAndBefor;
            Take = paging.Take;
            Skip = paging.Skip;
            TotalPages = paging.TotalPages;

            return this;
        }

        #endregion
    }
}
