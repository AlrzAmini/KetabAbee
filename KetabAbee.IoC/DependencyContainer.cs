using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
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


            #region html encode

            service.AddSingleton<HtmlEncoder>(
                HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic })
            );

            #endregion


            #region Authentication



            #endregion


        }
    }
}
