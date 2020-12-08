using MLApplications.Core.Entities;
using MLApplications.Core.Interfaces;
using MLApplications.Infrastructure.Data.Constants;
using Microsoft.Azure.Cosmos;
using System;

namespace MLApplications.Infrastructure.Data
{
    public class WebCommentRepository : CosmosDbRepository<WebComment>, IWebCommentRepository
    {
        /// <summary>
        ///     Name of the cosmosDb container where entity records will reside.
        /// </summary>
        public override string ContainerName { get; } = CosmosDbContainerConstants.CONTAINER_NAME_FEEDBACK;
        public override string GenerateId(WebComment entity) => $"{entity.FeedbackType}:{Guid.NewGuid()}";
        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public WebCommentRepository(ICosmosDbContainerFactory factory) : base(factory)
        { }


    }
}
