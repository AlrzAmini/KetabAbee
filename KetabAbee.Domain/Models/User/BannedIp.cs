using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.User
{
    public class BannedIp
    {
        [Key]
        public int BanId { get; set; }

        public int? UserId { get; set; }

        [Required]
        public string Ip { get; set; }

        public bool IsDelete { get; set; }

        public User User { get; set; }
    }

    public enum BanUserIpResult
    {
        Success,
        Error,
        NotHaveIp
    }

    public enum FreeUserIpResult
    {
        Success,
        Error,
        NotHaveIp,
        UserIsAlreadyFree
    }

    public enum BanIpResult
    {
        Success,
        Error,
        IpIsAlreadyExist
    }
}
