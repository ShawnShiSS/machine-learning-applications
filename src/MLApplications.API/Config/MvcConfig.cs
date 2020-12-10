using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MLApplications.API.Infrastructure.Filters;
using ZymLabs.NSwag.FluentValidation.AspNetCore;

namespace MLApplications.API.Config
{
    /// <summary>
    ///     Mvc
    /// </summary>
    public static class MvcConfig
    {
        /// <summary>
        ///     controllers config
        /// </summary>
        /// <param name="services"></param>
        public static void SetupControllers(this IServiceCollection services)
        {
            // API controllers
            services.AddControllers(options =>
                        // handle exceptions thrown from API endpoints
                        options.Filters.Add(new ApiExceptionFilter())
                    )
                    // Using Newtonsoft Json instead of System.Text.Json.Serialization 
                    // DO NOT mix and match using both NewtonSoft.Json and System.Text.Json.Serialization, as you may run into inconsistent behaviors!
                    .AddNewtonsoftJson(options => {
                        // Serilize enum in string
                        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    })
                    // This adds validation on model binding
                    .AddFluentValidation(c =>
                    {
                        c.RegisterValidatorsFromAssemblyContaining<Startup>();

                        // Optionally set validator factory if you have problems with scope resolve inside validators.
                        c.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
                    })
                    // Fix double execution issue : https://github.com/jasontaylordev/NorthwindTraders/issues/76
                    .AddMvcOptions(options => {
                        // Clear the default MVC model validations, as we are registering all model validators using FluentValidation
                        // https://github.com/jasontaylordev/NorthwindTraders/issues/76
                        options.ModelMetadataDetailsProviders.Clear();
                        options.ModelValidatorProviders.Clear();
                    });
        }
    }
}
