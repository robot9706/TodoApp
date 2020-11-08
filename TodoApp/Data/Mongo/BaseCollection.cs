using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TodoApp.Data.Mongo
{
    public class BaseCollection<T> where T: BaseDocument
    {
        protected static IMongoCollection<T> collection;

        internal static void Init(IMongoDatabase database, string collectionName)
        {
            collection = database.GetCollection<T>(collectionName);

            // Apply custom modifiers
            foreach (PropertyInfo propInfo in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propInfo.GetCustomAttribute<EnsureIndex>() is EnsureIndex indexAttribute)
                {
                    // Find the IndexDefinition based on the IndexType
                    IndexKeysDefinition<T> indexDef = null;
                    switch (indexAttribute.IndexType)
                    {
                        case IndexType.Ascending:
                            indexDef = Builders<T>.IndexKeys.Ascending(propInfo.Name);
                            break;
                        case IndexType.Descending:
                            indexDef = Builders<T>.IndexKeys.Descending(propInfo.Name);
                            break;
                    }

                    // Create the index
                    collection.Indexes.CreateOneAsync(new CreateIndexModel<T>(indexDef, new CreateIndexOptions()
                    {
                        Unique = indexAttribute.IsUnique
                    }));
                }
            }
        }

        public static List<T> GetAll()
        {
            return collection.Find<T>(_ => true).ToList();
        }

        public static void InsertOne(T data)
        {
            collection.InsertOne(data);
        }

        public static void InsertMany(List<T> data)
        {
            collection.InsertMany(data);
        }

        public static bool DeleteOne(T data)
        {
            // TODO: Return count?
            return collection.DeleteOne(Builders<T>.Filter.Eq(x => x.ID, data.ID)).DeletedCount > 0;
        }

        public static bool DeleteOneById(ObjectId id)
        {
            // TODO: Return count?
            return collection.DeleteOne(Builders<T>.Filter.Eq(x => x.ID, id)).DeletedCount > 0;
        }

        public static T FindOneById(ObjectId id)
        {
            if (id == ObjectId.Empty)
            {
                return default(T);
            }

            return collection.Find(Builders<T>.Filter.Eq(x => x.ID, id)).FirstOrDefault();
        }
    }
}
