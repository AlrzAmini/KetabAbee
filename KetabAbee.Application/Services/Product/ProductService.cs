using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.Generators;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Application.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public bool AddGroup(ProductGroup group)
        {
            return _productRepository.AddGroup(group);
        }

        public IEnumerable<ProductGroup> GetGroups()
        {
            return _productRepository.GetGroups();
        }

        public IEnumerable<ProductGroup> GetGroupsForAdmin()
        {
            return _productRepository.GetGroupsForAdmin();
        }

        public bool UpdateGroup(ProductGroup group)
        {
            return _productRepository.UpdateGroup(group);
        }

        public ProductGroup GetGroupById(int groupId)
        {
            return _productRepository.GetGroupById(groupId);
        }

        public bool DeleteGroupById(int groupId)
        {
            var group = GetGroupById(groupId);
            group.IsDelete = true;
            return UpdateGroup(group);
        }

        public IEnumerable<Publisher> GetPublishers()
        {
            return _productRepository.GetPublishers();
        }

        public List<SelectListItem> GetGroupsForAddBook()
        {
            return _productRepository.GetGroups().Where(g => g.ParentId == null)
                .Select(g => new SelectListItem
                {
                    Value = g.GroupId.ToString(),
                    Text = g.GroupTitle
                }).ToList();
        }

        public List<SelectListItem> GetSubGroupsForAddBook(int groupId)
        {
            return _productRepository.GetGroups().Where(g => g.ParentId == groupId)
                .Select(g => new SelectListItem
                {
                    Value = g.GroupId.ToString(),
                    Text = g.GroupTitle
                }).ToList();
        }

        public bool AddBook(Book book, IFormFile imgFile)
        {
            
            if (imgFile == null || !imgFile.IsImage()) return _productRepository.AddBook(book);

            // add image
            // generate name
            book.ImageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(imgFile.FileName);

            // image path
            string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Book/image", book.ImageName);

            // save file in file
            using (var stream = new FileStream(imgPath, FileMode.Create))
            {
                imgFile.CopyTo(stream);
            }

            // thumb
            var imgResizer = new ImageConvertor();
            string imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Book/thumb", book.ImageName);
            imgResizer.Image_resize(imgPath, imgThumbPath, 400);

            return _productRepository.AddBook(book);
        }

        public IEnumerable<Book> GetBooksForAdmin()
        {
            return _productRepository.GetBooksForAdmin();
        }
    }
}
