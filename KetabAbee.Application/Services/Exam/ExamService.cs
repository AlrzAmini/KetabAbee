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

        public async Task<bool> DeleteExam(int examId)
        {
            var exam = await _examRepository.GetExamById(examId);
            if (exam == null)
            {
                return false;
            }
            return await _examRepository.RemoveExam(exam);
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
    }
}
