using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Task;

namespace KetabAbee.Application.DTOs.Admin.Task
{
    public class CreateTaskViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int SelectRoleId { get; set; }

        [DisplayName("متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(600, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Body { get; set; }

        [Required]
        public string CreateDate { get; set; }

        public string DeadLine { get; set; }

        [DisplayName("میزان اهمیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public TaskPriority TaskPriority { get; set; }
    }
}
