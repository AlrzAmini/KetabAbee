using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Application.DTOs.Admin.Products.Options
{
    public class ShowBookInfoViewModel
    {
        public int BookId { get; set; }

        public string PublisherName { get; set; }

        public string GroupName { get; set; }

        public string SubGroupName { get; set; }

        public string SubGroup2Name { get; set; }

        public AgeRange AgeRange { get; set; }

        public CoverType CoverType { get; set; }

        public string PageDescription { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Writer { get; set; }

        public int PagesCount { get; set; }

        public string ImageName { get; set; }

        public string Abstract { get; set; }

        public int? Inventory { get; set; }
    }

    public enum DeleteProductCommentsResult
    {
        Success,
        NotHaveComment,
        Error
    }
}
