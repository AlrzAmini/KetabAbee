using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.User
{
    public class UserIp
    {
        #region props

        [Key]
        public int UserIpId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(15)]
        public string Ip { get; set; }

        #endregion

        #region relations

        public User User { get; set; }

        #endregion
    }
}
