using System;
using System.IO;
using System.Linq;
using KetabAbee.Application.DTOs.Admin;
using KetabAbee.Application.Interfaces.Banner;
using KetabAbee.Domain.Interfaces;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin.Banner;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Generators;
using KetabAbee.Domain.Models.Banner;

namespace KetabAbee.Application.Services.Banner
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<bool> CreateBanner(CreateBannerViewModel banner)
        {
            try
            {
                var newBanner = new Domain.Models.Banner.Banner
                {
                    IsActive = banner.IsActive,
                    BannerLocation = banner.BannerLocation,
                    Link = banner.Link,
                    StartDate = banner.StartDate.ToMiladiDateTime(),
                    ImageName = "Defualt.jpg"
                };
                if (banner.EndDate != null)
                {
                    newBanner.EndDate = banner.EndDate.ToMiladiDateTime();
                }
                if (banner.Image == null) return await _bannerRepository.AddBanner(newBanner);

                newBanner.ImageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(banner.Image.FileName);
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + newBanner.GetBannerAddress());

                // save image in file
                await using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    await banner.Image.CopyToAsync(stream);
                }

                return await _bannerRepository.AddBanner(newBanner);
            }
            catch
            {
                return false;
            }
        }

        public async Task<DeleteBannerResult> DeleteBannerById(int bannerId)
        {
            var banner = await _bannerRepository.GetBannerById(bannerId);
            if (banner == null)
            {
                return DeleteBannerResult.NotFounded;
            }

            // delete image
            if (banner.ImageName != null)
            {
                if (banner.ImageName != "Default.jpg")
                {
                    if (!DeleteBannerImage(banner))
                    {
                        return DeleteBannerResult.ImageError;
                    }
                }
            }

            if (await _bannerRepository.DeleteBanner(banner))
            {
                return DeleteBannerResult.Success;
            }

            return DeleteBannerResult.Error;
        }

        private bool DeleteBannerImage(Domain.Models.Banner.Banner banner)
        {
            try
            {
                var imgDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + banner.GetBannerAddress());
                if (File.Exists(imgDeletePath))
                {
                    File.Delete(imgDeletePath);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public PagingBannersViewModel GetBannersWithPaging(PagingBannersViewModel model)
        {
            var result = _bannerRepository.GetBanners().AsQueryable();

            //paging
            var pager = Pager.Build(model.PageNum, result.Count(), model.Take, model.PageCountAfterAndBefor);
            var banners = result.Paging(pager).ToList();

            return model.SetPaging(pager).SetBanners(banners);
        }
    }
}