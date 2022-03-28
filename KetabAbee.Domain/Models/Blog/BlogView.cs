using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Blog
{
    public class BlogView
    {
        #region properties

        [Key]
        public int Id { get; set; }

        [MaxLength(16)]
        public string UserIp { get; set; }

        public int? UserId { get; set; }

        public int BlogId { get; set; }

        #endregion


        #region relations

        public User.User User { get; set; }

        public Blog Blog { get; set; }

        #endregion
    }
}
