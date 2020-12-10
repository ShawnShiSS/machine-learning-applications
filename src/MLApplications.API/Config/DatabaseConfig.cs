using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MLApplications.Core.Interfaces;
using MLApplications.Infrastructure.CosmosDbData.Config;
using MLApplications.Infrastructure.Data;
using MLApplications.Infrastructure.Extensions;

namespace MLApplications.API.Config
{
    /// <summary>
    ///     Database related configurations
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        ///     Setup Cosmos DB
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void SetupCosmosDb(this IServiceCollection services, IConfiguration configuration)
        {
            // Cosmos DB related bindings
            var cosmosDbConfig = configuration.GetSection("ConnectionStrings:MLApplications").Get<CosmosDbConfig>();
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);
            services.AddScoped<IWebCommentRepository, WebCommentRepository>();
            
        }

        
    }
}
