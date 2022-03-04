using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Comment.ProductComment;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Data.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly KetabAbeeDBContext _context;

        public CommentRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public bool IsUserSendComment(string userIp, int commentId)
        {
            return _context.ProductComments.Any(c => c.UserIp == userIp && c.CommentId == commentId);
        }

        public IEnumerable<ProductComment> GetUserComments(int userId)
        {
            return _context.ProductComments.Where(c => c.UserId == userId);
        }

        public bool AddAnswer(ProductCommentAnswer answer)
        {
            try
            {
                _context.ProductCommentAnswers.Add(answer);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddComment(ProductComment comment)
        {
            try
            {
                _context.ProductComments.Add(comment);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteComment(int commentId)
        {
            try
            {
                var comment = GetCommentById(commentId);
                comment.IsDelete = true;
                return UpdateComment(comment);
            }
            catch
            {
                return false;
            }
        }

        public ProductComment GetCommentById(int commentId)
        {
            return _context.ProductComments.Find(commentId);
        }

        public IEnumerable<ProductComment> GetProductComments(int productId)
        {
            return _context.ProductComments.Include(c => c.User)
                .Include(c => c.Answers)
                .Include(c => c.Product)
                .Where(c => c.ProductId == productId)
                .OrderByDescending(c => c.SendDate);
        }

        public bool IsUserSendComment(int userId, int commentId)
        {
            return _context.ProductComments.Any(c => c.UserId == userId && c.CommentId == commentId);
        }

        public int ProductCommentCount(int productId)
        {
            return _context.ProductComments.Count(c => c.ProductId == productId);
        }

        public bool UpdateComment(ProductComment comment)
        {
            try
            {
                _context.ProductComments.Update(comment);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsUserSendAnswer(int userId, int answerId)
        {
            return _context.ProductCommentAnswers.Any(a => a.UserId == userId && a.Id == answerId);
        }

        public bool DeleteAnswer(int answerId)
        {
            try
            {
                var answer = GetAnswerById(answerId);
                answer.IsDelete = true;
                return UpdateAnswer(answer);
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateAnswer(ProductCommentAnswer answer)
        {
            try
            {
                _context.ProductCommentAnswers.Update(answer);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ProductCommentAnswer GetAnswerById(int answerId)
        {
            return _context.ProductCommentAnswers.Find(answerId);
        }

        public IEnumerable<ProductComment> GetComments()
        {
            return _context.ProductComments
                .Include(c => c.Product)
                .Include(c => c.Answers)
                .OrderByDescending(c => c.SendDate);
        }

        public List<string> GetCommentsBodies()
        {
            return _context.ProductComments.Select(c => c.Body).ToList();
        }

        public IEnumerable<ProductCommentAnswer> GetCommentAnswers(int commentId)
        {
            return _context.ProductCommentAnswers
                .Where(a => a.CommentId == commentId)
                .OrderByDescending(a => a.SendDate);
        }
    }
}
