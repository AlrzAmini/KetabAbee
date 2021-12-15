using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KetabAbee.Application.Interfaces.User;
using KetabAbee.Application.Services.User;
using KetabAbee.Data.Repository;
using KetabAbee.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace KetabAbee.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection service)
        {
            //Cums From App Layer
            service.AddScoped<IUserService, UserService>();

            //Cums From Data Layer
            service.AddScoped<IUserRepository, UserRepository>();


            #region Authentication

           

            #endregion

           
        }
    }
}
