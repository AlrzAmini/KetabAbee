using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.AudioBook
{
    public class ShowAudioBookInListViewModel
    {
        #region props

        public int AudioBookId { get; set; }

        public string Title { get; set; }

        public string Writer { get; set; }

        public string Speaker { get; set; }

        public string ImageLocation { get; set; }

        public DateTime CreateDate { get; set; }

        public float FileSize { get; set; }

        #endregion
    }
}
