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
                .Where(c => c.ProductId == productId)
                .OrderByDescending(c=>c.SendDate);
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
    }
}
