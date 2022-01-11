using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.User
{
    public class Role
    {
        #region Role ID

        [Key]
        public int RoleId { get; set; }

        #endregion

        #region Role Title

        [DisplayName("عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string RoleTitle { get; set; }

        #endregion

        public bool IsDelete { get; set; }

        #region Relations

        public List<UserRole> UserRoles { get; set; }

        //public List<RolePermission> RolePermissions { get; set; }

        #endregion
    }
}
