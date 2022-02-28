using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.User
{
   public class UserFavoriteBooksViewModel
    {
        public int LikeId { get; set; }

        public int BookId { get; set; }

        public string UserName { get; set; }

        public string BookName { get; set; }

        public string BookImageName { get; set; }
    }
}
