﻿using Microsoft.Azure.Cosmos.Table;

namespace HacktoberfestProject.Web.Models.Entities
{
    public class TrackerEntryEntity : TableEntity
    {
        private const string ROWKEY_TEMPLATE = "{0}:{1}:{2}";

        [IgnoreProperty]
        public string Username
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        public string Url { get; set; }
    }
}
