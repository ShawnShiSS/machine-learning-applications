using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MLApplications.Infrastructure.Data
{
    public class CosmosDbContainer : ICosmosDbContainer
    {
        public Container _container { get; }

        // CosmosClient should be a singleton instance passed in here
        public CosmosDbContainer(CosmosClient cosmosClient,
                                 string databaseName,
                                 string containerName)
        {
            this._container = cosmosClient.GetContainer(databaseName, containerName);
        }
    }
}
