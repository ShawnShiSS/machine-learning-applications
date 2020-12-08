using System.Collections.Generic;
using System.Threading.Tasks;

namespace MLApplications.Infrastructure.Data
{
    /// <summary>
    ///     Wrapper for the Microsoft.Azure.Cosmos.CosmosClient.
    ///     This should be used as a singleton instance.
    ///     Per Microsoft documentation, CosmosClient is thread-safe. Its recommended to maintain a single instance of CosmosClient per lifetime of the application which enables efficient connection management and performance. 
    ///     Example usage, see <see cref="MLApplications.Infrastructure.Extensions.IServiceCollectionExtensions.AddCosmosDb(Microsoft.Extensions.DependencyInjection.IServiceCollection, string, string, string, List{string})"/>
    /// </summary>
    public interface ICosmosDbContainerFactory
    {
        /// <summary>
        ///     Returns a CosmosDbContainer wrapper
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        ICosmosDbContainer GetContainer(string containerName);

        /// <summary>
        ///     Ensure the database is created
        /// </summary>
        /// <returns></returns>
        Task EnsureDbSetupAsync();
    }
}
