using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.Products.Exam;

//using KetabAbee.Domain.Models.Products.Exam;

namespace KetabAbee.Domain.Interfaces
{
    public interface IExamRepository
    {
        Task<bool> AddExam(Exam exam);

        Task<bool> RemoveExam(Exam exam);

        Task<Exam> GetExamById(int examId);

        Task<Exam> GetExamByIdWithIncludes(int examId);

        Task<bool> UpdateExam(Exam exam);

        Task<List<Exam>> GetExams();

        Task<List<ExamQuestion>> GetExamQuestions(int examId);

        Task<bool> AddCorrectAnswer(CorrectAnswer correctAnswer);

        Task<bool> IsQuestionHaveCorrectAnswer(int questionId);

        Task<CorrectAnswer> GetCorrectAnswer(int questionId);

        Task<bool> UpdateCorrectAnswer(CorrectAnswer answer);

        Task<QuestionAnswer> GetAnswerById(int answerId);

        Task<bool> UpdateAnswer(QuestionAnswer answer);

        Task ChangeAllOfQuestionAnswerToIsNotCorrect(int questionId);

        Task<List<Exam>> GetAllIsActiveBookExams(int bookId);

        Task<string> GetExamBookName(int examId);
    }
}
