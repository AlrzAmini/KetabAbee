using System.ComponentModel.DataAnnotations;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Domain.Models.Permission
{
    public class RolePermission
    {
        [Key]
        public int RolePrmId { get; set; }

        public int RoleId { get; set; }

        public int PermissionId { get; set; }


        #region Relations

        public Role Role { get; set; }

        public Permission Permission { get; set; }

        #endregion
    }
}
