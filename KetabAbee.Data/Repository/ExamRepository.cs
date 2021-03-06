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

        public async Task AddExamTry(ExamTry examTry)
        {
            await _context.ExamTries.AddAsync(examTry);
            await _context.SaveChangesAsync();
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

        public async Task<int> CreateExamResult(ExamResult examResult)
        {
            await _context.ExamResults.AddAsync(examResult);
            await _context.SaveChangesAsync();
            return examResult.ExamResultId;
        }

        public async Task<bool> DeleteQuestion(ExamQuestion question)
        {
            try
            {
                foreach (var answer in question.QuestionAnswers)
                {
                    answer.IsDelete = true;
                }


                var correctAnswer = await GetCorrectAnswer(question.QuestionId);
                correctAnswer.IsDelete = true;
                await UpdateCorrectAnswer(correctAnswer);

                question.IsDelete = true;
                _context.ExamQuestions.Update(question);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Exam>> GetAllIsActiveBookExams(int bookId)
        {
            return await _context.Exams.Where(e => e.BookId == bookId && e.IsActive).ToListAsync();
        }

        public async Task<QuestionAnswer> GetAnswerById(int answerId)
        {
            return await _context.QuestionAnswers.FindAsync(answerId);
        }

        public async Task<Exam> GetBookLiveExam(int bookId)
        {
            return await _context.Exams
                .Include(e => e.Book)
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.BookId == bookId && e.IsActive);
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

        public async Task<int> GetExamQuestionsCount(int examId)
        {
            var exam = await GetExamByIdWithIncludes(examId);
            return exam.Questions.Count;
        }

        public async Task<ExamResult> GetExamResultById(int examResultId)
        {
            return await _context.ExamResults.FindAsync(examResultId);
        }

        public async Task<List<ExamResult>> GetExamResults()
        {
            return await _context.ExamResults
                .Include(r=>r.Exam)
                .ThenInclude(r=>r.Book)
                .OrderByDescending(r=>r.ExamResultId)
                .ToListAsync();
        }

        public async Task<List<Exam>> GetExams()
        {
            return await _context.Exams
                .Include(e => e.Book)
                .OrderByDescending(e => e.ExamId)
                .ToListAsync();
        }

        public async Task<int> GetQuestionCorrectAnswerId(int questionId)
        {
            var correctAnswer = await _context.CorrectAnswers
                .FirstOrDefaultAsync(c => c.QuestionId == questionId);
            return correctAnswer.QuestionAnswerId;
        }

        public async Task<int> GetQuestionIdByAnswerId(int answerId)
        {
            var qAnswer = await _context.QuestionAnswers.FirstOrDefaultAsync(q => q.QAnswerId == answerId);
            return qAnswer.QuestionId;
        }

        public async Task<ExamQuestion> GetQuestionWithIncludesById(int questionId)
        {
            return await _context.ExamQuestions
                .Include(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(q => q.QuestionId == questionId);
        }

        public async Task<int> GetUserBookExamResultsCount(string userIp, int bookId)
        {
            return await _context.ExamResults
                .Include(r => r.Exam)
                .CountAsync(r => r.Exam.BookId == bookId && r.UserIp == userIp);
        }

        public async Task<List<ExamResult>> GetUserExamResultsById(int userId)
        {
            return await _context.ExamResults
                .Include(r => r.Exam)
                .ThenInclude(r => r.Book)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<ExamResult>> GetUserExamResultsByIp(string userIp)
        {
            return await _context.ExamResults
                .Include(r => r.Exam)
                .ThenInclude(r => r.Book)
                .Where(r => r.UserIp == userIp)
                .ToListAsync();
        }

        public async Task<bool> IsQuestionHaveCorrectAnswer(int questionId)
        {
            return await _context.CorrectAnswers.AnyAsync(a => a.QuestionId == questionId);
        }

        public bool IsUserIpHaveAnyExamResult(string userIp)
        {
            return _context.ExamResults.Any(r=>r.UserIp == userIp);
        }

        public async Task<int> GetUserExamTriesCount(string userIp, int examId)
        {
            return await _context.ExamTries.CountAsync(t => t.ExamId == examId && t.UserIp == userIp);
        }

        public async Task<int> GetUserExamTriesCount(int userId, int examId)
        {
            return await _context.ExamTries.CountAsync(t => t.ExamId == examId && t.UserId == userId);
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

        public async Task<bool> IsBookHaveActiveExam(int bookId)
        {
            return await _context.Exams.AnyAsync(e => e.BookId == bookId && e.IsActive);
        }
    }
}
