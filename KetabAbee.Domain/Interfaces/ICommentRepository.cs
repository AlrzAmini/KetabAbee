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

        bool IsUserSendComment(int userId, int commentId);

        bool IsUserSendComment(string userIp, int commentId);

        bool IsUserSendAnswer(int userId, int answerId);

        IEnumerable<ProductComment> GetUserComments(int userId);


        #endregion

        #region answer

        bool AddAnswer(ProductCommentAnswer answer);

        bool DeleteAnswer(int answerId);

        bool UpdateAnswer(ProductCommentAnswer answer);

        ProductCommentAnswer GetAnswerById(int answerId);

        #endregion
    }
}
