using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;
using ZymLabs.NSwag.FluentValidation;

namespace MLApplications.API.Config
{
    /// <summary>
    ///     Swagger
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        ///     NSwag Swagger config
        /// </summary>
        /// <param name="services"></param>
        public static void SetupNSwag(this IServiceCollection services)
        {
            // Register the Swagger services
            services.AddOpenApiDocument((options, serviceProvider) => {
                options.DocumentName = "v1";
                options.Title = "MLApplications API";
                options.Version = "v1";

                // FluentValidation, add the fluent validations schema processor
                var fluentValidationSchemaProcessor = serviceProvider.GetService<FluentValidationSchemaProcessor>();
                options.SchemaProcessors.Add(fluentValidationSchemaProcessor);

                options.OperationProcessors.Add(new OperationSecurityScopeProcessor("auth"));
                options.DocumentProcessors.Add(new SecurityDefinitionAppender("auth", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Scheme = "bearer",
                    BearerFormat = "jwt"
                }));
            });

            // Add the FluentValidationSchemaProcessor as a singleton
            services.AddSingleton<FluentValidationSchemaProcessor>();
        }
    }
}
