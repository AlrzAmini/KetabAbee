using KetabAbee.Application.Convertors;
using KetabAbee.Application.DTOs;
using KetabAbee.Application.DTOs.Admin.Home;
using KetabAbee.Application.DTOs.Admin.User;
using KetabAbee.Application.DTOs.Admin.Wallet;
using KetabAbee.Application.DTOs.Paging;
using KetabAbee.Application.Extensions;
using KetabAbee.Application.Generators;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Application.Security;
using KetabAbee.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.Comment;
using KetabAbee.Domain.Models.User;

namespace KetabAbee.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletService _walletService;
        private readonly ICommentService _commentService;

        public UserService(IUserRepository userRepository, IWalletService walletService, ICommentService commentService)
        {
            _userRepository = userRepository;
            _walletService = walletService;
            _commentService = commentService;
        }

        public async Task<Domain.Models.User.User> RegisterUser(RegisterViewModel user)
        {
            var newUser = new Domain.Models.User.User
            {
                UserName = user.UserName.Sanitizer(),
                Email = user.Email.Sanitizer(),
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

            await _userRepository.RegisterUser(newUser);
            _userRepository.AddUserIp(new UserIp
            {
                UserId = newUser.UserId,
                Ip = user.UserIp
            });
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

        public async Task<Domain.Models.User.User> GetUserForLogin(LoginViewModel login)
        {
            return await _userRepository.IsUserRegistered(login.Email, PasswordHasher.EncodePasswordMd5(login.Password));
        }

        //public bool ActiveAccountByEmail(string emailActiveCode)
        //{
        //    return _userRepository.ActiveAccountByEmail(emailActiveCode);
        //}

        public Domain.Models.User.User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email.Sanitizer());
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
            var user = _userRepository.GetUserByEmail(email);

            var userInfo = new UserInformationInDashboardViewmodel
            {
                Email = user.Email,
                Mobile = user.Mobile,
                Age = user.Age,
                RegisterDate = user.RegisterDate,
                UserName = user.UserName,
                Wallet = user.WalletBalance,
                Address = user.Address,
                ImageName = user.AvatarName,
                BirthDate = user.BirthDay
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
                Wallet = user.WalletBalance
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
                BirthDay = user.BirthDay,
                Address = user.Address
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
                    edit.CurrentAvatar = CodeGenerator.GenerateUniqCode() + Path.GetExtension(edit.UserAvatar.FileName);

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

                user.Mobile = edit.Mobile.Sanitizer();
                user.AvatarName = edit.CurrentAvatar;
                user.BirthDay = edit.BirthDay;
                user.Age = edit.BirthDay.GetAgeByDateTime();
                user.Address = edit.Address.Sanitizer();

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
                user.Password = PasswordHasher.EncodePasswordMd5(newPass.Sanitizer());

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
                .Select(u => new UserForShowInUserListAdminViewModel
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

        public async Task<bool> DeleteUserById(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdWithIncludes(userId);

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

                #region delete user relations

                #region delete user roles

                foreach (var userRoleId in user.UserRoles.Select(ur => ur.UserRoleId))
                {
                    _userRepository.DeleteUserRoleByUserRoleId(userRoleId);
                }

                #endregion

                #region delete user tickets

                _userRepository.DeleteUserTickets(user.Tickets.Select(t => t.TicketId).ToList());

                #endregion

                #region delete user ticket answers

                _userRepository.DeleteUserTicketAnswers(user.TicketAnswers.Select(t => t.AnswerId).ToList());

                #endregion

                #region delete user wallet

                _userRepository.DeleteUserWallets(user.Wallets.Select(t => t.WalletId).ToList());

                #endregion

                #region delete user favs

                _userRepository.DeleteUserFavs(user.FavoriteBooks.Select(t => t.LikeId).ToList());

                #endregion

                #region delete user orders

                _userRepository.DeleteUserOrders(user.Orders.Select(t => t.OrderId).ToList());

                #endregion

                #region delete user books

                _userRepository.DeleteUserBooks(user.UserBooks.Select(t => t.UserProductId).ToList());

                #endregion

                #region delete user scores

                _userRepository.DeleteUserScores(user.BookScores.Select(t => t.ScoreId).ToList());

                #endregion

                #region delete user comments

                _userRepository.DeleteUserComments(user.ProductComments.Select(t => t.CommentId).ToList());

                #endregion

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

        public async Task<string> GetAvatarNameByUserId(int userId)
        {
            return await _userRepository.GetAvatarNameByUserId(userId);
        }

        public async Task<int> AddUser(AddUserFromAdminViewModel user, IFormFile imgFile)
        {
            Domain.Models.User.User newUser = new();

            if (user == null) return 0;

            #region Check and add Avatar

            AddAvatar(imgFile, newUser);

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
            return await _userRepository.RegisterUser(newUser);
        }

        private static void AddAvatar(IFormFile imgFile, Domain.Models.User.User newUser)
        {
            if (imgFile != null && imgFile.IsImage())
            {
                // generate new image path and name
                newUser.AvatarName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(imgFile.FileName);

                string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar", newUser.AvatarName);

                // save avatar in file
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
        }

        public bool EmailActivatorBy5ThCode(string activateCode)
        {
            var user = _userRepository.GetUserByEmailActive5ThCode(activateCode.Sanitizer());

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

        public EditUserViewModel GetUserForEditInAdmin(int userId)
        {
            return _userRepository.GetUsersForEditAdmin()
                .Where(u => u.UserId == userId)
                .Select(u => new EditUserViewModel()
                {
                    UserName = u.UserName,
                    AvatarName = u.AvatarName,
                    Mobile = u.Mobile,
                    Email = u.Email,
                    BirthDay = u.BirthDay,
                    UserId = u.UserId,
                    UserRoles = u.UserRoles.Select(r => r.RoleId).ToList(),
                    Address = u.Address
                }).Single();

        }

        public string GetUserNameByUserId(int userId)
        {
            return _userRepository.GetUserNameByUserId(userId);
        }

        public string GetEmailByUserId(int userId)
        {
            return _userRepository.GetEmailByUserId(userId);
        }

        public string GetMobileByUserId(int userId)
        {
            return _userRepository.GetMobileByUserId(userId);
        }

        public bool EditUserFromAdmin(EditUserViewModel user)
        {
            try
            {
                var newUser = _userRepository.GetUserById(user.UserId);

                newUser.Email = FixText.EmailFixer(user.Email);
                newUser.Mobile = user.Mobile;
                newUser.UserName = user.UserName;
                if (!string.IsNullOrEmpty(user.Password))
                {
                    newUser.Password = PasswordHasher.EncodePasswordMd5(user.Password);
                }
                newUser.BirthDay = user.BirthDay?.ToString(CultureInfo.InvariantCulture).StringShamsiToMiladi();
                newUser.UserId = user.UserId;
                newUser.Address = user.Address;

                #region Check and add Avatar

                EditAvatar(user, newUser);

                #endregion

                return _userRepository.UpdateUser(newUser);
            }
            catch
            {
                return false;
            }
        }

        private static void EditAvatar(EditUserViewModel user, Domain.Models.User.User newUser)
        {
            if (user.Avatar != null && user.Avatar.IsImage())
            {
                // delete old avatar if avatar changed
                if (user.AvatarName != "User.jpg")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar", user.AvatarName);

                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }

                    // delete old avatar thumb if avatar changed
                    string thumbDeletePath =
                        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar/thumb", user.AvatarName);

                    if (File.Exists(thumbDeletePath))
                    {
                        File.Delete(thumbDeletePath);
                    }
                }

                // generate new image path and name
                newUser.AvatarName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(user.Avatar.FileName);

                string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar", newUser.AvatarName);

                // save avatar in file
                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    user.Avatar.CopyTo(stream);
                }

                var imgResizer = new ImageConvertor();
                string imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Avatar/thumb", newUser.AvatarName);
                imgResizer.Image_resize(imgPath, imgThumbPath, 200);
            }
        }

        public ChargeWalletFromAdminViewModel GetChargeInfoForAdmin(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            var charge = new ChargeWalletFromAdminViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                Inventory = user.WalletBalance.ToString("#,0")
            };

            return charge;
        }

        public async Task<List<int>> GetUserFavBookIds(int userId)
        {
            return await _userRepository.GetUserFavBookIds(userId);
        }

        public string GetUserAddressByUserId(int userId)
        {
            return _userRepository.GetUserAddressByUserId(userId);
        }

        public IEnumerable<Domain.Models.User.User> GetLastNDaysUsers(int n)
        {
            return _userRepository.GetLastNDaysUsers(n);
        }

        public IEnumerable<UserForAutoCompleteViewModel> GetUsersForAutoComplete()
        {
            return _userRepository.GetUsers().Select(u => new UserForAutoCompleteViewModel
            {
                UserId = u.UserId,
                UserName = u.UserName
            });
        }

        public UsersStatisticsViewModel GetUsersStaticsForAdmin()
        {
            return new UsersStatisticsViewModel
            {
                AllUsersCount = _userRepository.AllUsersCount(),
                ValidUsersCount = _userRepository.ValidUsersCount(),
                OnlineUsersCount = _userRepository.OnlineUsersCount(),
                AdminsCount = _userRepository.AdminsCount(),
            };
        }

        public void AddUserIp(UserIp userIp)
        {
            if (!_userRepository.CheckUserIpIsNotRepetitious(userIp))
            {
                _userRepository.AddUserIp(userIp);
            }
        }

        public async Task<List<int>> GetUserRoleIds(int userId)
        {
            return await _userRepository.GetUserRoleIds(userId);
        }

        public async Task<UserInfoViewModel> GetUserForShowInUserInfo(int userId)
        {
            var user = await _userRepository.GetUserByIdWithIncludeIps(userId);
            if (user == null)
            {
                return new UserInfoViewModel();
            }

            return new UserInfoViewModel
            {
                UserName = user.UserName,
                AvatarName = user.AvatarName,
                Address = user.Address,
                Age = user.Age,
                BirthDay = user.BirthDay,
                Email = user.Email,
                EmailActivationCode = user.EmailActivationCode,
                IsEmailActive = user.IsEmailActive,
                IsMobileActive = user.IsMobileActive,
                IsOnline = user.IsOnline,
                Mobile = user.Mobile,
                MobileActivationCode = user.MobileActivationCode,
                RegisterDate = user.RegisterDate,
                UserId = user.UserId,
                LatestUserIp = user.UserIps.OrderByDescending(i => i.UserIpId).FirstOrDefault()?.Ip,
                WalletBalance = user.WalletBalance,
                IsBanned = user.IsBanned
            };
        }

        public List<string> GetUserIps(int userId)
        {
            return _userRepository.GetUserIps(userId);
        }

        public List<UserBooksScoresViewModel> GetUserBookScores(int userId)
        {
            return _userRepository.GetUserBookScores(userId)
                .Select(s => new UserBooksScoresViewModel
                {
                    UserName = s.User.UserName,
                    UserIp = s.UserIp,
                    BookName = s.Book.Name,
                    ContentScore = s.ContentScore,
                    QualityScore = s.QualityScore,
                    AverageScores = s.AverageScores,
                    ScoreDate = s.ScoreDate,
                    ScoreId = s.ScoreId,
                    UserId = s.UserId,
                    BookId = s.BookId
                }).ToList();
        }

        public List<UserFavoriteBooksViewModel> GetUserFavoriteBooks(int userId)
        {
            return _userRepository.GetUserFavoriteBooks(userId)
                .Select(f => new UserFavoriteBooksViewModel
                {
                    UserName = f.User.UserName,
                    BookName = f.Book.Name,
                    BookImageName = f.Book.ImageName,
                    LikeId = f.LikeId,
                    BookId = f.BookId
                }).ToList();
        }

        public List<UserOrderViewModel> GetUserOrders(int userId)
        {
            return _userRepository.GetUserOrders(userId)
                .Select(o => new UserOrderViewModel
                {
                    UserName = o.User.UserName,
                    Address = o.Address,
                    CreateDate = o.CreateDate,
                    IsFinally = o.IsFinally,
                    OrderId = o.OrderId,
                    OrderSum = o.OrderSum,
                    SendingProcessIsCompleted = o.SendingProcessIsCompleted
                })
                .ToList();
        }

        public List<UserProductCommentViewModel> GetUserProductComments(int userId)
        {
            return _userRepository.GetUserProductComments(userId)
                .Select(c => new UserProductCommentViewModel
                {
                    UserName = c.UserName,
                    Email = c.Email,
                    ProductId = c.ProductId,
                    Body = c.Body,
                    UserIp = c.UserIp,
                    CommentId = c.CommentId,
                    IsReadByAdmin = c.IsReadByAdmin,
                    SendDate = c.SendDate,
                    ProductName = c.Product.Name
                }).ToList();
        }

        public List<UserBookViewModel> GetUserBooks(int userId)
        {
            return _userRepository.GetUserBooks(userId)
                .Select(b => new UserBookViewModel
                {
                    BookName = b.Name,
                    BookImageName = b.ImageName,
                    BookId = b.BookId
                }).ToList();
        }

        public List<UserTicketsViewModel> GetUserTickets(int userId)
        {
            return _userRepository.GetUserTickets(userId)
                .Select(t => new UserTicketsViewModel
                {
                    Body = t.Body,
                    IsReadByAdmin = t.IsReadByAdmin,
                    IsReadBySender = t.IsReadBySender,
                    SenderName = t.Sender.UserName,
                    TicketId = t.TicketId,
                    TicketState = t.TicketState,
                    TicketSendDate = t.TicketSendDate,
                    TicketPriority = t.TicketPriority,
                    Title = t.Title
                }).ToList();
        }

        public List<UserWalletViewModel> GetUserWallets(int userId)
        {
            return _userRepository.GetUserWallets(userId)
                .Select(w => new UserWalletViewModel
                {
                    UserName = w.User.UserName,
                    CreateDate = w.CreateDate,
                    Amount = w.Amount,
                    Behalf = w.Behalf,
                    IsPay = w.IsPay,
                    WalletId = w.WalletId
                }).ToList();
        }

        public bool UpdateUserWalletBalance(int userId)
        {
            var user = _userRepository.GetUserById(userId);

            var userBalance = user.WalletBalance;
            var newBalance = _walletService.BalanceUserWallet(userId);

            if (userBalance == newBalance) return false;

            user.WalletBalance = newBalance;
            return UpdateUser(user);
        }

        public async Task<long> GetUserWalletBalance(int userId)
        {
            return await _userRepository.GetUserWalletBalance(userId);
        }

        public DeleteUserCommentsResult DeleteUserComments(int userId)
        {
            try
            {
                var userComments = _userRepository.GetUserProductComments(userId);
                if (!userComments.Any())
                {
                    return DeleteUserCommentsResult.NotHaveComment;
                }

                foreach (var comment in userComments)
                {
                    _commentService.DeleteComment(comment.CommentId);
                }

                return DeleteUserCommentsResult.Success;
            }
            catch
            {
                return DeleteUserCommentsResult.Error;
            }
        }

        public BanUserIpResult BanUser(int userId)
        {
            var userIps = GetUserIps(userId);
            if (!userIps.Any())
            {
                return BanUserIpResult.NotHaveIp;
            }

            foreach (var ip in userIps)
            {
                _userRepository.AddIpToBannedIps(userId, ip);
            }

            var user = _userRepository.GetUserById(userId);
            user.IsBanned = true;
            return UpdateUser(user) ? BanUserIpResult.Success : BanUserIpResult.Error;

        }

        public async Task<List<string>> GetBannedIps()
        {
            var userIps = await _userRepository.GetBannedIps();
            return userIps.Select(b => b.Ip).ToList();
        }

        public FreeUserIpResult FreeUser(int userId)
        {
            try
            {
                var userIps = GetUserIps(userId);
                if (!userIps.Any())
                {
                    return FreeUserIpResult.NotHaveIp;
                }

                foreach (var ip in userIps)
                {
                    _userRepository.RemoveIpFromBannedIps(userId, ip);
                }

                var user = _userRepository.GetUserById(userId);
                if (!user.IsBanned)
                {
                    return FreeUserIpResult.UserIsAlreadyFree;
                }
                user.IsBanned = false;
                return UpdateUser(user) ? FreeUserIpResult.Success : FreeUserIpResult.Error;
            }
            catch
            {
                return FreeUserIpResult.Error;
            }
        }

        public Domain.Models.User.User CreateNewEmailActiveCodeForUser(int userId)
        {
            var user = _userRepository.GetUserById(userId);

            user.EmailActivationCode = new Random().Next(10000, 99999).ToString();
            user.IsEmailActive = false;

            return UpdateUser(user) ? user : new Domain.Models.User.User();
        }

        public async Task<int> GetUserFavBookCount(int userId)
        {
            return await _userRepository.GetUserFavBookCount(userId);
        }

        public void DeleteUserTickets(List<int> ticketIds)
        {
            _userRepository.DeleteUserTickets(ticketIds);
        }

        public async Task<BanIpResult> BanUserByIp(string userIp)
        {
            try
            {
                if (await _userRepository.IsIpExistInBanneds(userIp))
                {
                    return BanIpResult.IpIsAlreadyExist;
                }

                if (await _userRepository.AddUserIpToBannedIps(userIp))
                {
                    return BanIpResult.Success;
                }

                return BanIpResult.Error;
            }
            catch
            {
                return BanIpResult.Error;
            }
        }

        public IEnumerable<Domain.Models.User.User> GetLastNHoursUsers(int n)
        {
            return _userRepository.GetLastNHoursUsers(n);
        }
    }
}
