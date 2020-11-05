using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TodoApp.Data.Mongo;

namespace TodoApp.Data.Model
{
    public class User : BaseDocument
    {
        [BsonElement("name")]
        [EnsureIndex(IndexType.Ascending, true)]
        public string Username { get; set; }

        [BsonElement("fullname")]
        public string FullName { get; set; }

        [BsonElement("pass")]
        public string PasswordHash { get; set; }

        [BsonElement("roles")]
        public string[] Roles { get; set; }
    }
}
