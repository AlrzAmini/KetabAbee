using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetabAbee.Domain.Models.Products
{
    public class CompareItem
    {
        #region properties

        [Key]
        public int CompareItemId { get; set; }

        [Required]
        public string CompareId { get; set; }

        [Required]
        public int BookId { get; set; }

        public bool IsDelete { get; set; }

        #endregion

        #region relations

        public Compare Compare { get; set; }

        public Book Book { get; set; }

        #endregion
    }
}
