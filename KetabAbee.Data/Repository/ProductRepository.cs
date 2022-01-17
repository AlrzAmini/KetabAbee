﻿using System;
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
    public class ProductRepository : IProductRepository
    {
        private readonly KetabAbeeDBContext _context;

        public ProductRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public bool AddGroup(ProductGroup group)
        {
            _context.ProductGroups.Add(group);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<ProductGroup> GetGroups()
        {
            return _context.ProductGroups;
        }

        public IEnumerable<ProductGroup> GetGroupsForAdmin()
        {
            return _context.ProductGroups
                .Include(g => g.ProductGroups);
        }

        public bool UpdateGroup(ProductGroup group)
        {
            _context.ProductGroups.Update(group);
            _context.SaveChanges();
            return true;
        }

        public ProductGroup GetGroupById(int groupId)
        {
            return _context.ProductGroups.Find(groupId);
        }

        public IEnumerable<Publisher> GetPublishers()
        {
            return _context.Publishers;
        }

        public bool AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Book> GetBooksForAdmin()
        {
            return _context.Books
                .Include(b => b.Publisher)
                .Include(b => b.Group)
                .Include(b=>b.SubGroup)
                .Include(b=>b.SubGroup2);
        }
    }
}
