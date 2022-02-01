using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KetabAbee.Domain.Models.Order
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }


        #region props

        [Required]
        public int OrderSum { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [DisplayName("آدرس")]
        [MaxLength(700, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Address { get; set; }

        public bool IsFinally { get; set; }

        public bool SendingProcessIsCompleted { get; set; }

        public bool IsDelete { get; set; }

        #endregion


        #region realtions

        public User.User User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}
