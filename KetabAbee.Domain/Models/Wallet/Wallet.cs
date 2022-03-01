using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Wallet
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }

        [Required]
        public int UserId { get; set; }

        [DisplayName("نوع تراکنش")]
        [Required]
        public WalletType WalletType { get; set; }

        #region Props

        [DisplayName("مبلغ")]
        [Required]
        public long Amount { get; set; }

        [DisplayName("بابت")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Behalf { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [DisplayName("انجام شد")]
        public bool IsPay { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region Relations

        public User.User User { get; set; }

        #endregion
    }

    public enum WalletType
    {
        [Display(Name = "واریز")]
        Deposit,
        [Display(Name = "برداشت")]
        Withdraw
    }
}
