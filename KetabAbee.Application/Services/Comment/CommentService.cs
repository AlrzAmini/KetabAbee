using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Comment;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Interfaces.Comment;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Comment.ProductComment;

namespace KetabAbee.Application.Services.Comment
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public bool IsUserSendComment(int userId, int commentId)
        {
            return _commentRepository.IsUserSendComment(userId, commentId);
        }

        public CreateCommentAnswerResult AddAnswer(CreateCommentAnswerViewModel answer)
        {
            var answerBody = answer.AnswerBody.Sanitizer();
            if (string.IsNullOrEmpty(answerBody))
            {
                return CreateCommentAnswerResult.EmptyBody;
            }

            try
            {
                var newAnswer = new ProductCommentAnswer
                {
                    AnswerBody = answerBody,
                    CommentId = answer.CommentId,
                    Email = answer.Email,
                    SendDate = DateTime.Now,
                    UserId = answer.UserId,
                    UserIp = answer.UserIp,
                    UserName = answer.UserName
                };
                return _commentRepository.AddAnswer(newAnswer) ? CreateCommentAnswerResult.Success : CreateCommentAnswerResult.Error;
            }
            catch
            {
                return CreateCommentAnswerResult.Error;
            }
        }

        public bool AddComment(CreateCommentViewModel comment)
        {
            try
            {
                var commentBody = comment.Body.Sanitizer();
                if (string.IsNullOrEmpty(commentBody))
                {
                    return false;
                }

                var newComment = new ProductComment
                {
                    SendDate = DateTime.Now,
                    Body = commentBody,
                    Email = comment.Email.Sanitizer(),
                    ProductId = comment.ProductId,
                    UserId = comment.UserId,
                    UserIp = comment.UserIp,
                    UserName = comment.UserName.Sanitizer()
                };

                return _commentRepository.AddComment(newComment);
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteComment(int commentId)
        {
            return _commentRepository.DeleteComment(commentId);
        }

        public ProductComment GetCommentById(int commentId)
        {
            return _commentRepository.GetCommentById(commentId);
        }

        public IEnumerable<ProductComment> GetProductComments(int productId)
        {
            return _commentRepository.GetProductComments(productId);
        }

        public Tuple<List<ProductComment>, int> GetProductCommentWithPaging(int productId, int pageNum = 1)
        {
            var take = Paging.CommentPerPage;
            var skip = (pageNum - 1) * take;

            var pageCount = (int)Math.Ceiling((decimal)ProductCommentCount(productId) / take);

            return Tuple.Create(GetProductComments(productId)
                .Skip(skip)
                .Take(take)
                .ToList(), pageCount);
        }

        public int ProductCommentCount(int productId)
        {
            return _commentRepository.ProductCommentCount(productId);
        }

        public bool UpdateComment(ProductComment comment)
        {
            return _commentRepository.UpdateComment(comment);
        }

        public bool IsUserSendComment(string userIp, int commentId)
        {
            return _commentRepository.IsUserSendComment(userIp, commentId);
        }

        public bool IsUserSendAnswer(int userId, int answerId)
        {
            return _commentRepository.IsUserSendAnswer(userId, answerId);
        }

        public bool DeleteAnswer(int answerId)
        {
            return _commentRepository.DeleteAnswer(answerId);
        }

        public bool UpdateAnswer(ProductCommentAnswer answer)
        {
            return _commentRepository.AddAnswer(answer);
        }

        public ProductCommentAnswer GetAnswerById(int answerId)
        {
            return _commentRepository.GetAnswerById(answerId);
        }

        public IEnumerable<ShowCommentInUserPanel> GetUserCommentsInUserPanel(int userId)
        {
            return _commentRepository.GetUserComments(userId).Select(c => new ShowCommentInUserPanel
            {
                Body = c.Body,
                Id = c.CommentId,
                SendDate = c.SendDate,
                UserName = c.UserName
            });
        }
    }
}
