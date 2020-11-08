using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TodoApp.Data.Mongo;

namespace TodoApp.Data.Model
{
    public class CardList : BaseDocument
    {
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [BsonElement("table")]
        [EnsureIndex(IndexType.Ascending, false)]
        [JsonIgnore]
        public ObjectId Table { get; set; }

        [BsonElement("content")]
        [JsonPropertyName("content")]
        public List<Card> Content { get; set; }
    }

    public class Card
    {
        [BsonElement("title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
