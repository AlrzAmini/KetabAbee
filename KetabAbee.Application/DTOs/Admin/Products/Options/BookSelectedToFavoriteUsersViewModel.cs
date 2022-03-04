using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Products.Options
{
    public class BookSelectedToFavoriteUsersViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string ImageName { get; set; }
    }
}
