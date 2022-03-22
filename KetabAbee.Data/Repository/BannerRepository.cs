using System;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Banner;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using System.Collections.Generic;
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
    }
}