using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Data.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly KetabAbeeDBContext _context;

        public DiscountRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public bool AddDiscount(ProductDiscount discount)
        {
            try
            {
                _context.ProductDiscounts.Add(discount);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<ProductDiscount> GetDiscounts()
        {
            return _context.ProductDiscounts.Include(d => d.Product).OrderByDescending(d=>d.DiscountId);
        }
    }
}
