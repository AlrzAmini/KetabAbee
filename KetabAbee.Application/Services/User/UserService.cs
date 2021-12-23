using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Admin.User;
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

        public bool UpdateUser(Domain.Models.User.User user)
        {
            return _userRepository.UpdateUser(user);
        }

        public UserInformationInDashboardViewmodel GetInfoByUserEmail(string email)
        {
            var user =  _userRepository.GetUserByEmail(email);

            var userInfo =  new UserInformationInDashboardViewmodel
            {
                Email = user.Email,
                Mobile = user.Mobile,
                Age = user.Age,
                RegisterDate = user.RegisterDate,
                UserName = user.UserName
            };

            return userInfo;

        }

        public UserPanelSideBarViewmodel GetUserPanelSideBarInfoByEmail(string userName)
        {
            var user = _userRepository.GetUserByUserName(userName);

            var sideBarInfo = new UserPanelSideBarViewmodel
            {
                Email = user.Email,
                AvatarName = user.AvatarName,
                Name = user.UserName,
            };

            return sideBarInfo;
        }

        public UserPanelEditInfoViewModel GetInfoForEditInUserPanel(string userName)
        {
            var user = _userRepository.GetUserByUserName(userName);

            var infoForEdit = new UserPanelEditInfoViewModel
            {
                Mobile = user.Mobile,
                Age = user.Age,
                CurrentAvatar = user.AvatarName,
                Email = user.Email
            };

            return infoForEdit;
        }

        public bool EditUserProfile(UserPanelEditInfoViewModel edit)
        {
            try
            {
                if (edit.UserAvatar != null && edit.UserAvatar.IsImage())
                {
                    string imgPath = "";
                    if (edit.CurrentAvatar != "User.jpg")
                    {
                        // get old avatar path
                        imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar", edit.CurrentAvatar);

                        // delete old avatar
                        if (File.Exists(imgPath))
                        {
                            File.Delete(imgPath);
                        }
                    }

                    // generate new image path and name
                    edit.CurrentAvatar = CodeGenerator.GenerateUniqCode()+Path.GetExtension(edit.UserAvatar.FileName);

                    imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar", edit.CurrentAvatar);
                
                    // save file in file
                    using (var stream = new FileStream(imgPath, FileMode.Create))
                    {
                        edit.UserAvatar.CopyTo(stream);
                    }
                }

                // update user
                var user = GetUserByEmail(edit.Email);

                user.Mobile = edit.Mobile;
                user.Age = edit.Age;
                user.AvatarName = edit.CurrentAvatar;

                UpdateUser(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsOldPasswordCorrect(string username, string oldPass)
        {
            return _userRepository.IsOldPasswordCorrect(username, PasswordHasher.EncodePasswordMd5(oldPass));
        }

        public bool ChangePasswordInUserPanel(string username, string newPass)
        {
            try
            {
                var user = _userRepository.GetUserByUserName(username);
                user.Password = PasswordHasher.EncodePasswordMd5(newPass);

                UpdateUser(user);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public IEnumerable<UserForShowInUserListAdminViewModel> GetAllUsersForAdmin()
        {
            return _userRepository.GetUsers()
                .Select(u=>new UserForShowInUserListAdminViewModel()
                {
                    Email = u.Email,
                    Mobile = u.Mobile,
                    RegisterDate = u.RegisterDate,
                    UserName = u.UserName,
                    ImageName = u.AvatarName,
                    IsActiveByEmail = u.IsEmailActive,
                    Userid = u.UserId
                });
        }

        public bool DeleteUserById(int userId)
        {
            try
            {
                var user = _userRepository.GetUserById(userId);
                user.IsDelete = true;

                UpdateUser(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
