using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Products;

namespace KetabAbee.Domain.Models.User
{
    public class UserBook
    {
        [Key]
        public int UserProductId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }



        #region relations

        public User User { get; set; }

        public Book Book { get; set; }

        #endregion
    }
}
