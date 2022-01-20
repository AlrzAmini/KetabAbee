using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Book
{
   public class BookListViewModel
    {
        public string ImageName { get; set; }

        public string Name { get; set; }

        public string Writer { get; set; }

        public string PublisherName { get; set; }

        public int Price { get; set; }

        public int BookId { get; set; }
    }
}
