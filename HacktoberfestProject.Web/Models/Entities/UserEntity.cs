using System.Collections.Generic;

using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace HacktoberfestProject.Web.Models.Entities
{
    public class UserEntity : TableEntity
    {
        public const string PARTITIONKEY = "Users";

        [IgnoreProperty]
        public string Username
        {
            get => RowKey;
            set => RowKey = value;
        }

        [IgnoreProperty]
        public List<RepositoryEntity> RepositoryPrAddedTo { get; set; }

        public UserEntity()
		{
            PartitionKey = PARTITIONKEY;
		}

        public UserEntity(string username, List<RepositoryEntity> repositoryPrAddedTo = null) : this()
        {
            Username = username;
            RepositoryPrAddedTo = repositoryPrAddedTo;
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
