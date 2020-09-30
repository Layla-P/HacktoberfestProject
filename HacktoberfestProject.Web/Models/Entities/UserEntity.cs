using HacktoberfestProject.Web.Models.DTOs;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace HacktoberfestProject.Web.Models.Entities
{
	public class UserEntity : TableEntity
    {
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
		}

        public UserEntity(string username = null, List<RepositoryEntity> repositoryPrAddedTo = null)
        {
            Username = username;
            RepositoryPrAddedTo = repositoryPrAddedTo;
            PartitionKey = "Users";
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

        public static explicit operator User(UserEntity userEntity)
        {
            return new User(userEntity.Username, 
                            userEntity.RepositoryPrAddedTo?.Select(repo => new Repository(repo.Owner, 
                                                                                         repo.Name, 
                                                                                         null,
                                                                                         repo.PrEntities?.Select(pr => new Pr(pr.PrId, pr.Url)).ToList())
                                                                 ).ToList());
        }

        public static explicit operator UserEntity(User user)
        {
            return new UserEntity(user.Username,
                            user.RepositoryPrAddedTo?.Select(repo => new RepositoryEntity(repo.Owner,
                                                                                         repo.Name,
                                                                                         repo.Prs?.Select(pr => new PrEntity(pr.PrId, pr.Url)).ToList())
                                                                 ).ToList());
        }
    }
}
