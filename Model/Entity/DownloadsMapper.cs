using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItCommonLibrary.Model.Entity
{
    public sealed class DownloadsMapper
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string EntityId { get; set; }

        public string EntityType { get; set; }

        public string LocalFileName { get; set; }

        public DownloadsMapper() { }

        public DownloadsMapper(string userId, string entityId, string entityType, string localFileName)
        {
            UserId = userId;
            EntityId = entityId;
            EntityType = entityType;
            Id = $@"{userId}_{entityId}_{entityType}";
            LocalFileName = localFileName;
        }
    }
}
