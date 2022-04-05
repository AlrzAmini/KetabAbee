using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.AudioBook
{
   public class ShowPlayerViewModel
    {
        public string ImageSavePath { get; set; }

        public string FileSavePath { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string Writer { get; set; }

        public string Speaker { get; set; }
    }
}
