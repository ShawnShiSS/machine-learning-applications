using MLApplications.Core.Enumerations;
using System;
using System.Collections.Generic;


namespace MLApplications.Core.Entities.Base
{
    // This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
    public abstract class BaseEntity
    {
        /// <summary>
        ///     Id.
        ///     Note we have to use Newtonsoft.Json.JsonPropertyAttribute instead of System.Text.Json, 
        ///     as the latter one has issues. See https://github.com/Azure/azure-cosmos-dotnet-v3/issues/202.
        ///     Until System.Text.Json issue is addressed, infrastructure and API projects should try to use Newtonsoft.Json for consistency.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public virtual string Id { get; set; }

        /// <summary>
        ///     Events associated with the entity that can be used to trigger event handlers
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public List<BaseDomainEvent> EntityEvents = new List<BaseDomainEvent>();

        // AKA properties
        public virtual EntityStatus EntityStatus { get; set; }
        public virtual DateTime DateCreatedUTC { get; set; } 
        public virtual DateTime DateModifiedUTC { get; set; }
        public virtual Guid CreatedBy { get; set; } 
        public virtual Guid ModifiedBy { get; set; } 
    }
}
