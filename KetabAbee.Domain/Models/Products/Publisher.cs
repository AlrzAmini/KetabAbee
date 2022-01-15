using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products
{
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }

        public int BooksId { get; set; }

        #region Props

        [DisplayName("ناشر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string PublisherName { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region Relations

        public ICollection<Book> Books { get; set; }


        #endregion

    }
}
