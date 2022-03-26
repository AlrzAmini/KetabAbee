using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products
{
    public class Compare
    {
        #region properties

        [Key]
        public string CompareId { get; set; }

        [Required]
        [MaxLength(16)]
        public string UserIp { get; set; }

        public int? UserId { get; set; }

        public int? FirstBookId { get; set; }

        public int? SecondBookId { get; set; }

        [Required]
        public DateTime CompareDate { get; set; }

        public bool IsFull { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public Book FirstBook { get; set; }

        public Book SecondBook { get; set; }

        public User.User User { get; set; }

        #endregion
    }
}
