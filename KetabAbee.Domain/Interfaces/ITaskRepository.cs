﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Domain.Models.User;
using Task = KetabAbee.Domain.Models.Task.Task;

namespace KetabAbee.Domain.Interfaces
{
    public interface ITaskRepository
    {
        bool AddTask(Task task);

        IEnumerable<Role> GetRoles();
    }
}