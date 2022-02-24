using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Comment.CommentReport
{
    public class CommentsReport
    {
        [Key]
        public int CReportId { get; set; }

        [Required]
        public int CommentId { get; set; }

        public int? UserId { get; set; }

        #region props

        [DisplayName("توضیحات")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string Description { get; set; }

        [DisplayName("آی پی")]
        [MaxLength(15, ErrorMessage = "{0} نمیتواند بیش از {1} کاراکتر داشته باشد")]
        public string UserIp { get; set; }

        [Required]
        public DateTime ReportData { get; set; }

        #endregion

        #region relations

        public ProductComment.ProductComment Comment { get; set; }

        public User.User User { get; set; }

        #endregion
    }
}
