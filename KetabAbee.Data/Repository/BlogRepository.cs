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
            return _context.Blogs.Include(b=>b.User).OrderByDescending(b=>b.CreateDate);
        }
    }
}
