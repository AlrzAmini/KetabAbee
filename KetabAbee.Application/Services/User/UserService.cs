﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.Generators;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Security;
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

        public Domain.Models.User.User RegisterUser(RegisterViewModel user)
        {
            var newUser = new Domain.Models.User.User()
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = PasswordHasher.EncodePasswordMd5(user.Password),
                EmailActivationCode = CodeGenerator.GenerateUniqCode(),
                MobileActivationCode = new Random().Next(100000, 999998).ToString(),
                AvatarName = "User.jpg",
                IsMobileActive = false,
                IsEmailActive = false,
                IsDelete = false,
                RegisterDate = DateTime.Now,
            };

            _userRepository.RegisterUser(newUser);

            return newUser;
        }

        public bool IsEmailExist(string email)
        {
            return _userRepository.IsEmailExist(email);
        }

        public bool IsUserNameExist(string userName)
        {
            return _userRepository.IsUserNameExist(userName);
        }

        ListUsersViewModel IUserService.GetUsers()
        {
            return new ListUsersViewModel()
            {
                Users = _userRepository.GetUsers()
            };
        }

        public bool IsMobileExist(string mobile)
        {
            return _userRepository.IsMobileExist(mobile);
        }

        public Domain.Models.User.User GetUserForLogin(LoginViewModel login)
        {
            return _userRepository.IsUserRegistered(login.Email, PasswordHasher.EncodePasswordMd5(login.Password));
        }

        public bool ActiveAccountByEmail(string emailActiveCode)
        {
            return _userRepository.ActiveAccountByEmail(emailActiveCode);
        }

        public Domain.Models.User.User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        public Domain.Models.User.User GetUserByEmailActivationCode(string emailActiveCode)
        {
            return _userRepository.GetUserByEmailActivationCode(emailActiveCode);
        }

        public async Task<bool> UpdateUser(Domain.Models.User.User user)
        {
            return await _userRepository.UpdateUser(user);
        }
    }
}
