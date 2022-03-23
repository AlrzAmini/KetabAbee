using System;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Banner;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Data.Repository
{
    public class BannerRepository : IBannerRepository
    {
        private readonly KetabAbeeDBContext _context;

        public BannerRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddBanner(Banner banner)
        {
            try
            {
                await _context.Banners.AddAsync(banner);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteBanner(Banner banner)
        {
            try
            {
                banner.IsDelete = true;
                return await UpdateBanner(banner);
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Banner> GetBanners()
        {
            return _context.Banners;
        }

        public async Task<Banner> GetBannerById(int bannerId)
        {
            return await _context.Banners.FindAsync(bannerId);
        }

        public async Task<bool> UpdateBanner(Banner banner)
        {
            try
            {
                _context.Banners.Update(banner);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> GetActiveBannersCountByLocation(BannerLocation bannerLocation)
        {
            return await _context.Banners.CountAsync(b => b.BannerLocation == bannerLocation);
        }

        public async Task<List<Banner>> GetAllIsActiveBanners()
        {
            return await _context.Banners.AsQueryable()
                .Where(b => b.IsActive)
                .ToListAsync();
        }

        public async Task<List<Banner>> GetAllIsActiveHeadBanners()
        {
            return await _context.Banners.AsQueryable()
                .Where(b => b.IsActive)
                .Where(b =>
                    b.BannerLocation == BannerLocation.LongHead ||
                    b.BannerLocation == BannerLocation.ShortHead ||
                    b.BannerLocation == BannerLocation.Slider)
                .ToListAsync();
        }

        public async Task<IQueryable<Banner>> GetAllActiveBannersByLocation(BannerLocation bannerLocation)
        {
            return await Task.FromResult(_context.Banners.AsQueryable()
                .Where(b => b.IsActive && b.BannerLocation == bannerLocation));
        }
    }
}