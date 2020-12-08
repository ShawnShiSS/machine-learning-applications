using Ardalis.Specification;
using MLApplications.Core.Entities.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MLApplications.Core.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity//, IAggregateRoot
    {
        
        Task<T> GetItemAsync(string id);
        /// <summary>
        ///     Create an item and return id
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<string> AddItemAsync(T item);
        Task UpdateItemAsync(string id, T item);
        Task DeleteItemAsync(string id);

        /// <summary>
        ///     Soft delete an entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task MarkAsDeletedAsync(string id);

        /// <summary>
        ///     Get a list of items that match the specification
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetItemsAsync(ISpecification<T> specification);

        /// <summary>
        ///     Get the count on items that match the specification
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        Task<int> GetItemsCountAsync(ISpecification<T> specification);
    }
}
