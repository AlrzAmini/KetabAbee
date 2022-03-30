using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Comment;
using KetabAbee.Application.DTOs.Comment;
using KetabAbee.Application.DTOs.Paging;
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

        public async Task<CreateCommentAnswerResult> AddAnswer(CreateCommentAnswerViewModel answer)
        {
            var answerBody = answer.AnswerBody.Sanitizer();

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
                return await _commentRepository.AddAnswer(newAnswer) ? CreateCommentAnswerResult.Success : CreateCommentAnswerResult.Error;
            }
            catch
            {
                return CreateCommentAnswerResult.Error;
            }
        }

        public async Task<bool> AddComment(CreateCommentViewModel comment)
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

                return await _commentRepository.AddComment(newComment);
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
            return _commentRepository.UpdateAnswer(answer);
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

        public FilterCommentsViewModel FilterComments(FilterCommentsViewModel filter)
        {
            #region get comments

            var result = _commentRepository.GetComments()
                .Select(c => new CommentInAdminViewModel
                {
                    Body = c.Body,
                    CommentId = c.CommentId,
                    SendDate = c.SendDate,
                    SenderName = c.UserName,
                    ProductName = c.Product.Name,
                    IsReadByAdmin = c.IsReadByAdmin,
                    AnswersCount = c.Answers.Count,
                    UserId = c.UserId,
                    UserIp = c.UserIp
                }).AsQueryable();

            #endregion

            #region filter by sender name

            if (!string.IsNullOrEmpty(filter.UserName))
            {
                result = result.Where(r => r.SenderName.Contains(filter.UserName));
            }

            #endregion

            #region filter by body


            if (!string.IsNullOrEmpty(filter.Body))
            {
                result = result.Where(r => r.Body.Contains(filter.Body));
            }

            #endregion

            #region filter by is not read

            if (filter.IsNotReadByAdmin)
            {
                result = result.Where(r => !r.IsReadByAdmin);
            }

            #endregion

            #region paging

            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var comments = result.Paging(pager).ToList();

            #endregion

            return filter.SetPaging(pager).SetComments(comments);
        }

        public ChangeCommentIsReadResult ChangeCommentIsRead(int commentId)
        {
            try
            {
                var comment = GetCommentById(commentId);
                if (comment == null)
                {
                    return ChangeCommentIsReadResult.NotFound;
                }

                switch (comment.IsReadByAdmin)
                {
                    case true:
                        comment.IsReadByAdmin = false;
                        UpdateComment(comment);
                        return ChangeCommentIsReadResult.ChangedToIsNotRead;
                    case false:
                        comment.IsReadByAdmin = true;
                        UpdateComment(comment);
                        return ChangeCommentIsReadResult.ChangedToRead;
                }
            }
            catch
            {
                return ChangeCommentIsReadResult.Error;
            }
        }

        public DeleteEnglishCommentsResult DeleteEnglishComments()
        {
            try
            {
                var comments = _commentRepository.GetComments()
                    .Select(c => new DeleteEnglishCommentsViewModel()
                    {
                        CommentId = c.CommentId,
                        Body = c.Body
                    }).Where(c => c.Body.IsAllCharEnglish()).ToList();

                if (!comments.Any()) return DeleteEnglishCommentsResult.NotFoundAnyEnglishComment;

                foreach (var comment in comments)
                {
                    DeleteComment(comment.CommentId);
                }

                return DeleteEnglishCommentsResult.Success;
            }
            catch
            {
                return DeleteEnglishCommentsResult.Error;
            }
        }

        public int EnglishCommentsCount()
        {
            return _commentRepository.GetCommentsBodies().Count(c => c.IsAllCharEnglish());
        }

        public IEnumerable<CommentAnswersViewModel> GetCommentAnswers(int commentId)
        {
            return _commentRepository.GetCommentAnswers(commentId).Select(a => new CommentAnswersViewModel
            {
                UserName = a.UserName,
                Email = a.Email,
                SendDate = a.SendDate,
                AnswerBody = a.AnswerBody,
                CAnswerId = a.Id,
                UserId = a.UserId,
                UserIp = a.UserIp,
                CommentId = a.CommentId
            });
        }

    }
}
