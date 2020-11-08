using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TodoApp.Data.Mongo
{
    public class BaseDocument
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId ID { get; set; }
    }
}
