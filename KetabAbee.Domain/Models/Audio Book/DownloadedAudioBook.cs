using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Audio_Book
{
    public class DownloadedAudioBook
    {
        #region properties

        [Key]
        public int Id { get; set; }

        public int AudioBookId { get; set; }

        [MaxLength(16)]
        public string UserIp { get; set; }

        public DateTime DownloadDate { get; set; } = DateTime.Now;

        #endregion

        #region relations

        public AudioBook AudioBook { get; set; }

        #endregion
    }
}
