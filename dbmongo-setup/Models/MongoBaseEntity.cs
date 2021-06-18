using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace dbmongo_setup.Models
{
    public class MongoBaseEntity
    {
        public MongoBaseEntity() {

        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement()]
        public DateTime CreatedAtUtc { get; set; }

        [BsonElement()]
        public DateTime UpdatedAtUtc { get; set; }
    }
}
