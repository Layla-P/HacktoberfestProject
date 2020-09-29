using Microsoft.Azure.Cosmos.Table;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace HacktoberfestProject.Web.Models.Entities
{
    public class UserEntity : TableEntity
    {
        public Guid UserId { get; set; }

        [IgnoreProperty]
        public List<RepositoryEntity> RepositoryPrAddedTo { get; set; }

        public UserEntity()
		{

		}

        public UserEntity(Guid id, List<RepositoryEntity> repositoryPrAddedTo = null)
        {
            UserId = id;
            RepositoryPrAddedTo = repositoryPrAddedTo;
            PartitionKey = "Users";
            RowKey = UserId.ToString();
            
        }

        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            // This line will write partition key and row key, but not properties with IgnoreProperty attribute
            var propertiesToBeWritten = base.WriteEntity(operationContext);

            // Writing x manually as a serialized string.
            propertiesToBeWritten["jsonDetails"] = new EntityProperty(JsonConvert.SerializeObject(this.RepositoryPrAddedTo));
            return propertiesToBeWritten;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            if (properties.ContainsKey(nameof(this.RepositoryPrAddedTo)))
            {
                this.RepositoryPrAddedTo = JsonConvert.DeserializeObject<List<RepositoryEntity>>(properties["jsonDetails"].StringValue);
            }
        }
    }
}
