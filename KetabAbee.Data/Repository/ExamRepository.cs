using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
//using KetabAbee.Domain.Models.Products.Exam;

namespace KetabAbee.Data.Repository
{
    //public class ExamRepository : IExamRepository
    //{
    //    private readonly KetabAbeeDBContext _context;

    //    public ExamRepository(KetabAbeeDBContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<bool> AddExam(Exam exam)
    //    {
    //        try
    //        {
    //            await _context.Exams.AddAsync(exam);
    //            await _context.SaveChangesAsync();
    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }

    //    public async Task<Exam> GetExamById(int examId)
    //    {
    //        return await _context.Exams.FindAsync(examId);
    //    }

    //    public async Task<bool> RemoveExam(Exam exam)
    //    {
    //        exam.IsDelete = true;
    //        return await UpdateExam(exam);
    //    }

    //    public async Task<bool> UpdateExam(Exam exam)
    //    {
    //        try
    //        {
    //            _context.Exams.Update(exam);
    //            await _context.SaveChangesAsync();
    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }
    //}
}
