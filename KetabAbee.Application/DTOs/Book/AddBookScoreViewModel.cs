using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Book
{
    public class AddBookScoreViewModel
    {
        public int BookId { get; set; }

        public int UserId { get; set; }

        public string UserIp { get; set; }

        public int QualityScore { get; set; }

        public int ContentScore { get; set; }
    }

    public enum AddScoreResult
    {
        Success,
        Failed,
        Error,
        OutRangeScoreValue
    }
}
