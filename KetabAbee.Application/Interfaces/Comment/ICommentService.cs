using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Comment.ProductComment;

namespace KetabAbee.Application.Interfaces.Comment
{
    public interface ICommentService
    {
        #region commnet

        bool AddComment(ProductComment comment);

        bool DeleteComment(int commentId);

        bool UpdateComment(ProductComment comment);

        ProductComment GetCommentById(int commentId);

        IEnumerable<ProductComment> GetProductComments(int productId);

        Tuple<List<ProductComment>, int> GetProductCommentWithPaging(int productId, int pageNum = 1);

        int ProductCommentCount(int productId);

        #endregion

        #region answer

        bool AddAnswer(ProductCommentAnswer answer);

        #endregion
    }
}
