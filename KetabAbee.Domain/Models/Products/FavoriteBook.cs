using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products
{
    public class FavoriteBook
    {
        [Key]
        public int LikeId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }

        public bool IsLiked { get; set; }

        public bool IsDelete { get; set; }

        #region relations

        public User.User User { get; set; }

        public Book Book { get; set; }

        #endregion
    }
}
