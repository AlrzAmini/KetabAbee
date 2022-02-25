using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Task;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KetabAbee.Application.DTOs.Admin.Task
{
    public class CreateTaskViewModel
    {
        [Required]
        public int UserId { get; set; }

        [DisplayName("نقش هدف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int SelectRoleId { get; set; }

        [DisplayName("متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(600, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Body { get; set; }

        [DisplayName("تاریخ شروع تسک")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string CreateDate { get; set; }

        [DisplayName("تاریخ پایان تسک")]
        public string DeadLine { get; set; }

        [DisplayName("میزان اهمیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public TaskPriority TaskPriority { get; set; }

        public List<SelectListItem> RolesList { get; set; }
    }

    public enum CreateTaskResult
    {
        Success,
        Error,
        DateError
    }
}
