using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Const;
using KetabAbee.Application.DTOs.Admin.Exam;
using KetabAbee.Application.Interfaces.Exam;
using KetabAbee.Application.Security;

namespace KetabAbee.Web.Areas.AdminPanel.Controllers.Exam
{
    [PermissionChecker(PerIds.AdminExams)]
    [Route("Admin/Exams")]
    public class ExamController : AdminBaseController
    {
        #region constructor

        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        #endregion

        #region index

        public async Task<IActionResult> Index()
        {
            return View(await _examService.GetExamsForAdmin());
        }

        #endregion

        #region exam results

        [HttpGet("Results")]
        public async Task<IActionResult> IndexResults()
        {
            return View(await _examService.GetExamResults());
        }

        #endregion

        #region add exam

        [HttpGet("Create")]
        public IActionResult CreateExam()
        {
            return View();
        }

        [HttpPost("Create"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExam(CreateExamViewModel exam)
        {
            if (!ModelState.IsValid)
            {
                return View(exam);
            }

            if (await _examService.CreateExam(exam))
            {
                TempData["SuccessMessage"] = "آزمون با موفقیت ایجاد شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "مشکلی در ایجاد آزمون رخ داد";
            return RedirectToAction("Index");
        }

        #endregion

        #region delete exam

        [HttpGet("Delete")]
        public async Task<IActionResult> DeleteExam(int examId)
        {
            if (await _examService.DeleteExam(examId))
            {
                TempData["SuccessMessage"] = "آزمون با موفقیت حذف شد";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "آزمون حذف نشد";
            return RedirectToAction("Index");
        }

        #endregion

        #region exam details

        [HttpGet("Details")]
        public async Task<IActionResult> ExamDetails(int examId)
        {
            var exam = await _examService.GetExamDetails(examId);
            if (!string.IsNullOrEmpty(exam.BookName))
            {
                ViewBag.IsAllOfQuestionsHaveAnswer =
                    await _examService.IsCorrectAnswerIsExistForAllExamQuestions(examId);
                return View(exam);
            }

            TempData["WarningMessage"] = "آزمونی یافت نشد";
            return RedirectToAction("Index");
        }

        #endregion

        #region select correct answer

        [HttpGet("CorrectAnswer/A/{answerId}/Q/{questionId}/E/{examId}")]
        public async Task<IActionResult> AddCorrectAnswer(int answerId, int questionId, int examId)
        {
            if (await _examService.AddCorrectAnswer(answerId, questionId))
            {
                TempData["SuccessMessage"] = "پاسخ صحیح با موفقیت ثبت شد";
                return RedirectToAction("ExamDetails", new { examId });
            }
            TempData["ErrorMessage"] = "مشکلی در ثبت پاسخ رخ داد";
            return RedirectToAction("ExamDetails", new { examId });
        }

        #endregion

        #region change is active status

        [HttpGet("ChangeIsActive/examId")]
        public async Task<IActionResult> ChangeIsActiveStatus(int examId)
        {
            if (await _examService.ChangeIsActiveStatus(examId))
            {
                TempData["SuccessMessage"] = "با موفقیت انجام شد";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion

        #region Create New Question

        [HttpGet("Create-Question/E/{examId}")]
        public async Task<IActionResult> CreateQuestion(int examId)
        {
            var model = await _examService.GetCreateQuestionInfo(examId);
            if (model != null) return View(model);

            TempData["ErrorMessage"] = "آزمونی یافت نشد";
            return RedirectToAction("ExamDetails", new { examId });
        }

        [HttpPost("Create-Question/E/{examId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(CreateQuestionViewModel questionModel)
        {
            if (!ModelState.IsValid)
            {
                return View(questionModel);
            }

            if (await _examService.AddQuestionToExam(questionModel))
            {
                TempData["SuccessMessage"] = "سوال افزوده شد";
                return RedirectToAction("ExamDetails", new { examId = questionModel.ExamId });
            }
            TempData["ErrorMessage"] = "مشکلی در ثبت سوال رخ داد";
            return RedirectToAction("ExamDetails", new { examId = questionModel.ExamId });
        }

        #endregion

        #region Edit Question

        [HttpGet("Edit/{examId}")]
        public async Task<IActionResult> EditExam(int examId)
        {
            var exam = await _examService.GetInfoForEditExam(examId);
            if (exam != null) return View(exam);

            TempData["ErrorMessage"] = "آزمونی یافت نشد";
            return RedirectToAction("Index");
        }

        [HttpPost("Edit/{examId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditExam(EditExamViewModel exam)
        {
            if (!ModelState.IsValid)
            {
                return View(exam);
            }

            var res = await _examService.EditExam(exam);
            switch (res)
            {
                case EditExamResult.Success:
                    TempData["SuccessMessage"] = "آزمون ویرایش شد";
                    return RedirectToAction("Index");
                case EditExamResult.NotFound:
                    TempData["ErrorMessage"] = "آزمون یافت نشد";
                    return RedirectToAction("Index");
                case EditExamResult.BookHaveActiveExam:
                    TempData["ErrorMessage"] = "کتاب انتخابی در حال حاضر یک آزمون فعال دارد . یک کتاب نمیتواند دو آزمون فعال داشته باشد";
                    return RedirectToAction("EditExam", new { examId = exam.ExamId });
                case EditExamResult.Error:
                    TempData["ErrorMessage"] = "مشکلی رخ داد";
                    return RedirectToAction("Index");
                default:
                    TempData["ErrorMessage"] = "مشکلی رخ داد";
                    return RedirectToAction("Index");
            }
        }

        #endregion

        #region delete question

        [HttpGet("Delete/Q/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int questionId, int examId)
        {
            if (await _examService.DeleteQuestion(questionId))
            {
                TempData["SuccessMessage"] = "سوال با موفقیت حذف شد";
                return RedirectToAction("ExamDetails", new { examId });
            }
            TempData["ErrorMessage"] = "حذف سوال با مشکل مواجه شد";
            return RedirectToAction("ExamDetails", new { examId });
        }

        #endregion
    }
}
