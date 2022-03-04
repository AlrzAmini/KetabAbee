using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Application.DTOs.Admin.Products.Options
{
    public class BookScoreViewModel
    {
        public int ScoreId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int BookId { get; set; }

        public string BookName { get; set; }

        public int QualityScore { get; set; }

        public int ContentScore { get; set; }

        public float AverageScores { get; set; }

        public string UserIp { get; set; }

        public DateTime ScoreDate { get; set; }
    }
}
