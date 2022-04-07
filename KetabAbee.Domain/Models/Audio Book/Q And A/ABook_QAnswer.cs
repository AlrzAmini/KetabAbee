using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Audio_Book.Q_And_A
{
    public class ABook_QAnswer
    {
        #region props

        [Key]
        public int AnswerId { get; set; }

        public int QuestionId { get; set; }

        public int? UserId { get; set; }

        public string UserIp { get; set; }

        public string Body { get; set; }

        public DateTime SendDate { get; set; } = DateTime.Now;

        #endregion

        #region relations

        public ABook_Question Question { get; set; }

        public User.User User { get; set; }

        #endregion
    }
}

