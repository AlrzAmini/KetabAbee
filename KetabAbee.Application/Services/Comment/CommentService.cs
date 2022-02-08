﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Comment;
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

        public bool AddAnswer(ProductCommentAnswer answer)
        {
            answer.SendDate = DateTime.Now;
            return _commentRepository.AddAnswer(answer);
        }

        public bool AddComment(ProductComment comment)
        {
            comment.SendDate = DateTime.Now;
            return _commentRepository.AddComment(comment);
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
