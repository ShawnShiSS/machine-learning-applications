using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MLApplications.API.Config
{
    /// <summary>
    ///     MediatR config
    /// </summary>
    public static class MediatrConfig
    {
        /// <summary>
        ///     MediatR
        /// </summary>
        /// <param name="services"></param>
        public static void SetupMediatr(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MLApplications.API.Infrastructure.Behaviours.ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MLApplications.API.Infrastructure.Behaviours.UnhandledExceptionBehaviour<,>));
        }
    }
}
