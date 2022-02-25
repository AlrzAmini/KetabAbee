using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.User;
using Task = KetabAbee.Domain.Models.Task.Task;

namespace KetabAbee.Data.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly KetabAbeeDBContext _context;

        public TaskRepository(KetabAbeeDBContext context)
        {
            _context = context;
        }

        public bool AddTask(Task task)
        {
            try
            {
                _context.Tasks.Add(task);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Role> GetRoles()
        {
            return _context.Roles;
        }
    }
}
