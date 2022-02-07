using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Comment.ProductComment;

namespace KetabAbee.Domain.Interfaces
{
    public interface ICommentRepository
    {
        #region comment

        bool AddComment(ProductComment comment);

        bool DeleteComment(int commentId);

        bool UpdateComment(ProductComment comment);

        ProductComment GetCommentById(int commentId);

        IEnumerable<ProductComment> GetProductComments(int productId);

        int ProductCommentCount(int productId);

        #endregion

        #region answer

        bool AddAnswer(ProductCommentAnswer answer);

        #endregion
    }
}
