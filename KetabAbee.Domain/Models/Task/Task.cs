using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Domain.Models.Task
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        public int RoleId { get; set; }

        #region properties

        [DisplayName("متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(600, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Body { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime DeadLine { get; set; }

        [DisplayName("میزان اهمیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public TaskPriority TaskPriority { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsOutOfDate { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region Relations

        public User.User Creator { get; set; }

        public Role Role { get; set; }

        #endregion
    }

    public enum TaskPriority
    {
        [Display(Name = "مهم")]
        Important,
        [Display(Name = "عادی")]
        Normal,
        [Display(Name = "غیر ضروری")]
        NotImportant
    }
}
