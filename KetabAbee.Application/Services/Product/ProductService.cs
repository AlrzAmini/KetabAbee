﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Products;
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
    }
}
