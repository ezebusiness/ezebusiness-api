using dbmongo_setup.Models;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace cafe_common.Models
{
    public class User : MongoBaseEntity
    {
        public string Username { get; set; }
        public string[] RefreshTokens { get; set; }
    }
}
