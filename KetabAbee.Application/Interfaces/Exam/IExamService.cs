using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Exam;

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
    }
}
