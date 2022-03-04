using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using KetabAbee.Application.Convertors;
using KetabAbee.Application.Interfaces.Blog;
using KetabAbee.Application.Interfaces.Comment;
using KetabAbee.Application.Interfaces.Contact;
using KetabAbee.Application.Interfaces.Order;
using KetabAbee.Application.Interfaces.Permission;
using KetabAbee.Application.Interfaces.Product;
using KetabAbee.Application.Interfaces.Product.Discount;
using KetabAbee.Application.Interfaces.Task;
using KetabAbee.Application.Interfaces.Ticket;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Interfaces.Wallet;
using KetabAbee.Application.Services.Blog;
using KetabAbee.Application.Services.Comment;
using KetabAbee.Application.Services.Contact;
using KetabAbee.Application.Services.Order;
using KetabAbee.Application.Services.Permission;
using KetabAbee.Application.Services.Product;
using KetabAbee.Application.Services.Product.Discount;
using KetabAbee.Application.Services.Task;
using KetabAbee.Application.Services.Ticket;
using KetabAbee.Application.Services.User;
using KetabAbee.Application.Services.Wallet;
using KetabAbee.Data.Repository;
using KetabAbee.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace KetabAbee.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection service)
        {
            #region Cums From App Layer

            service.AddScoped<IUserService, UserService>();
            service.AddScoped<ITicketService, TicketService>();
            service.AddScoped<IWalletService, WalletService>();
            service.AddScoped<IPermissionService, PermissionService>();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<IOrderService, OrderService>();
            service.AddScoped<IContactService, ContactService>();
            service.AddScoped<ICommentService, CommentService>();
            service.AddScoped<IDiscountService, DiscountService>();
            service.AddScoped<IBlogService, BlogService>();
            service.AddScoped<ITaskService, TaskService>();

            #endregion

            #region Cums From Data Layer

            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<ITicketRepository, TicketRepository>();
            service.AddScoped<IWalletRepository, WalletRepository>();
            service.AddScoped<IPermissionRepository, PermissionRepository>();
            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<IOrderRepository, OrderRepository>();
            service.AddScoped<IContactRepository, ContactRepository>();
            service.AddScoped<ICommentRepository, CommentRepository>();
            service.AddScoped<IDiscountRepository, DiscountRepository>();
            service.AddScoped<IBlogRepository, BlogRepository>();
            service.AddScoped<ITaskRepository, TaskRepository>();

            #endregion

            #region html encode

            service.AddSingleton<HtmlEncoder>(
                HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic })
            );

            #endregion

            #region email

            service.AddScoped<IViewRenderService, RenderViewToString>();

            #endregion
        }
    }
}
