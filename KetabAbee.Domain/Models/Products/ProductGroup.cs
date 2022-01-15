using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products
{
    public class ProductGroup
    {
        [Key]
        public int GroupId { get; set; }

        public int BooksId { get; set; }

        #region Props

        [DisplayName("عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string GroupTitle { get; set; }

        public bool IsDelete { get; set; }

        [DisplayName("گروه اصلی")]
        public int? ParentId { get; set; }

        #endregion

        #region Relations

        [ForeignKey("ParentId")]
        public ICollection<ProductGroup> ProductGroups { get; set; }

        [InverseProperty("Group")]
        public ICollection<Book> Books { get; set; }

        [InverseProperty("SubGroup")]
        public ICollection<Book> Subs1 { get; set; }

        [InverseProperty("SubGroup2")]
        public ICollection<Book> Subs2 { get; set; }

        #endregion
    }
}
