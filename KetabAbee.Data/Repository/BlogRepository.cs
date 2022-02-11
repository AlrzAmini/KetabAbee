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

        public bool DeleteBlog(Blog blog)
        {
            try
            {
                blog.IsDelete = true;
                return UpdateBlog(blog);
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateBlog(Blog blog)
        {
            try
            {
                _context.Blogs.Update(blog);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Blog GetBlogById(int blogId)
        {
            return _context.Blogs.Find(blogId);
        }

        public List<Blog> GetWriterBlogs(int userId)
        {
            return _context.Blogs.Include(b=>b.User)
                .Where(b => b.UserId == userId)
                .Take(6)
                .ToList();
        }
    }
}
