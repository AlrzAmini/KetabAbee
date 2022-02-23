using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Comment;
using KetabAbee.Application.DTOs.Comment;
using KetabAbee.Domain.Models.Comment.ProductComment;

namespace KetabAbee.Application.Interfaces.Comment
{
    public interface ICommentService
    {
        #region commnet

        bool AddComment(CreateCommentViewModel comment);

        bool DeleteComment(int commentId);

        bool UpdateComment(ProductComment comment);

        ProductComment GetCommentById(int commentId);

        IEnumerable<ProductComment> GetProductComments(int productId);

        Tuple<List<ProductComment>, int> GetProductCommentWithPaging(int productId, int pageNum = 1);

        int ProductCommentCount(int productId);

        bool IsUserSendComment(int userId, int commentId);

        bool IsUserSendComment(string userIp, int commentId);

        bool IsUserSendAnswer(int userId, int answerId);

        IEnumerable<ShowCommentInUserPanel> GetUserCommentsInUserPanel(int userId);

        #endregion

        #region answer

        CreateCommentAnswerResult AddAnswer(CreateCommentAnswerViewModel answer);

        bool DeleteAnswer(int answerId);

        bool UpdateAnswer(ProductCommentAnswer answer);

        ProductCommentAnswer GetAnswerById(int answerId);


        #endregion

        #region admin

        FilterCommentsViewModel FilterComments(FilterCommentsViewModel filter);

        ChangeCommentIsReadResult ChangeCommentIsRead(int commentId);

        DeleteEnglishCommentsResult DeleteEnglishComments();

        int EnglishCommentsCount();

        #endregion
    }
}
