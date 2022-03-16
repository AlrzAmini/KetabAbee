using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Exam;
using KetabAbee.Domain.Models.Products.Exam;

namespace KetabAbee.Application.Interfaces.Exam
{
    public interface IExamService
    {
        Task<bool> CreateExam(CreateExamViewModel exam);

        Task<List<ShowExamInListViewModel>> GetExamsForAdmin();

        Task<bool> DeleteExam(int examId);

        Task<ExamDetailsViewModel> GetExamDetails(int examId);

        Task<bool> AddCorrectAnswer(int answerId, int questionId);

        Task<bool> ChangeIsActiveStatus(int examId);

        Task<CreateQuestionViewModel> GetCreateQuestionInfo(int examId);

        Task<bool> AddQuestionToExam(CreateQuestionViewModel question);

        Task<bool> DeleteQuestion(int questionId);

        Task<EditExamViewModel> GetInfoForEditExam(int examId);

        Task<bool> EditExam(EditExamViewModel exam);

        Task<ExamGuideViewModel> GetBookExamGuideInfo(int bookId);

        Task<LiveExamViewModel> GetLiveExamInfo(int examId);

        Task<bool> CheckJustOneAnswerSelectedInQuestion(List<int> answerIds);

        Task<int> GetQuestionCorrectAnswerId(int questionId);

        Task<int> GetExamQuestionsCount(int examId);

        Task<int> GetExamResultScore(int questionsCount, int correctAnswersCount);

        Task<int> CreateExamResult(ExamResult examResult);

        Task<ExamResult> GetExamResultById(int examResultId);

        Task<bool> IsUserCanDoExam(string userIp, int bookId);

        Task<bool> IsCorrectAnswerIsExistForAllExamQuestions(int examId);

        Task<List<ExamResult>> GetUserExamResultsByIp(string userIp);

        Task<List<ExamResult>> GetUserExamResultsById(int userId);

        Task<List<ExamResult>> GetExamResults();

        bool IsUserIpHaveAnyExam(string userIp);
    }
}
