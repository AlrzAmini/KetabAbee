using System;
using System.Collections;
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
using System.Collections.Generic;

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
                    Alt = banner.Alt,
                    EndDate = banner.EndDate.ToMiladiDateTime()
                };

                if (!banner.IsActive)
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

                if (await CheckBannerLimitations(newBanner.BannerLocation, null))
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

        public async Task<bool> CheckBannerLimitations(BannerLocation bannerLocation, int? increaseValue)
        {
            var currentBannersCount = await _bannerRepository.GetActiveBannersCountByLocation(bannerLocation);
            if (increaseValue != null)
            {
                currentBannersCount++;
            }

            switch (bannerLocation)
            {
                case BannerLocation.Slider:
                    // no limitation
                    return true;

                case BannerLocation.LongHead:
                    // long head banner limitation is 1
                    return currentBannersCount < 1;

                case BannerLocation.ShortHead:
                    // short head banners limitation is 2
                    return currentBannersCount < 2;

                case BannerLocation.MainAndProfile:
                    // no limitation
                    return true;

                case BannerLocation.Main:
                    // no limitation
                    return true;

                default:
                    return false;
            }
        }

        public async Task<HeadBannersViewModel> GetHeadBanners()
        {
            var longHeadBanners = await _bannerRepository.GetAllIsActiveHeadBanners();
            if (longHeadBanners == null)
            {
                return null;
            }

            var filteredBanners = longHeadBanners.Where(b => b.StartDate < DateTime.Now && b.EndDate > DateTime.Now);

            filteredBanners = filteredBanners.Distinct();

            var longHeadBannersInfo = filteredBanners
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

        public async Task<ActiveBannerResult> ActiveBanner(int bannerId)
        {
            try
            {
                var banner = await _bannerRepository.GetBannerById(bannerId);
                if (banner == null)
                {
                    return ActiveBannerResult.NotFounded;
                }

                if (await CheckBannerLimitations(banner.BannerLocation, 1))
                {
                    banner.IsActive = true;
                    if (await _bannerRepository.UpdateBanner(banner))
                    {
                        return ActiveBannerResult.Success;
                    }
                }

                return ActiveBannerResult.OutOfRange;
            }
            catch
            {
                return ActiveBannerResult.Error;
            }
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

        public async Task<EditBannerViewModel> GetBannerForEdit(int bannerId)
        {
            var banner = await _bannerRepository.GetBannerById(bannerId);
            if (banner == null)
            {
                return null;
            }

            return new EditBannerViewModel
            {
                ImageName = banner.ImageName,
                BannerLocation = banner.BannerLocation,
                IsActive = banner.IsActive,
                Alt = banner.Alt,
                BannerId = banner.BannerId,
                EndDate = banner.EndDate.ToShamsi(),
                ImageSavePath = PathExtensions.BannerFullAddress(banner.ImageName),
                Link = banner.Link,
                StartDate = banner.StartDate.ToShamsi()
            };
        }

        public async Task<EditBannerResult> EditBanner(EditBannerViewModel banner)
        {
            var newBanner = await _bannerRepository.GetBannerById(banner.BannerId);
            if (newBanner == null)
            {
                return EditBannerResult.NotFounded;
            }

            if (!newBanner.IsActive)
            {
                if (banner.IsActive)
                {
                    if (!await CheckBannerLimitations(banner.BannerLocation, 1))
                    {
                        return EditBannerResult.OutOfRange;
                    }
                }
            }

            newBanner.IsActive = banner.IsActive;
            newBanner.Alt = banner.Alt;
            newBanner.BannerLocation = banner.BannerLocation;
            newBanner.EndDate = banner.EndDate.ToMiladiDateTime();
            newBanner.StartDate = banner.StartDate.ToMiladiDateTime();
            newBanner.Link = banner.Link;
            newBanner.BannerId = banner.BannerId;

            try
            {
                await EditBannerImage(banner, newBanner);
            }
            catch
            {
                return EditBannerResult.ImageError;
            }

            if (await _bannerRepository.UpdateBanner(newBanner))
            {
                return EditBannerResult.Success;
            }

            return EditBannerResult.Error;
        }

        public async System.Threading.Tasks.Task EditBannerImage(EditBannerViewModel banner, Domain.Models.Banner.Banner newBanner)
        {
            if (banner.Image != null)
            {
                if (banner.ImageName != "Default.jpg")
                {
                    // delete current image
                    DeleteBannerImage(newBanner);
                }

                // add new image

                // generate new image path and name
                newBanner.ImageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(banner.Image.FileName);

                var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + newBanner.GetBannerAddress());

                // save image in file
                await using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    await banner.Image.CopyToAsync(stream);
                }
            }
        }

        public async Task<List<LittleBannerInfoViewModel>> GetMainAndProfileBannersForShow()
        {
            var banners = await _bannerRepository.GetAllActiveBannersByLocation(BannerLocation.MainAndProfile);

            var filteredBanners = banners.Where(b => b.StartDate < DateTime.Now && b.EndDate > DateTime.Now);

            filteredBanners = filteredBanners.Distinct();

            return filteredBanners.Select(b => new LittleBannerInfoViewModel
            {
                Alt = b.Alt,
                ImageSavePath = PathExtensions.BannerFullAddress(b.ImageName),
                Link = b.Link
            }).ToList();
        }

        public async Task<List<LittleBannerInfoViewModel>> GetMainBannersForShow()
        {
            var banners = await _bannerRepository.GetAllActiveBannersByLocation(BannerLocation.Main);

            var filteredBanners = banners.Where(b => b.StartDate < DateTime.Now && b.EndDate > DateTime.Now);

            filteredBanners = filteredBanners.Distinct();

            return filteredBanners.Select(b => new LittleBannerInfoViewModel
            {
                Alt = b.Alt,
                ImageSavePath = PathExtensions.BannerFullAddress(b.ImageName),
                Link = b.Link
            }).ToList();
        }
    }
}