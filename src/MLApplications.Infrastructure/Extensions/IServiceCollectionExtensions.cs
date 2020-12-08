using MLApplications.Infrastructure.CosmosDbData.Config;
using MLApplications.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace MLApplications.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        ///     Register a singleton instance of Cosmos Db Container Factory, which is a wrapper for the CosmosClient.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="endpointUrl"></param>
        /// <param name="primaryKey"></param>
        /// <param name="databaseName"></param>
        /// <param name="containers"></param>
        /// <returns></returns>
        public static IServiceCollection AddCosmosDb(this IServiceCollection services,
                                                     string endpointUrl,
                                                     string primaryKey,
                                                     string databaseName,
                                                     List<ContainerInfo> containers)
        {
            Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(endpointUrl, primaryKey);
            var cosmosDbClientFactory = new CosmosDbContainerFactory(client, databaseName, containers);

            // This will be done at the API level so we ONLY ensure db creation in development environment
            // cosmosDbClientFactory.EnsureDbSetupAsync().Wait();

            services.AddSingleton<ICosmosDbContainerFactory>(cosmosDbClientFactory);

            return services;
        }

    }
}
