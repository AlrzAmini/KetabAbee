using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products
{
    public class BookScore
    {
        [Key]
        public int ScoreId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }

        #region props

        [DisplayName("امتیاز")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0, 5, ErrorMessage = "{0} باید بین {1} و {2} باشد")]
        public int Score { get; set; }

        [DisplayName("آی پی")]
        [Required]
        public string UserIp { get; set; }

        [DisplayName("تاریخ ثبت امتیاز")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime ScoreDate { get; set; }

        [DisplayName("تاریخ بروزرسانی امتیاز")]
        public DateTime? UpdateScoreDate { get; set; }

        public bool IsScored { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public User.User User { get; set; }

        public Book Book { get; set; }

        #endregion
    }
}
