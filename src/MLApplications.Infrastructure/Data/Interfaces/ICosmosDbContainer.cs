using Microsoft.Azure.Cosmos;

namespace MLApplications.Infrastructure.Data
{
    public interface ICosmosDbContainer
    {
        Container _container { get; }
    }
}
