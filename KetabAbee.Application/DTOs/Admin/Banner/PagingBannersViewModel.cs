using System.Collections.Generic;
using KetabAbee.Application.DTOs.Paging;

namespace KetabAbee.Application.DTOs.Admin.Banner
{
    public class PagingBannersViewModel : BasePaging
    {
        public List<Domain.Models.Banner.Banner> Banners { get; set; }

        #region Methods

        public PagingBannersViewModel SetBanners(List<Domain.Models.Banner.Banner> banners)
        {
            Banners = banners;
            return this;
        }

        public PagingBannersViewModel SetPaging(BasePaging paging)
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