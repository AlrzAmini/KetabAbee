using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Book
{
    public class CompareBooksViewModel
    {
        public BookInCompareBooksViewModel FirstBook { get; set; }

        public BookInCompareBooksViewModel SecondBook { get; set; } 
    }
}
