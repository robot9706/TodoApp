using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using TodoApp.Data.Mongo;

namespace TodoApp.Data.Model
{
    public class Table : BaseDocument
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("owner")]
        [JsonIgnore]
        public ObjectId Owner { get; set; }
    }
}
