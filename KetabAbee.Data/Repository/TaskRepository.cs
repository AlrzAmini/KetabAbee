using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Data.Context;
using KetabAbee.Domain.Interfaces;
using KetabAbee.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
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

        public Task GetTaskById(int taskId)
        {
            return _context.Tasks.Find(taskId);
        }

        public Task GetTaskByIdWithIncludes(int taskId)
        {
            return _context.Tasks.Include(t => t.Role)
                .Include(t => t.Creator)
                .FirstOrDefault(t => t.TaskId == taskId);
        }

        public IEnumerable<Task> GetTaskForEachAdmin(int roleId)
        {
            return _context.Tasks.Include(t => t.Creator).Where(t => t.RoleId == roleId);
        }

        public IEnumerable<Task> GetTasks()
        {
            return _context.Tasks.Include(t => t.Creator)
                .Include(t => t.Role).OrderByDescending(t => t.TaskId);
        }

        public async Task<List<Task>> GetTasksByRoleIds(List<int> roleIds)
        {
            var listTasks = new List<Task>();

            foreach (var roleId in roleIds)
            {
                var tasks = await _context.Tasks.Include(t => t.Creator)
                    .OrderByDescending(t => t.TaskId)
                    .Where(t => t.RoleId == roleId && !t.IsCompleted).ToListAsync();
                if (tasks.Any())
                {
                    listTasks.AddRange(tasks);
                }
            }

            return listTasks;
        }

        public bool UpdateTask(Task task)
        {
            try
            {
                _context.Tasks.Update(task);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
