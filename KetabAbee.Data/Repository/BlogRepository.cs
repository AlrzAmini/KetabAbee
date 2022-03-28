using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Blog;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Data.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly KetabAbeeDBContext _context;

        public BlogRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }


        public IEnumerable<Blog> GetBlogs()
        {
            return _context.Blogs.Include(b => b.User).OrderByDescending(b => b.CreateDate);
        }

        public bool AddBlog(Blog blog)
        {
            try
            {
                _context.Blogs.Add(blog);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteBlog(Blog blog)
        {
            try
            {
                blog.IsDelete = true;
                return await UpdateBlog(blog);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateBlog(Blog blog)
        {
            try
            {
                _context.Blogs.Update(blog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Blog> GetWriterBlogs(int userId)
        {
            return _context.Blogs.Include(b => b.User)
                .Where(b => b.UserId == userId)
                .Take(6)
                .ToList();
        }

        public async Task<bool> AddBlogView(BlogView view)
        {
            try
            {
                await _context.BlogViews.AddAsync(view);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsBlogViewExist(int blogId, string userIp)
        {
            return await _context.BlogViews.AnyAsync(v => v.BlogId == blogId && v.UserIp == userIp);
        }

        public async Task<bool> IsBlogViewExist(int blogId, int userId)
        {
            return await _context.BlogViews.AnyAsync(v => v.BlogId == blogId && v.UserId == userId);
        }

        public async Task<Blog> GetBlogById(int blogId)
        {
            return await _context.Blogs.FindAsync(blogId);
        }
    }
}
