using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Admin.User;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Generators;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KetabAbee.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletService _walletService;

        public UserService(IUserRepository userRepository, IWalletService walletService)
        {
            _userRepository = userRepository;
            _walletService = walletService;
        }

        public Domain.Models.User.User RegisterUser(RegisterViewModel user)
        {
            var newUser = new Domain.Models.User.User()
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = PasswordHasher.EncodePasswordMd5(user.Password),
                EmailActivationCode = new Random().Next(10000, 99999).ToString(),
                MobileActivationCode = new Random().Next(100000, 999998).ToString(),
                AvatarName = "User.jpg",
                IsMobileActive = false,
                IsEmailActive = false,
                IsDelete = false,
                RegisterDate = DateTime.Now,
                BirthDay = null,
                Age = null
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
            return mobile != null && _userRepository.IsMobileExist(mobile);
        }

        public Domain.Models.User.User GetUserForLogin(LoginViewModel login)
        {
            return _userRepository.IsUserRegistered(login.Email, PasswordHasher.EncodePasswordMd5(login.Password));
        }

        //public bool ActiveAccountByEmail(string emailActiveCode)
        //{
        //    return _userRepository.ActiveAccountByEmail(emailActiveCode);
        //}

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
                UserName = user.UserName,
                Wallet = _walletService.BalanceUserWallet(user.UserId)
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
                Wallet = _walletService.BalanceUserWallet(user.UserId)
            };

            return sideBarInfo;
        }

        public UserPanelEditInfoViewModel GetInfoForEditInUserPanel(string userName)
        {
            var user = _userRepository.GetUserByUserName(userName);

            var infoForEdit = new UserPanelEditInfoViewModel
            {
                Mobile = user.Mobile,
                CurrentAvatar = user.AvatarName,
                Email = user.Email,
                BirthDay = user.BirthDay
            };

            return infoForEdit;
        }

        public bool EditUserProfile(UserPanelEditInfoViewModel edit)
        {
            try
            {
                if (edit.UserAvatar != null && edit.UserAvatar.IsImage())
                {
                    string imgPath;
                    string imgThumbPath;
                    if (edit.CurrentAvatar != "User.jpg")
                    {
                        // get old avatar path
                        imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar", edit.CurrentAvatar);

                        // delete old avatar
                        if (File.Exists(imgPath))
                        {
                            File.Delete(imgPath);
                        }

                        // get old avatar thumb path
                        imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar/thumb", edit.CurrentAvatar);

                        // delete old thumb avatar
                        if (File.Exists(imgThumbPath))
                        {
                            File.Delete(imgThumbPath);
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

                    var imgResizer = new ImageConvertor();
                    imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar/thumb", edit.CurrentAvatar);
                    imgResizer.Image_resize(imgPath, imgThumbPath, 200);
                }

                // update user
                var user = GetUserByEmail(edit.Email);

                user.Mobile = edit.Mobile;
                user.AvatarName = edit.CurrentAvatar;
                user.BirthDay = edit.BirthDay;
                user.Age = edit.BirthDay.GetAgeByDateTime();

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

                #region Delete Avatar

                if (user.AvatarName != null)
                {
                    if (user.AvatarName != "User.jpg")
                    {
                        string imgDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar",
                            user.AvatarName);
                        if (File.Exists(imgDeletePath))
                        {
                            File.Delete(imgDeletePath);
                        }

                        string thumbDeletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar/thumb",
                            user.AvatarName);
                        if (File.Exists(thumbDeletePath))
                        {
                            File.Delete(thumbDeletePath);
                        }
                    }
                }

                #endregion

                user.AvatarName = "User.jpg";
                user.IsDelete = true;

                UpdateUser(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public FilterUsersViewModel GetAllFilteredUsersInAdmin(FilterUsersViewModel filter)
        {
            var result = _userRepository.GetUsers().AsQueryable();

            #region Filters

            //filter by create date
            switch (filter.OrderBy)
            {
                case FilterUserOrder.RegisterDateAsc:
                    result = result.OrderBy(u => u.RegisterDate);
                    break;
                case FilterUserOrder.RegisterDateDsc:
                    result = result.OrderByDescending(u => u.RegisterDate);
                    break;
            }

            //filter by username
            if (!string.IsNullOrEmpty(filter.UserName))
            {
                result = result.Where(u => u.UserName.Contains(filter.UserName));
            }

            //filter by email
            if (!string.IsNullOrEmpty(filter.Email))
            {
                result = result.Where(u => u.Email.Contains(filter.Email));
            }

            //filter by mobile
            if (!string.IsNullOrEmpty(filter.Mobile))
            {
                result = result.Where(u => u.Mobile.Contains(filter.Mobile));
            }

            #endregion

            #region Paging

            var pager = Pager.Build(filter.PageNum, result.Count(), filter.Take, filter.PageCountAfterAndBefor);
            var users = result.Paging(pager).ToList();

            #endregion

            return filter.SetPaging(pager).SetUsers(users);

        }

        public string GetAvatarNameByUserId(int userId)
        {
            return _userRepository.GetAvatarNameByUserId(userId);
        }

        public int AddUser(AddUserFromAdminViewModel user , IFormFile imgFile)
        {
            Domain.Models.User.User newUser = new();
                
            if (user == null) return 0;

            #region Check and add Avatar

            if (imgFile != null && imgFile.IsImage())
            {
                // generate new image path and name
                newUser.AvatarName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(imgFile.FileName);

                string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar", newUser.AvatarName);

                // save file in file
                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    imgFile.CopyTo(stream);
                }

                var imgResizer = new ImageConvertor();
                string imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar/thumb", newUser.AvatarName);
                imgResizer.Image_resize(imgPath, imgThumbPath, 200);
            }
            else
            {
                newUser.AvatarName = "User.jpg";
            }

            #endregion

            newUser.UserName = user.UserName;
            newUser.Email = FixText.EmailFixer(user.Email);
            newUser.Password = PasswordHasher.EncodePasswordMd5(user.Password);
            newUser.Mobile = user.Mobile;
            newUser.BirthDay = user.BirthDay?.ToString(CultureInfo.InvariantCulture).StringShamsiToMiladi();
            newUser.Age = newUser.BirthDay.GetAgeByDateTime();
            newUser.EmailActivationCode = new Random().Next(10000, 99999).ToString();
            newUser.IsEmailActive = true;
            newUser.MobileActivationCode = new Random().Next(100000, 999998).ToString();
            newUser.IsMobileActive = true;
            newUser.RegisterDate = DateTime.Now;
            _userRepository.RegisterUser(newUser);

            return newUser.UserId;
        }

        public bool EmailActivatorBy5ThCode(string activateCode)
        {
            var user = _userRepository.GetUserByEmailActive5ThCode(activateCode);

            // user not found
            if (user == null)
            {
                return false;
            }

            // user is already active
            if (user.IsEmailActive)
            {
                return false;
            }

            user.IsEmailActive = true;
            user.EmailActivationCode = new Random().Next(10000, 99999).ToString();
            _userRepository.UpdateUser(user);

            return true;
        }
    }
}
