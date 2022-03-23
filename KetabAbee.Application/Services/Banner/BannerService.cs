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
using KetabAbee.Application.DTOs.Banner;

namespace KetabAbee.Application.Services.Banner
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<CreateBannerResult> CreateBanner(CreateBannerViewModel banner)
        {
            try
            {
                var newBanner = new Domain.Models.Banner.Banner
                {
                    IsActive = banner.IsActive,
                    BannerLocation = banner.BannerLocation,
                    Link = banner.Link,
                    StartDate = banner.StartDate.ToMiladiDateTime(),
                    ImageName = "Defualt.jpg",
                    Alt = banner.Alt
                };
                if (banner.EndDate != null)
                {
                    newBanner.EndDate = banner.EndDate.ToMiladiDateTime();
                }

                if (await CheckBannerLimitations(newBanner.BannerLocation))
                {
                    if (banner.Image == null)
                    {
                        if (await _bannerRepository.AddBanner(newBanner))
                        {
                            return CreateBannerResult.Success;
                        }

                        return CreateBannerResult.Error;
                    }

                    newBanner.ImageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(banner.Image.FileName);
                    var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + newBanner.GetBannerAddress());

                    // save image in file
                    await using (var stream = new FileStream(imgPath, FileMode.Create))
                    {
                        await banner.Image.CopyToAsync(stream);
                    }

                    if (await _bannerRepository.AddBanner(newBanner))
                    {
                        return CreateBannerResult.Success;
                    }

                    return CreateBannerResult.Error;
                }

                return CreateBannerResult.OutOfRangeBanner;
            }
            catch
            {
                return CreateBannerResult.Error;
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

        private static bool DeleteBannerImage(Domain.Models.Banner.Banner banner)
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

        public async Task<bool> CheckBannerLimitations(BannerLocation bannerLocation)
        {
            var currentBannersCount = await _bannerRepository.GetActiveBannersCountByLocation(bannerLocation);

            switch (bannerLocation)
            {
                case BannerLocation.Slider:
                    return true;

                case BannerLocation.LongHead:
                    // long head banner limitation is 1
                    return currentBannersCount < 1;

                case BannerLocation.ShortHead:
                    // short head banners limitation is 2
                    return currentBannersCount < 2;

                case BannerLocation.MainAndProfile:
                    // main & profile banner limitation is 4
                    return currentBannersCount < 4;

                case BannerLocation.Main:
                    // main & profile banner limitation is 2
                    return currentBannersCount < 2;

                default:
                    return false;
            }
        }

        public async Task<HeadBannersViewModel> GetHeadBanners()
        {
            var longHeadBanners = await _bannerRepository.GetAllIsActiveHeadBanners();
            if (longHeadBanners == null)
            {
                return new HeadBannersViewModel();
            }

            var longHeadBannersInfo = longHeadBanners
                .Select(b => new BannerInfoViewModel
                {
                    Link = b.Link,
                    Alt = b.Alt,
                    BannerId = b.BannerId,
                    BannerLocation = b.BannerLocation,
                    ImageName = b.ImageName,
                    ImageSavePath = PathExtensions.BannerFullAddress(b.ImageName)
                }).AsQueryable();

            var headBanners = new HeadBannersViewModel();

            var longHead = longHeadBannersInfo.FirstOrDefault(b => b.BannerLocation == BannerLocation.LongHead);
            if (longHead != null)
            {
                headBanners.LongHead = longHead;
            }

            var shortHeads = longHeadBannersInfo.Where(b => b.BannerLocation == BannerLocation.ShortHead);
            if (shortHeads.Any())
            {
                var firstShortHead = shortHeads.ElementAt(0);
                if (firstShortHead != null)
                {
                    headBanners.FirstShortHead = firstShortHead;
                }
            }
            if (shortHeads.Count() > 1)
            {
                var secondShortHead = shortHeads.ElementAt(1);
                if (secondShortHead != null)
                {
                    headBanners.SecondShortHead = secondShortHead;
                }
            }

            var sliders = longHeadBannersInfo.Where(b => b.BannerLocation == BannerLocation.Slider).ToList();
            if (sliders.Any())
            {
                headBanners.Sliders = sliders;
            }

            return headBanners;
        }

        public async Task<bool> ActiveBanner(int bannerId)
        {
            var banner = await _bannerRepository.GetBannerById(bannerId);
            if (banner == null)
            {
                return false;
            }

            banner.IsActive = true;
            return await _bannerRepository.UpdateBanner(banner);
        }

        public async Task<bool> DeActiveBanner(int bannerId)
        {
            var banner = await _bannerRepository.GetBannerById(bannerId);
            if (banner == null)
            {
                return false;
            }

            banner.IsActive = false;
            return await _bannerRepository.UpdateBanner(banner);
        }
    }
}