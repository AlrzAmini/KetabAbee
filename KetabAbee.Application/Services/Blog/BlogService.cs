using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs.Admin.Blog;
using KetabAbee.Application.DTOs.Blog;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Generators;
using KetabAbee.Application.Interfaces.Blog;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Application.Services.Blog
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public bool CreateBlog(CreateBlogViewModel blog)
        {
            try
            {
                var newBlog = new Domain.Models.Blog.Blog
                {
                    BlogBody = blog.BlogBody,
                    BlogTitle = blog.BlogTitle,
                    CreateDate = DateTime.Now,
                    PageDescription = blog.PageDescription,
                    Tags = blog.Tags,
                    UserId = blog.UserId
                };

                if (blog.Image == null || !blog.Image.IsImage())
                {
                    newBlog.ImageName = "Default.jpg";
                    return _blogRepository.AddBlog(newBlog);
                }

                AddImageToBlog(blog, newBlog);

                return _blogRepository.AddBlog(newBlog);
            }
            catch
            {
                return false;
            }

        }

        private static void AddImageToBlog(CreateBlogViewModel blog, Domain.Models.Blog.Blog newBlog)
        {
            // generate new image path and name
            newBlog.ImageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(blog.Image.FileName);

            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog/image", newBlog.ImageName);

            // save file in file
            using (var stream = new FileStream(imgPath, FileMode.Create))
            {
                blog.Image.CopyTo(stream);
            }

            var imgResizer = new ImageConvertor();
            var imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog/thumb", newBlog.ImageName);
            imgResizer.Image_resize(imgPath, imgThumbPath, 400);
        }

        public FilterBlogsViewModel FilterBlogs(FilterBlogsViewModel filter)
        {
            var result = GetBlogsForShowInAdmin().AsQueryable();

            #region filter by title

            if (!string.IsNullOrEmpty(filter.BlogTitle))
            {
                result = result.Where(r => r.BlogTitle.Contains(filter.BlogTitle));
            }

            #endregion

            #region filter by writer

            if (!string.IsNullOrEmpty(filter.Writer))
            {
                result = result.Where(r => r.Writer.Contains(filter.BlogTitle));
            }

            #endregion

            #region paging

            //paging
            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var blogs = result.Paging(pager).ToList();

            return filter.SetPaging(pager).SetBlogs(blogs);

            #endregion
        }

        public IEnumerable<ShowBlogInListViewModel> GetBlogsForShowInAdmin()
        {
            return _blogRepository.GetBlogs()
                .Select(b => new ShowBlogInListViewModel
                {
                    ImageName = b.ImageName,
                    CreateDate = b.CreateDate,
                    BlogId = b.BlogId,
                    BlogTitle = b.BlogTitle,
                    Writer = b.User.UserName
                });
        }

        public IEnumerable<BlogInCardViewModel> GetBlogsCardInfo()
        {
            return _blogRepository.GetBlogs().Select(b => new BlogInCardViewModel
            {
                ImageName = b.ImageName,
                CreateDate = b.CreateDate,
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                Writer = b.User.UserName,
                BlogDescription = b.PageDescription.TruncateLongString(50),
                WriterAvatarName = b.User.AvatarName,
                Tags = b.Tags
            });
        }

        public bool DeleteBlog(int blogId)
        {
            var blog = GetBlogById(blogId);

            #region Delete image

            if (blog.ImageName != null)
            {
                if (blog.ImageName != "Default.jpg")
                {
                    DeleteBlogImage(blog);
                }
            }

            blog.ImageName = "Default.jpg";

            #endregion

            blog.IsDelete = true;
            return _blogRepository.UpdateBlog(blog);
        }

        private static void DeleteBlogImage(Domain.Models.Blog.Blog blog)
        {
            var imgDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog/image",
                blog.ImageName);
            if (File.Exists(imgDeletePath))
            {
                File.Delete(imgDeletePath);
            }

            var thumbDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog/thumb",
                blog.ImageName);
            if (File.Exists(thumbDeletePath))
            {
                File.Delete(thumbDeletePath);
            }
        }

        public bool UpdateBlog(int blogId)
        {
            return _blogRepository.UpdateBlog(GetBlogById(blogId));
        }

        public Domain.Models.Blog.Blog GetBlogById(int blogId)
        {
            return _blogRepository.GetBlogById(blogId);
        }

        public EditBlogViewModel GetBlogForEdit(int blogId)
        {
            return _blogRepository.GetBlogs()
                .Where(b => b.BlogId == blogId)
                .Select(b => new EditBlogViewModel
                {
                    PageDescription = b.PageDescription,
                    ImageName = b.ImageName,
                    Writer = b.User.UserName,
                    UserId = b.UserId,
                    BlogBody = b.BlogBody,
                    BlogTitle = b.BlogTitle,
                    Tags = b.Tags,
                    BlogId = b.BlogId
                }).Single();
        }

        public BlogEditResult EditBlog(EditBlogViewModel blog)
        {
            try
            {
                var newBlog = GetBlogById(blog.BlogId);

                if (newBlog == null)
                {
                    return BlogEditResult.NotFound;
                }

                #region update photo

                EditBlogImage(blog);

                #endregion

                newBlog.BlogBody = blog.BlogBody;
                newBlog.BlogTitle = blog.BlogTitle;
                newBlog.Tags = blog.Tags;
                newBlog.UserId = blog.UserId;
                newBlog.ImageName = blog.ImageName;
                newBlog.PageDescription = blog.PageDescription;

                return _blogRepository.UpdateBlog(newBlog) ? BlogEditResult.Success : BlogEditResult.Failed;
            }
            catch
            {
                return BlogEditResult.UnHandledError;
            }
        }

        private static void EditBlogImage(EditBlogViewModel blog)
        {
            if (blog.Image != null && blog.Image.IsImage())
            {
                string imgPath;
                string imgThumbPath;
                if (blog.ImageName != "Default.jpg")
                {
                    // get old avatar path
                    imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog/image", blog.ImageName);

                    // delete old avatar
                    if (File.Exists(imgPath))
                    {
                        File.Delete(imgPath);
                    }

                    // get old avatar thumb path
                    imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog/thumb", blog.ImageName);

                    // delete old thumb avatar
                    if (File.Exists(imgThumbPath))
                    {
                        File.Delete(imgThumbPath);
                    }
                }

                // generate new image path and name
                blog.ImageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(blog.Image.FileName);

                imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog/image", blog.ImageName);

                // save file in file
                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    blog.Image.CopyTo(stream);
                }

                var imgResizer = new ImageConvertor();
                imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/blog/thumb", blog.ImageName);
                imgResizer.Image_resize(imgPath, imgThumbPath, 400);
            }
        }

        public FilterBlogsIndexViewModel FilterBlogsInIndex(FilterBlogsIndexViewModel filter)
        {
            var result = GetBlogsCardInfo().AsQueryable();

            #region filter by title

            if (!string.IsNullOrEmpty(filter.BlogTitle))
            {
                result = result.Where(r => r.BlogTitle.Contains(filter.BlogTitle));
            }

            #endregion

            #region filter by tags

            if (!string.IsNullOrEmpty(filter.Tag))
            {
                result = result.Where(r => r.Tags.Contains(filter.Tag));
            }

            #endregion

            #region paging

            //paging
            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var blogs = result.Paging(pager).ToList();

            return filter.SetPaging(pager).SetBlogs(blogs);

            #endregion
        }

        public ShowBlogInfoViewModel GetBlogForShowInBlogInfo(int blogId)
        {
            return _blogRepository.GetBlogs()
                .Where(b => b.BlogId == blogId)
                .Select(b => new ShowBlogInfoViewModel
                {
                    UserId = b.UserId,
                    CreateDate = b.CreateDate,
                    UserName = b.User.UserName,
                    PageDescription = b.PageDescription,
                    BlogTitle = b.BlogTitle,
                    BlogBody = b.BlogBody,
                    BlogId = b.BlogId,
                    Tags = b.Tags,
                    UserImageName = b.User.AvatarName,
                    WriterBlogs = GetWriterBlogs(b.UserId),
                    ImageName = b.ImageName
                }).Single();
        }

        public List<BlogInCardViewModel> GetWriterBlogs(int userId)
        {
            return _blogRepository.GetWriterBlogs(userId)
                .Select(b => new BlogInCardViewModel
                {
                    ImageName = b.ImageName,
                    Writer = b.User.UserName,
                    CreateDate = b.CreateDate,
                    BlogDescription = b.PageDescription.TruncateLongString(50),
                    BlogId = b.BlogId,
                    BlogTitle = b.BlogTitle,
                    Tags = b.Tags,
                    WriterAvatarName = b.User.AvatarName
                }).ToList();
        }
    }
}
