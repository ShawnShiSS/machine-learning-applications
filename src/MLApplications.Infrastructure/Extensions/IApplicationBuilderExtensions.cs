using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MLApplications.Core.Interfaces;
using MLApplications.Core.Specifications;
using MLApplications.Infrastructure.Data;

namespace MLApplications.Infrastructure.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Create Cosmos DB if not exist
        /// </summary>
        /// <param name="builder"></param>
        public static void EnsureCosmosDbIsCreated(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var factory = serviceScope.ServiceProvider.GetService<ICosmosDbContainerFactory>();

                factory.EnsureDbSetupAsync().Wait();
            
            }
        }

        /// <summary>
        ///     Seed application data in Cosmos DB
        /// </summary>
        /// <param name="builder"></param>
        public static void SeedApplicationData(this IApplicationBuilder builder)
        {
            using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var webCommentRepository = serviceScope.ServiceProvider.GetService<IWebCommentRepository>();
                var countSpecification = new WebCommentSearchAggregationSpecification("");
                var count = webCommentRepository.GetItemsCountAsync(countSpecification).Result;

                if(count == 0)
                {
                    foreach(var webComment in MLApplications.Infrastructure.Data.SeedData.WebComments)
                    {
                        // NOTE: must Wait() here
                        webCommentRepository.AddItemAsync(webComment).Wait();
                    }
                }

            }
        }
        
    }
}
