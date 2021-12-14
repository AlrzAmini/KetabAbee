using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Domain.Interfaces;

namespace KetabAbee.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        ListUsersViewModel IUserService.GetUsers()
        {
            return new ListUsersViewModel()
            {
                Users = _userRepository.GetUsers()
            };
        }
    }
}
