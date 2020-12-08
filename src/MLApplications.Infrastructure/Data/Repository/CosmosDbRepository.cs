using Ardalis.Specification;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using MLApplications.Core.Entities.Base;
using MLApplications.Core.Enumerations;
using MLApplications.Core.Interfaces;
using MLApplications.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLApplications.Infrastructure.Data
{
    public abstract class CosmosDbRepository<T> : IAsyncRepository<T>, IContainerContext<T> where T : BaseEntity
    {
        /// <summary>
        ///     Name of the container
        /// </summary>
        public abstract string ContainerName { get; }
        
        /// <summary>
        ///     Generate the entity id.
        ///     This has to be abstract instead of virtual, as we do not have a default method for all types of repositories.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract string GenerateId(T entity);
        
        /// <summary>
        ///     Resolve the partition key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public abstract PartitionKey ResolvePartitionKey(string entityId);

        // Use the singleton ICosmosDbContainerFactory to construct a container
        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public CosmosDbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            this._cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            this._container = this._cosmosDbContainerFactory.GetContainer(ContainerName)._container;

        }

        public async Task<string> AddItemAsync(T item)
        {
            item.Id = GenerateId(item);
            item.EntityStatus = EntityStatus.Active;
            DateTime now = DateTime.UtcNow;
            item.DateCreatedUTC = now;
            item.DateModifiedUTC = now;
            // TODO (SS): update CreatedBy and ModifiedBy when authentication is in place
            await _container.CreateItemAsync<T>(item, ResolvePartitionKey(item.Id));

            return item.Id;
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<T>(id, ResolvePartitionKey(id));
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, ResolvePartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task UpdateItemAsync(string id, T item)
        {
            // update 
            item.DateModifiedUTC = DateTime.UtcNow;
            // TODO (SS): update ModifiedBy when authentication is in place
            await this._container.UpsertItemAsync<T>(item, ResolvePartitionKey(id));
        }

        public async Task MarkAsDeletedAsync(string id)
        {
            T entity = await GetItemAsync(id);
            if (entity != null)
            {
                entity.EntityStatus = EntityStatus.Deleted;
                await UpdateItemAsync(id, entity);
            }
        }

        /// <inheritdoc cref="IAsyncRepository{T}.GetItemsAsync(ISpecification{T})"/>
        public async Task<IEnumerable<T>> GetItemsAsync(ISpecification<T> specification)
        {
            var queryable = ApplySpecification(specification);
            var iterator = queryable.ToFeedIterator<T>();

            List<T> results = new List<T>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        /// <inheritdoc cref="IAsyncRepository{T}.GetItemsCountAsync(ISpecification{T})"/>
        public async Task<int> GetItemsCountAsync(ISpecification<T> specification)
        {
            var queryable = ApplySpecification(specification);
            // TODO (SS): investigate the performance and query request charge (not an issue until dataset is huge)
            // Aggregations are supported as of GitHub ticket: https://github.com/Azure/azure-cosmos-dotnet-v3/pull/729
            return await queryable.CountAsync();
        }

        /// <summary>
        ///     Evaluate specification and return IQueryable
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            var evaluator = new CosmosDbSpecificationEvaluator<T>();
            return evaluator.GetQuery(_container.GetItemLinqQueryable<T>(), specification);
        }

    }
}
