using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.AudioBook
{
    public class ShowAudioBookInfoViewModel
    {
        #region properties

        public int AudioBookId { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string Writer { get; set; }

        public string Speaker { get; set; }

        public string Review { get; set; }

        public string PageDescription { get; set; }

        public string ImageSavePath { get; set; }

        public string FileSavePath { get; set; }

        public float FileSize { get; set; }

        public int Time { get; set; }

        public string ImageName { get; set; }

        public DateTime CreateDate { get; set; }

        #endregion
    }
}
