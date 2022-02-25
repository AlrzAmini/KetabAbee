using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Products.Book.Publishers
{
    public class CreatePublisherViewModel
    {
        [DisplayName("نام ناشر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string PublisherName { get; set; }
    }

    public class EditPublisherViewModel
    {
        public int PublisherId { get; set; }

        public string PublisherName { get; set; }
    }

    public enum EditPublisherResult
    {
        Success,
        Error,
        RepetitiousName
    }
    public enum CreatePublisherResult
    {
        Success,
        Error,
        RepetitiousName
    }
}
