using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Products.Exam;
using Microsoft.EntityFrameworkCore;

namespace KetabAbee.Data.Repository
{
    public class ExamRepository : IExamRepository
    {
        private readonly KetabAbeeDBContext _context;

        public ExamRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCorrectAnswer(CorrectAnswer correctAnswer)
        {
            try
            {
                await _context.CorrectAnswers.AddAsync(correctAnswer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddExam(Exam exam)
        {
            try
            {
                await _context.Exams.AddAsync(exam);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task ChangeAllOfQuestionAnswerToIsNotCorrect(int questionId)
        {
            var qAnswers = await _context.QuestionAnswers.Where(a => a.QuestionId == questionId).ToListAsync();
            foreach (var answer in qAnswers)
            {
                answer.IsCorrect = false;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Exam>> GetAllIsActiveBookExams(int bookId)
        {
            return await _context.Exams.Where(e => e.BookId == bookId && e.IsActive).ToListAsync();
        }

        public async Task<QuestionAnswer> GetAnswerById(int answerId)
        {
            return await _context.QuestionAnswers.FindAsync(answerId);
        }

        public async Task<CorrectAnswer> GetCorrectAnswer(int questionId)
        {
            return await _context.CorrectAnswers
                .FirstOrDefaultAsync(a => a.QuestionId == questionId);
        }

        public async Task<string> GetExamBookName(int examId)
        {
            var exam = await GetExamByIdWithIncludes(examId);
            return exam.Book.Name;
        }

        public async Task<Exam> GetExamById(int examId)
        {
            return await _context.Exams.FindAsync(examId);
        }

        public async Task<Exam> GetExamByIdWithIncludes(int examId)
        {
            return await _context.Exams
                .Include(e => e.Book)
                .Include(e => e.Questions)
                .ThenInclude(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(e => e.ExamId == examId);
        }

        public async Task<List<ExamQuestion>> GetExamQuestions(int examId)
        {
            return await _context.ExamQuestions
                .Where(q => q.ExamId == examId)
                .ToListAsync();
        }

        public async Task<List<Exam>> GetExams()
        {
            return await _context.Exams
                .Include(e => e.Book)
                .OrderByDescending(e => e.ExamId)
                .ToListAsync();
        }

        public async Task<bool> IsQuestionHaveCorrectAnswer(int questionId)
        {
            return await _context.CorrectAnswers.AnyAsync(a => a.QuestionId == questionId);
        }

        public async Task<bool> RemoveExam(Exam exam)
        {
            exam.IsDelete = true;
            return await UpdateExam(exam);
        }

        public async Task<bool> UpdateAnswer(QuestionAnswer answer)
        {
            try
            {
                _context.QuestionAnswers.Update(answer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCorrectAnswer(CorrectAnswer answer)
        {
            try
            {
                _context.CorrectAnswers.Update(answer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateExam(Exam exam)
        {
            try
            {
                _context.Exams.Update(exam);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
