using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Data.Mongo
{
    public class BaseDocument
    {
        [BsonId]
        public ObjectId ID { get; set; }
    }
}
