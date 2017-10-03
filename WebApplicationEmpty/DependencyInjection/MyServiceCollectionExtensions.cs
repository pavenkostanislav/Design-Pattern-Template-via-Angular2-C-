using KPMA.Constant;
using KPMA.Interfaces;
using KPMA.Managers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KPMA.Extensions.DependencyInjection
{
    public static class MyServiceCollectionExtensions
    {
        public static IServiceCollection AddKpi(this IServiceCollection services)
        {
            services.AddScoped<IGridManager<Data.Models.Chat>, EmployeeChatManager>();
			return services;
        }

    }
}
