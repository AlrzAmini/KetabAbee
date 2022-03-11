using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs.Admin.Exam;
using KetabAbee.Application.Interfaces.Exam;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.Products.Exam;

namespace KetabAbee.Application.Services.Exam
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;

        public ExamService(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<bool> AddCorrectAnswer(int answerId, int questionId)
        {
            if (await _examRepository.IsQuestionHaveCorrectAnswer(questionId))
            {
                await _examRepository.ChangeAllOfQuestionAnswerToIsNotCorrect(questionId);

                var addAnswer = await _examRepository.GetAnswerById(answerId);
                addAnswer.IsCorrect = true;
                await _examRepository.UpdateAnswer(addAnswer);

                var cAnswer = await _examRepository.GetCorrectAnswer(questionId);
                cAnswer.QuestionAnswerId = answerId;
                return await _examRepository.UpdateCorrectAnswer(cAnswer);
            }

            var correctAnswer = new CorrectAnswer
            {
                QuestionAnswerId = answerId,
                QuestionId = questionId
            };

            var answer = await _examRepository.GetAnswerById(answerId);
            answer.IsCorrect = true;
            await _examRepository.UpdateAnswer(answer);
            return await _examRepository.AddCorrectAnswer(correctAnswer);
        }

        public async Task<bool> AddQuestionToExam(CreateQuestionViewModel question)
        {
            try
            {
                var exam = await _examRepository.GetExamByIdWithIncludes(question.ExamId);
                if (exam == null)
                {
                    return false;
                }

                exam.Questions.Add(new ExamQuestion
                {
                    QBody = question.Question,
                    QuestionAnswers = new List<QuestionAnswer>
                    {
                        new(){QAnswerBody = question.QAnswer1},
                        new(){QAnswerBody = question.QAnswer2},
                        new(){QAnswerBody = question.QAnswer3},
                        new(){QAnswerBody = question.QAnswer4}
                    }
                });
                return await _examRepository.UpdateExam(exam);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangeIsActiveStatus(int examId)
        {
            var exam = await _examRepository.GetExamById(examId);
            if (exam == null)
            {
                return false;
            }

            if (exam.IsActive)
            {
                exam.IsActive = false;
                return await _examRepository.UpdateExam(exam);
            }

            var bookExams = await _examRepository.GetAllIsActiveBookExams(exam.BookId);
            if (bookExams != null && bookExams.Any())
            {
                foreach (var bExam in bookExams)
                {
                    bExam.IsActive = false;
                    await _examRepository.UpdateExam(bExam);
                }
            }
            exam.IsActive = true;
            return await _examRepository.UpdateExam(exam);
        }

        public async Task<bool> CheckJustOneAnswerSelectedInQuestion(List<int> answerIds)
        {
            var qIds = new List<int>();
            foreach (var id in answerIds)
            {
                var qId = await _examRepository.GetQuestionIdByAnswerId(id);
                qIds.Add(qId);
            }
            return qIds.Distinct().Count() == qIds.Count;
        }

        public async Task<bool> CreateExam(CreateExamViewModel exam)
        {
            var newExam = new Domain.Models.Products.Exam.Exam
            {
                BookId = exam.productId,
                Time = exam.Time,
                Questions = new List<ExamQuestion>
                {
                    new()
                    {
                        QBody = exam.Question,
                        QuestionAnswers = new List<QuestionAnswer>
                        {
                            new(){QAnswerBody = exam.QAnswer1},
                            new(){QAnswerBody = exam.QAnswer2},
                            new(){QAnswerBody = exam.QAnswer3},
                            new(){QAnswerBody = exam.QAnswer4}
                        }
                    }
                }
            };
            return await _examRepository.AddExam(newExam);
        }

        public async Task<int> CreateExamResult(ExamResult examResult)
        {
            try
            {
                examResult.ResultStatus = examResult.Score >= 75 ? ExamResultStatus.Pass : ExamResultStatus.Failed;
                return await _examRepository.CreateExamResult(examResult);
            }
            catch
            {
                return default;
            }
        }

        public async Task<bool> DeleteExam(int examId)
        {
            var exam = await _examRepository.GetExamById(examId);
            if (exam == null)
            {
                return false;
            }
            return await _examRepository.RemoveExam(exam);
        }

        public async Task<bool> DeleteQuestion(int questionId)
        {
            var question = await _examRepository.GetQuestionWithIncludesById(questionId);
            return await _examRepository.DeleteQuestion(question);
        }

        public async Task<bool> EditExam(EditExamViewModel exam)
        {
            var newExam = await _examRepository.GetExamById(exam.ExamId);
            newExam.BookId = exam.productId;
            newExam.Time = exam.Time;
            return await _examRepository.UpdateExam(newExam);
        }

        public async Task<ExamGuideViewModel> GetBookExamGuideInfo(int bookId)
        {
            var exam = await _examRepository.GetBookLiveExam(bookId);
            if (exam == null)
            {
                return null;
            }

            return new ExamGuideViewModel
            {
                BookName = exam.Book.Name,
                QuestionsCount = exam.Questions.Count,
                Time = exam.Time,
                BookId = exam.BookId,
                ExamId = exam.ExamId
            };
        }

        public async Task<CreateQuestionViewModel> GetCreateQuestionInfo(int examId)
        {
            return new CreateQuestionViewModel
            {
                BookName = await _examRepository.GetExamBookName(examId),
                ExamId = examId
            };
        }

        public async Task<ExamDetailsViewModel> GetExamDetails(int examId)
        {
            var exam = await _examRepository.GetExamByIdWithIncludes(examId);
            if (exam == null)
            {
                return new ExamDetailsViewModel();
            }

            return new ExamDetailsViewModel
            {
                BookId = exam.BookId,
                BookName = exam.Book.Name,
                Time = exam.Time,
                ExamId = examId,
                Questions = exam.Questions,
                IsActive = exam.IsActive
            };
        }

        public async Task<int> GetExamQuestionsCount(int examId)
        {
            return await _examRepository.GetExamQuestionsCount(examId);
        }

        public async Task<ExamResult> GetExamResultById(int examResultId)
        {
            return await _examRepository.GetExamResultById(examResultId);
        }

        public async Task<List<ExamResult>> GetExamResults()
        {
            return await _examRepository.GetExamResults();
        }

        public async Task<int> GetExamResultScore(int questionsCount, int correctAnswersCount)
        {
            if (questionsCount == 0)
            {
                return default;
            }
            if (questionsCount == correctAnswersCount)
            {
                return 100;
            }
            var coefficient = 100.0 / questionsCount;
            return await System.Threading.Tasks.Task.Run(() => (int)Math.Round(correctAnswersCount * coefficient));
        }

        public async Task<List<ShowExamInListViewModel>> GetExamsForAdmin()
        {
            var exams = await _examRepository.GetExams();
            if (!exams.Any())
            {
                return new List<ShowExamInListViewModel>();
            }

            return exams.Select(e => new ShowExamInListViewModel
            {
                BookId = e.BookId,
                Time = e.Time,
                ExamId = e.ExamId,
                IsActive = e.IsActive,
                BookName = e.Book.Name
            }).ToList();
        }

        public async Task<EditExamViewModel> GetInfoForEditExam(int examId)
        {
            var exam = await _examRepository.GetExamByIdWithIncludes(examId);
            return new EditExamViewModel
            {
                ProductName = exam.Book.Name,
                Time = exam.Time,
                productId = exam.BookId,
                ExamId = exam.ExamId
            };
        }

        public async Task<LiveExamViewModel> GetLiveExamInfo(int examId)
        {
            var exam = await _examRepository.GetExamByIdWithIncludes(examId);
            if (exam == null)
            {
                return null;
            }

            return new LiveExamViewModel
            {
                Exam = exam
            };
        }

        public async Task<int> GetQuestionCorrectAnswerId(int questionId)
        {
            return await _examRepository.GetQuestionCorrectAnswerId(questionId);
        }

        public async Task<List<ExamResult>> GetUserExamResultsById(int userId)
        {
            return await _examRepository.GetUserExamResultsById(userId);
        }

        public async Task<List<ExamResult>> GetUserExamResultsByIp(string userIp)
        {
            return await _examRepository.GetUserExamResultsByIp(userIp);
        }

        public async Task<bool> IsCorrectAnswerIsExistForAllExamQuestions(int examId)
        {
            var exam = await _examRepository.GetExamByIdWithIncludes(examId);
            foreach (var qId in exam.Questions.Select(q=>q.QuestionId))
            {
                if (!await _examRepository.IsQuestionHaveCorrectAnswer(qId))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> IsUserCanDoExam(string userIp, int bookId)
        {
            var resultsCount = await _examRepository.GetUserBookExamResultsCount(userIp, bookId);
            return resultsCount <= 2;
        }
    }
}
