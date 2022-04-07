using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.AudioBook
{
   public class ShowAudioBookRequestInList
    {
        public string UserName { get; set; }

        public string UserIp { get; set; }

        public string BookTitle { get; set; }
    }
}
